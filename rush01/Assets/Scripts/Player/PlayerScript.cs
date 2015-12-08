using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(Animator))]
public class PlayerScript : CharacterScript {
	// Singleton
	public static PlayerScript	instance;

	// Utility
	public 	bool				_dead;
	public	bool				_attack;
	public	bool				enemyTargeting;
	public 	bool				AOETargeting;

	public	List<CharacterScript>	enemyTargetsInRange;
	public	GameObject			playerRightHand;
	public	GameObject			playerChest;

	// Audio
	public AudioSource			footstepsSound;
	public AudioSource[]		attackSounds;

	// Stats
	[Range(0, 49)]
	public	int					skillPoints;
	[Range(0, 2500)]
	public	int					statPoints;
	public	long[]				experienceCurve;

	// Skills
	public	SkillScript[]		skills;
	public 	SkillScript			currentSkill;

	// Equipment
	public	WeaponScript		weapon;

	// Calculated Stat

	public override int			weaponDamage { get { return weapon == null || !weapon.equipped ? 0 : weapon.damage; }}
	public override float		attackCooldown { get { return weapon == null || !weapon.equipped ? 2.5f : weapon.coolDown; }}
	public	float				weaponRange { get { return weapon == null || !weapon.equipped ? 2f : weapon.range; }}

	// Use this for initialization
	protected override void Start () {
		base.Start ();
		experienceCurve = new long[49];
		experienceCurve[0] = 15;
		for (long i = 1; i < experienceCurve.Length; i++)
			experienceCurve[i] = (int)(experienceCurve[i - 1] * 1.25f);

		instance = this;
		skills = new SkillScript[4];
	}

	public override bool isInRange (CharacterScript target)
	{
		return (enemyTargetsInRange.Find (x => x == target) != null);
	}

	public override void AttackSound ()
	{
		return;
	}

	public override void DamageSound ()
	{
		return;
	}

	public long GetNextLevelXp()
	{
		return (level > experienceCurve.Length) ? long.MaxValue : experienceCurve[level - 1];
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
			sphereCollider.radius = weaponRange;
			enemyTargeting = false;
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
					navMeshAgent.destination = hit.point;
					navMeshAgent.stoppingDistance = 0.0f;
					CancelAttackAnimation ();
					currentSkill = null;
					_attack = false;
					enemyTargeting = false;
					enemyTarget = null;
					navMeshAgent.Resume ();
				}
				else if (hit.collider.tag == "Enemy")
				{
					navMeshAgent.destination = hit.point;
					navMeshAgent.stoppingDistance = sphereCollider.radius - 0.5f;
					enemyTarget = hit.collider.gameObject.GetComponent<CharacterScript>();
					if (enemyTarget == null || enemyTarget != hit.collider.gameObject)
						CancelAttackAnimation ();
					_attack = false;
					enemyTargeting = true;
					navMeshAgent.Resume();
					if (weapon != null && !weapon.equipped)
						animator.SetTrigger("Equip");
				}
				else if (hit.collider.tag == "Weapon" && hit.collider.gameObject.GetComponent<WeaponScript>() != weapon)
					InventoryScript.instance.addWeapon (hit.collider.gameObject.GetComponent<WeaponScript>());
			}
		}
	}

	bool targetInRange()
	{
		return (enemyTarget != null && isInRange(enemyTarget));
	}

	public void applyDamage()
	{
		if (NoSkillSelected ()) {
			if (enemyTarget != null)
			{
				float val = 75 + agi - enemyTarget.GetComponent<Enemy> ().agi - Random.Range (1, 101);
				bool hit = val > 0 ? true : false;

				if (hit)
					enemyTarget.GetComponent<Enemy> ().ReceiveDamage (GetDamage ());
				else
					enemyTarget.GetComponent<Enemy>().ReceiveDamage (0, true);
			}
		} else if (CurrentSkillIsDirectAttack ())
			currentSkill.ApplyEffect (enemyTarget.transform.position, playerRightHand);
		else if (currentSkill.skillType == SkillScript.SkillType.SELF_AOE
			|| currentSkill.skillType == SkillScript.SkillType.PASSIVE_AOE
			|| currentSkill.skillType == SkillScript.SkillType.TARGETED_AOE)
			currentSkill.ApplyEffect (this.transform.position, gameObject);
	}

	public void attackOver()
	{
		_attack = false;
		currentSkill = null;
		if (!Input.GetMouseButton (0))
			enemyTargeting = false;
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

	protected override IEnumerator onCoolDown ()
	{
		_onCoolDown = true;
		yield return new WaitForSeconds(attackCooldown);
		_onCoolDown = false;
		if (!Input.GetMouseButton (0))
			enemyTargeting = false;
	}

	void AttackEnemy()
	{
		if (!_attack && enemyTargeting && targetInRange () && navMeshAgent.velocity == Vector3.zero)
		{
			if (NoSkillSelected () && !_onCoolDown)
			{
				navMeshAgent.Stop ();
				StartCoroutine (onCoolDown());
				transform.LookAt (new Vector3 (enemyTarget.transform.position.x, this.transform.position.y, enemyTarget.transform.position.z));
				animator.SetInteger ("AttackType", 7);
				animator.SetTrigger ("WeaponAttack");
				if (!Input.GetMouseButton (0))
					enemyTargeting = false;
			}
			if (!NoSkillSelected () && currentSkill.skillType == SkillScript.SkillType.DIRECT_ATTACK && !currentSkill.onCoolDown)
			{
				navMeshAgent.Stop ();
				currentSkill.UseSkill ();
				transform.LookAt (new Vector3 (enemyTarget.transform.position.x, this.transform.position.y, enemyTarget.transform.position.z));
				enemyTargeting = false;
			}
		}
	}

	void RunAnimation ()
	{
		if (Mathf.Abs(navMeshAgent.remainingDistance - navMeshAgent.stoppingDistance) < 0.1f)
		{
			navMeshAgent.Stop();
			navMeshAgent.velocity = Vector3.zero;
		}
		animator.SetBool("Run", navMeshAgent.velocity != Vector3.zero);
	}

	void DeathAnimation ()
	{
		if (!_dead && current_hp <= 0)
		{
			navMeshAgent.Stop ();
			animator.SetTrigger("Death");
			animator.SetInteger ("RandomDeath", Random.Range(0, 4));
			_dead = true;
			StartCoroutine (PrepareToReload());
		}
	}

	public bool isInRange(GameObject target)
	{
		return (enemyTargetsInRange.Find (x => x == target) != null);
	}

	void OnTriggerEnter(Collider col)
	{
		if (col.gameObject.tag == "Enemy") 
			enemyTargetsInRange.Add (col.gameObject.GetComponent<CharacterScript>());
		if (col.gameObject.tag == "Potion") {
			this.ReceiveDamage((int)-(hpMax * 0.3f), false);
			Destroy (col.gameObject);
		}
	}
	
	void OnTriggerExit(Collider col)
	{
		enemyTargetsInRange.Remove (col.gameObject.GetComponent<CharacterScript>());
	}


	void UpdateRange ()
	{
		if (!NoSkillSelected () && CurrentSkillIsDirectAttack())
			sphereCollider.radius = currentSkill.range;
		else
			sphereCollider.radius = weaponRange;
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


	IEnumerator PrepareToReload ()
	{
		yield return new WaitForSeconds (5.0f);
		Application.LoadLevel (Application.loadedLevel);
	}

	public void StopMoving()
	{
		this.navMeshAgent.Stop ();
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