using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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
	public	int					base_str;
	public	int					str;
	public	int					base_agi;
	public	int					agi;
	public	int					base_con;	
	public	int					con;
	public	int					xp;
	public	int					money;
	[Range(1, 50)]
	public	int					level;
	public	int					current_mana;
	public	int					bonus_damage;
	public	int					bonus_hp;
	public	int					bonus_mana;
	[Range(0, 49)]
	public	int					skillPoints;	

	// Skills
	public	SkillScript[]		skills;
	public 	SkillScript			currentSkill;

	// Equipment
	public	WeaponScript		weapon;
	public	ArmorScript			armor;

	// Calculated Stats
	public	int					minDamage { get { return str / 2 + bonus_damage;}}
	public	int					maxDamage { get { return minDamage + weaponDamage;}}
	public	int					hpMax { get { return 5 * con + bonus_hp; } }
	public	int					manaMax { get { return 100 + bonus_mana; }}
	public	int					weaponDamage { get { return weapon == null || !weapon.equipped ? 0 : weapon.damage; }}
	public	int					armorValue { get { return armor == null || !weapon.equipped ? 0 : armor.armorValue; }} 
	public	float				weaponCoolDown { get { return weapon == null || !weapon.equipped ? 2.5f : weapon.coolDown; }}
	public	float				weaponRange { get { return weapon == null || !weapon.equipped ? 2f : weapon.range; }}

	// Use this for initialization
	void Start () {
		current_hp = hpMax;
		instance = this;
		_attackType = AttackType.NONE;
		_navMeshAgent = GetComponent<NavMeshAgent>();
		animator = GetComponentInChildren<Animator>();
		_sphereCollider = GetComponentInChildren<SphereCollider>();
		skills = new SkillScript[4];
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
		if (CurrentSkillIsDirectAttack() && Input.GetMouseButtonDown (0) && !UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
		{
			RaycastHit hit;
			if (Physics.Raycast (Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
			{
				if (hit.collider.tag == "Ground")
				{
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
					_navMeshAgent.stoppingDistance = _sphereCollider.radius;
					_enemyTarget = hit.collider.gameObject;
					if (_enemyTarget == null || _enemyTarget != hit.collider.gameObject)
						CancelAttackAnimation ();
					_attack = false;
					_enemyTargeting = true;
					_navMeshAgent.Resume();
				}
				else if (hit.collider.tag == "Weapon")
					InventoryScript.instance.addWeapon (hit.collider.gameObject.GetComponent<WeaponScript>());
			}
		}
	}

	IEnumerator onCoolDown ()
	{
		_onCoolDown = true;
		yield return new WaitForSeconds(weaponCoolDown);
		_onCoolDown = false;
	}

	bool targetInRange()
	{
		return (_enemyTarget != null && isInRange(_enemyTarget));
	}

	public void applyDamage()
	{
		if (NoSkillSelected())
		{
			if (_enemyTarget != null)
				_enemyTarget.GetComponent<Enemy>().RecieveDamage(GetDamage ());
		}
		else if (CurrentSkillIsDirectAttack())
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
		}
	}

	public void attachWeapon(WeaponScript weapon)
	{
		weapon.transform.SetParent (playerChest.transform);
		weapon.transform.localPosition = new Vector3(0.016f, 0.153f, -0.428f);
		weapon.transform.localRotation = Quaternion.Euler (276.4f, 161.7201f, 14.6008f);
		weapon.GetComponent<Rigidbody>().isKinematic = true;
		weapon.equipped = false;
		this.weapon = weapon;
		if (!animator.GetBool("HasWeapon"))
			animator.SetTrigger("Equip");
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
		}
	}

	public bool isInRange(GameObject target)
	{
		return (_enemyTargetsInRange.Find (x => x == target) != null);
	}

	void OnTriggerEnter(Collider col)
	{
		_enemyTargetsInRange.Add (col.gameObject);
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
	
	// Update is called once per frame
	void Update () {
		if (_dead)
			return ;
		ManageSkills();
		UpdateRange();
		AttackEnemy ();
		FollowMouse();
		DeathAnimation();
		//UpdateSpeed ();
		RunAnimation();
	}

	void LateUpdate()
	{
		UpdateRange ();
	}
}