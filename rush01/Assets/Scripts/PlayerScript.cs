using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerScript : MonoBehaviour {
	// Singleton
	public static PlayerScript	instance;

	// Components
	private NavMeshAgent		_navMeshAgent;
	private Animator			_animator;
	private SphereCollider		_sphereCollider;

	// Utility
	public 	bool				_dead;
	public 	bool				_attack;
	public	bool				_targeting;
	public	bool				_onCoolDown;
	public 	GameObject			_target;
	public 	SkillScript			_currentSkill;
	public	List<GameObject>	_targetsInRange;

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
	public	int					mana;

	// Equipment
	public	WeaponScript		weapon;
	public	ArmorScript			armor;

	// Calculated Stats
	public	int					minDamage { get { return str / 2;}}
	public	int					maxDamage { get { return minDamage + weaponDamage;}}
	public	int					hpMax { get { return 5 * con; } }
	public	int					weaponDamage { get { return weapon == null ? 0 : weapon.damage; }}
	public	int					armorValue { get { return armor == null ? 0 : armor.armorValue; }} 
	public	float				weaponCoolDown { get { return weapon == null ? 3.0f : weapon.coolDown; }}
	public	float				weaponRange { get { return weapon == null ? 2f : weapon.range; }}

	// Use this for initialization
	void Start () {
		instance = this;
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
					_attack = false;
					_navMeshAgent.Resume ();
				}
				else if (hit.collider.tag == "Enemy")
				{
					_navMeshAgent.destination = hit.point;
					_navMeshAgent.stoppingDistance = NoSkillSelected () ? weaponRange : _currentSkill.range;
					_target = hit.collider.gameObject;
					_attack = true;
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

	void AttackEnemy()
	{
		if (_attack && targetInRange () && _navMeshAgent.velocity == Vector3.zero)
		{
			if (NoSkillSelected () && !_onCoolDown)
			{
				StartCoroutine (onCoolDown());
				_navMeshAgent.Stop ();
				transform.LookAt (_target.transform);
				_animator.SetTrigger ("WeaponAttack");
				attackSounds[Random.Range (0, attackSounds.Length)].Play ();
				if (!Input.GetMouseButton (0))
					_attack = false;
			}
			if (!NoSkillSelected () && !_currentSkill.onCoolDown)
			{
				_navMeshAgent.Stop ();
				_currentSkill.UseSkill ();
				if (!Input.GetMouseButton(0))
					_attack = false;
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
		_sphereCollider.radius = weaponRange;
	}
	
	// Update is called once per frame
	void Update () {
		AttackEnemy ();
		FollowMouse();
		DeathAnimation();
		UpdateRange();
		RunAnimation();
		RunSound();
	}

	void LateUpdate()
	{
		UpdateRange ();
	}
}
