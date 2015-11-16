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
	private Animator			_animator;
	private SphereCollider		_sphereCollider;

	// Utility
	public 	bool				_dead;
	public	bool				_attack;
	public	bool				_enemyTargeting;
	public	bool				_onCoolDown;
	public 	GameObject			_enemyTarget;
	public	List<SkillScript>	_skills;
	public 	SkillScript			currentSkill;
	public	List<GameObject>	_enemyTargetsInRange;
	public	AttackType			_attackType;
	public	GameObject			playerRightHand;

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
	public	int					level;
	public	int					current_mana;

	// Equipment
	public	WeaponScript		weapon;
	public	ArmorScript			armor;

	// Calculated Stats
	public	int					minDamage { get { return str / 2;}}
	public	int					maxDamage { get { return minDamage + weaponDamage;}}
	public	int					hpMax { get { return 5 * con; } }
	public	int					manaMax { get { return 100; }}
	public	int					weaponDamage { get { return weapon == null ? 0 : weapon.damage; }}
	public	int					armorValue { get { return armor == null ? 0 : armor.armorValue; }} 
	public	float				weaponCoolDown { get { return weapon == null ? 3.0f : weapon.coolDown; }}
	public	float				weaponRange { get { return weapon == null ? 2f : weapon.range; }}

	// Use this for initialization
	void Start () {
		instance = this;
		_attackType = AttackType.NONE;
		_navMeshAgent = GetComponent<NavMeshAgent>();
		_animator = GetComponent<Animator>();
		_sphereCollider = GetComponentInChildren<SphereCollider>();
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
			_animator.SetTrigger ("Cancel");
			_attack = false;
		}
	}

	void FollowMouse ()
	{
		if (CurrentSkillIsDirectAttack() && Input.GetMouseButtonDown (0))
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
					_navMeshAgent.stoppingDistance = NoSkillSelected () ? weaponRange : currentSkill.range;
					_enemyTarget = hit.collider.gameObject;
					CancelAttackAnimation ();
					_attack = false;
					_enemyTargeting = true;
					_navMeshAgent.Resume();
				}
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

	void applyDamage()
	{
		if (NoSkillSelected())
			return ;
		if (CurrentSkillIsDirectAttack())
			currentSkill.ApplyEffect (_enemyTarget.transform.position, playerRightHand.transform.position);
		else if (currentSkill.skillType == SkillScript.SkillType.SELF_AOE
		    || currentSkill.skillType == SkillScript.SkillType.PASSIVE_AOE
		    || currentSkill.skillType == SkillScript.SkillType.TARGETED_AOE)
			currentSkill.ApplyEffect (this.transform.position, Vector3.zero);
	}

	void attackOver()
	{
		_attack = false;
		currentSkill = null;
	}

	void attackBegin()
	{
		_attack = true;
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
				_animator.SetTrigger ("WeaponAttack");
				attackSounds[Random.Range (0, attackSounds.Length)].Play ();
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
		_animator.SetBool("Run", _navMeshAgent.velocity != Vector3.zero);
	}

	void RunSound()
	{
		if (_navMeshAgent.velocity != Vector3.zero)
		{
			if (!footstepsSound.isPlaying)
				footstepsSound.Play ();
		}
		else
			footstepsSound.Stop ();
	}

	void DeathAnimation ()
	{
		if (!_dead && current_hp <= 0)
		{
			_navMeshAgent.Stop ();
			_animator.SetTrigger("Death");
			_animator.SetInteger ("RandomDeath", Random.Range(0, 4));
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
		if (_attack || _skills[index] == null)
			return ;
		currentSkill = (_skills[index].SelectSkill()) ? _skills[index] : null;
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
	}
	
	// Update is called once per frame
	void Update () {
		ManageSkills();
		UpdateRange();
		AttackEnemy ();
		FollowMouse();
		DeathAnimation();
		//UpdateSpeed ();
		RunAnimation();
		RunSound();
	}

	void LateUpdate()
	{
		UpdateRange ();
	}
}
