using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(Animator))]
public class PlayerScript : MonoBehaviour {
	public enum AttackType
	{
		WEAPON_ATTACK,
		SPELL_ATTACK,
		NONE
	}
	// Singleton
	public static PlayerScript	instance;

	// Components
	private NavMeshAgent		_navMeshAgent;
	public	Animator			animator;
	private SphereCollider		_sphereCollider;

	// Utility
	public 	bool				_dead;
	public	bool				_attack;
	public	bool				_enemyTargeting;
	public 	bool				AOETargeting;
	public	bool				_onCoolDown;
	public 	GameObject			_enemyTarget;

	public	List<GameObject>	_enemyTargetsInRange;
	public	AttackType			_attackType;
	public	GameObject			playerRightHand;
	public	GameObject			playerChest;

	// Audio
	public AudioSource			footstepsSound;
	public AudioSource[]		attackSounds;
	

	// Stats
	public	int					current_hp;
	[Tooltip("Strength value")]
	public	int					str;
	[Tooltip("Agility value")]
	public	int					agi;
	[Tooltip("Constitution value")]
	public	int					con;
	[Tooltip("Intelligence value")]
	public	int					intel;
	[Tooltip("Experience value")]
	public	long				xp;
	[Tooltip("Money value")]
	public	int					money;
	[Tooltip("Level value between 1 and 50")]
	[Range(1, 50)]
	public	int					level;
	public	int					current_mana;
	[Range(0, 49)]
	public	int					skillPoints;
	[Range(0, 2500)]
	public	int					statPoints;
	public	long[]				experienceCurve;
	public	LayerMask			raycastLayerMask;

	// Skills
	public	SkillScript[]		skills;
	public 	SkillScript			currentSkill;

	// Equipment
	public	WeaponScript		weapon;

	// Calculated Stat
	public	int					strTotal { get  { return (int)(str + buffs.str + str * (buffs.pStr / 100.0f)); }}
	public	int					agiTotal { get	{ return (int)(agi + buffs.agi + agi * (buffs.pAgi / 100.0f)); }}
	public	int					conTotal { get	{ return (int)(con + buffs.con + con * (buffs.pCon / 100.0f)); }}
	public	int					intelTotal { get { return (int)(intel + buffs.intel + intel * (buffs.pIntel / 100.0f)); }}
	public	int					minDamage { get { return (int)((strTotal / 2.0f) + (strTotal / 2.0f) * (buffs.pDamage / 100.0f) + buffs.damage);}}
	public	int					maxDamage { get { return minDamage + weaponDamage;}}
	public	int					hpMax { get { return (int)((5 * conTotal) + (5 * conTotal) * (buffs.pHp / 100.0f) + buffs.hp); } }
	public	int					manaMax { get { return (int)((50 + 5 * (intelTotal)) + (50 + 5 * (intelTotal)) * (buffs.pMp / 100.0f) + buffs.mana); }}
	public	int					weaponDamage { get { return weapon == null || !weapon.equipped ? 0 : weapon.damage; }}
	public	float				weaponCoolDown { get { return weapon == null || !weapon.equipped ? 2.5f : weapon.coolDown; }}
	public	float				weaponRange { get { return weapon == null || !weapon.equipped ? 2f : weapon.range; }}

	public	BuffScript			buffs = new BuffScript();
	
	public class PassiveStatChange
	{
		int			str;
		int			agi;
		int			con;
		int			intel;
		int			hp;
		int			mana;
		int			damage;
		int			attackSpeed;
		float		cooldownReduction;
	}

	// Use this for initialization
	void Start () {
		experienceCurve = new long[49];
		experienceCurve[0] = 15;
		for (long i = 1; i < experienceCurve.Length; i++)
			experienceCurve[i] = (int)(experienceCurve[i - 1] * 1.25f);
		current_hp = hpMax;
		current_mana = manaMax;
		instance = this;
		_attackType = AttackType.NONE;
		_navMeshAgent = GetComponent<NavMeshAgent>();
		animator = GetComponentInChildren<Animator>();
		_sphereCollider = GetComponentInChildren<SphereCollider>();
		skills = new SkillScript[4];
		StartCoroutine (RegenMana ());
	}

	public long GetNextLevelXp()
	{
		return (level > experienceCurve.Length) ? long.MaxValue : experienceCurve[level - 1];
	}

	public int GetDamage()
	{
		return Random.Range (minDamage, maxDamage + 1);
	}

	bool CurrentSkillIsDirectAttack()
	{
		return (currentSkill == null || (currentSkill.skillType == SkillScript.SkillType.DIRECT_ATTACK));
	}

	bool NoSkillSelected()
	{
		return (currentSkill == null);
	}

	void CancelAttackAnimation()
	{
		if (_attack)
		{
			currentSkill = null;
			_sphereCollider.radius = weaponRange;
			animator.SetTrigger ("Cancel");
			_attack = false;
		}
	}

	void FollowMouse ()
	{
		if (!AOETargeting && Input.GetMouseButtonDown (0) && !UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
		{
			RaycastHit hit;
			if (Physics.Raycast (Camera.main.ScreenPointToRay(Input.mousePosition), out hit, raycastLayerMask))
			{
				if (hit.collider.tag == "Ground")
				{
					Debug.Log ("Ground");
					_navMeshAgent.destination = hit.point;
					_navMeshAgent.stoppingDistance = 0.0f;
					CancelAttackAnimation ();
					currentSkill = null;
					_attack = false;
					_enemyTargeting = false;
					_enemyTarget = null;
					_navMeshAgent.Resume ();
				}
				else if (hit.collider.tag == "Enemy")
				{
					_navMeshAgent.destination = hit.point;
					_navMeshAgent.stoppingDistance = _sphereCollider.radius - 0.5f;
					_enemyTarget = hit.collider.gameObject;
					if (_enemyTarget == null || _enemyTarget != hit.collider.gameObject)
						CancelAttackAnimation ();
					_attack = false;
					_enemyTargeting = true;
					_navMeshAgent.Resume();
				}
				else if (hit.collider.tag == "Weapon" && hit.collider.gameObject.GetComponent<WeaponScript>() != weapon)
					InventoryScript.instance.addWeapon (hit.collider.gameObject.GetComponent<WeaponScript>());
			}
		}
	}

	bool targetInRange()
	{
		return (_enemyTarget != null && isInRange(_enemyTarget));
	}

	public void applyDamage()
	{
		if (NoSkillSelected ()) {
			if (_enemyTarget != null)
			{
				float val = 75 + agi - _enemyTarget.GetComponent<Enemy> ().agi - Random.Range (1, 101);
				bool hit = val > 0 ? true : false;

				if (hit)
					_enemyTarget.GetComponent<Enemy> ().ReceiveDamage (GetDamage ());
			}
		} else if (CurrentSkillIsDirectAttack ())
			currentSkill.ApplyEffect (_enemyTarget.transform.position, playerRightHand);
		else if (currentSkill.skillType == SkillScript.SkillType.SELF_AOE
			|| currentSkill.skillType == SkillScript.SkillType.PASSIVE_AOE
			|| currentSkill.skillType == SkillScript.SkillType.TARGETED_AOE)
			currentSkill.ApplyEffect (this.transform.position, gameObject);
	}

	public void attackOver()
	{
		_attack = false;
		currentSkill = null;
	}

	public void attackBegin()
	{
		_attack = true;
	}

	public void equip ()
	{
		if (weapon != null)
		{
			weapon.transform.SetParent (playerRightHand.transform);
			weapon.transform.localPosition = new Vector3(0, 0, 0);
			weapon.equipped = true;
			animator.SetBool ("HasWeapon", true);
			animator.SetFloat("AttackSpeed", weapon.attackSpeed);
		}
	}

	public void unequip()
	{
		if (weapon != null)
		{
			weapon.transform.SetParent (playerChest.transform);
			weapon.transform.localPosition = new Vector3(-0.063f, 0.099f, -0.43f);
			weapon.equipped = false;
			animator.SetBool ("HasWeapon", false);
			animator.SetFloat ("AttackSpeed", 0);
		}
	}

	public void attachWeapon(WeaponScript weapon)
	{	if (this.weapon != null)
		{
			this.weapon.transform.SetParent(null);
			this.weapon.GetComponent<Rigidbody>().isKinematic = false;
			this.weapon.equipped = false;
			this.weapon.gameObject.SetActive(false);
		}
		weapon.GetComponent<Rigidbody>().isKinematic = true;
		this.weapon = weapon;
		animator.SetFloat ("AttackSpeed", weapon.attackSpeed);
		if (animator.GetBool("HasWeapon"))
		{
			weapon.transform.SetParent (playerRightHand.transform);
			weapon.transform.localPosition = Vector3.zero;
			weapon.transform.localRotation = Quaternion.Euler (69.72504f, 54.6933f, 209.1173f);
			weapon.equipped = true;
		}
		else
		{
			weapon.transform.SetParent (playerChest.transform);
			weapon.transform.localPosition = new Vector3(0.016f, 0.153f, -0.428f);
			weapon.transform.localRotation = Quaternion.Euler (276.4f, 161.7201f, 14.6008f);
			weapon.equipped = false;
			animator.SetTrigger("Equip");
		}
	}

	void AttackEnemy()
	{
		if (!_attack && _enemyTargeting && targetInRange () && _navMeshAgent.velocity == Vector3.zero)
		{
			if (NoSkillSelected () && !_onCoolDown)
			{
				_navMeshAgent.Stop ();
				StartCoroutine (onCoolDown());
				transform.LookAt (new Vector3 (_enemyTarget.transform.position.x, this.transform.position.y, _enemyTarget.transform.position.z));
				animator.SetInteger ("AttackType", 7);
				animator.SetTrigger ("WeaponAttack");
				//attackSounds[Random.Range (0, attackSounds.Length)].Play ();
				if (!Input.GetMouseButton (0))
					_enemyTargeting = false;
			}
			if (!NoSkillSelected () && currentSkill.skillType == SkillScript.SkillType.DIRECT_ATTACK && !currentSkill.onCoolDown)
			{
				_navMeshAgent.Stop ();
				currentSkill.UseSkill ();
				transform.LookAt (new Vector3 (_enemyTarget.transform.position.x, this.transform.position.y, _enemyTarget.transform.position.z));
				_enemyTargeting = false;
			}
		}
	}

	void RunAnimation ()
	{
		if (Mathf.Abs(_navMeshAgent.remainingDistance - _navMeshAgent.stoppingDistance) < 0.1f)
		{
			_navMeshAgent.Stop();
			_navMeshAgent.velocity = Vector3.zero;
		}
		animator.SetBool("Run", _navMeshAgent.velocity != Vector3.zero);
	}

	void DeathAnimation ()
	{
		if (!_dead && current_hp <= 0)
		{
			_navMeshAgent.Stop ();
			animator.SetTrigger("Death");
			animator.SetInteger ("RandomDeath", Random.Range(0, 4));
			_dead = true;
			StartCoroutine (PrepareToReload());
		}
	}

	public bool isInRange(GameObject target)
	{
		return (_enemyTargetsInRange.Find (x => x == target) != null);
	}

	void OnTriggerEnter(Collider col)
	{
		if (col.gameObject.tag == "Enemy") 
			_enemyTargetsInRange.Add (col.gameObject);
		if (col.gameObject.tag == "Potion") {
			current_hp = Mathf.Clamp (current_hp + (int) (hpMax * 0.30), 0, hpMax);
			Destroy (col.gameObject);
		}
	}
	
	void OnTriggerExit(Collider col)
	{
		_enemyTargetsInRange.Remove (col.gameObject);
	}


	void UpdateRange ()
	{
		if (!NoSkillSelected () && CurrentSkillIsDirectAttack())
			_sphereCollider.radius = currentSkill.range;
		else
			_sphereCollider.radius = weaponRange;
	}

	void SelectSkill(int index)
	{
		if (_attack || skills[index] == null)
			return ;
		currentSkill = (skills[index].SelectSkill()) ? skills[index] : null;
	}

	void ManageSkills ()
	{
		if (Input.GetKeyDown(KeyCode.Alpha1))
			SelectSkill (0);
		if (Input.GetKeyDown (KeyCode.Alpha2))
			SelectSkill (1);
		if (Input.GetKeyDown (KeyCode.Alpha3))
			SelectSkill (2);
		if (Input.GetKeyDown (KeyCode.Alpha4))
			SelectSkill (3);
		if (weapon != null && Input.GetKeyDown ("e"))
		    animator.SetTrigger ("Equip");
	}

	void CheatCode () 
	{
		if (Input.GetKeyUp ("l"))
			xp += GetNextLevelXp();
	}

	void LevelUp()
	{
		if (xp > GetNextLevelXp ())
		{
			xp -= GetNextLevelXp ();
			level++;
			skillPoints++;
			statPoints += 5;
		}
	}

	IEnumerator RegenMana () {
		while (true) {
			yield return new WaitForSeconds(1.0f);
			current_mana = Mathf.Clamp(current_mana + 1, 0, manaMax);
		}
	}

	IEnumerator PrepareToReload ()
	{
		yield return new WaitForSeconds (5.0f);
		Application.LoadLevel (Application.loadedLevel);
	}

	IEnumerator onCoolDown ()
	{
		_onCoolDown = true;
		yield return new WaitForSeconds(weaponCoolDown);
		_onCoolDown = false;
	}

	public void DamagePlayer (int damage)
	{
		this.current_hp = Mathf.Clamp (this.current_hp - damage, 0, this.hpMax);
	}

	public void StopMoving()
	{
		this._navMeshAgent.Stop ();
	}

	// Update is called once per frame
	void Update () {
		if (_dead)
			return ;
		ManageSkills();
		UpdateRange();
		AttackEnemy ();
		RunAnimation();
		FollowMouse();
		DeathAnimation();
		LevelUp();
		//UpdateSpeed ();
		CheatCode ();
	}

	void LateUpdate()
	{
		UpdateRange ();
	}
}