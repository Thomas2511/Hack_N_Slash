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
	public	bool				_targeting;
	public	bool				_onCoolDown;
	public 	GameObject			_target;
	public 	SkillScript			_currentSkill;
	public	List<GameObject>	_targetsInRange;
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
		return (_currentSkill == null || (_currentSkill.skillType == SkillScript.SkillType.DIRECT_ATTACK));
	}

	bool NoSkillSelected()
	{
		return (_currentSkill == null);
	}

	void CancelAttackAnimation()
	{
		if (_attack)
		{
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
					_targeting = false;
					_target = null;
					_navMeshAgent.Resume ();
				}
				else if (hit.collider.tag == "Enemy")
				{
					_navMeshAgent.destination = hit.point;
					_navMeshAgent.stoppingDistance = NoSkillSelected () ? weaponRange : _currentSkill.range;
					_target = hit.collider.gameObject;
					CancelAttackAnimation ();
					_targeting = true;
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
		return (_target != null && isInRange(_target));
	}

	void applyDamage()
	{
		if (!NoSkillSelected() && CurrentSkillIsDirectAttack())
			_currentSkill.ApplyEffect (_target, playerRightHand.transform.position);
	}

	void attackOver()
	{
		_attack = false;
	}

	void AttackEnemy()
	{
		if (_targeting && targetInRange () && _navMeshAgent.velocity == Vector3.zero)
		{
			if (NoSkillSelected () && !_onCoolDown)
			{
				_navMeshAgent.Stop ();
				StartCoroutine (onCoolDown());
				transform.LookAt (new Vector3 (_target.transform.position.x, this.transform.position.y, _target.transform.position.z));
				_animator.SetTrigger ("WeaponAttack");
				_attack = true;
				attackSounds[Random.Range (0, attackSounds.Length)].Play ();
				if (!Input.GetMouseButton (0))
					_targeting = false;
			}
			if (!NoSkillSelected () && !_currentSkill.onCoolDown)
			{
				_navMeshAgent.Stop ();
				_currentSkill.UseSkill (_animator);
				_attack = true;
				transform.LookAt (new Vector3 (_target.transform.position.x, this.transform.position.y, _target.transform.position.z));
				if (!Input.GetMouseButton(0))
					_targeting = false;
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
		return (_targetsInRange.Find (x => x == target) != null);
	}

	void OnTriggerEnter(Collider col)
	{
		_targetsInRange.Add (col.gameObject);
	}
	
	void OnTriggerExit(Collider col)
	{
		_targetsInRange.Remove (col.gameObject);
	}


	void UpdateRange ()
	{
		if (!NoSkillSelected () && CurrentSkillIsDirectAttack())
			_sphereCollider.radius = _currentSkill.range;
		else
			_sphereCollider.radius = weaponRange;
	}

	/*void UpdateSpeed()
	{
		_animator.SetFloat ("Speed", _navMeshAgent.velocity.magnitude / 3.5f);
	}*/
	
	// Update is called once per frame
	void Update () {
		AttackEnemy ();
		FollowMouse();
		DeathAnimation();
		UpdateRange();
		//UpdateSpeed ();
		RunAnimation();
		RunSound();
	}

	void LateUpdate()
	{
		UpdateRange ();
	}
}
