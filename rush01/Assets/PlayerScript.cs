using UnityEngine;
using System.Collections;

public class PlayerScript : MonoBehaviour {
	// Singleton
	public static PlayerScript	instance;

	// Components
	private NavMeshAgent		_navMeshAgent;
	private Animator			_animator;

	// Utility
	private bool				dead;

	// Audio
	public AudioSource			footstepsSound;

	// Stats
	public	int					current_hp;
	public	int					str;
	public	int					agi;
	public	int					con;
	public	int					xp;
	public	int					money;
	public	int					level;

	// Equipment 		

	// Calculated Stats
	public	int					minDamage { get { return str / 2;}}
	public	int					maxDamage { get { return minDamage + str;}}
	public	int					hpMax { get { return 5 * con; } }
	// Use this for initialization
	void Start () {
		instance = this;
		_navMeshAgent = GetComponent<NavMeshAgent>();
		_animator = GetComponent<Animator>();
	}

	void FollowMouse ()
	{
		if (Input.GetMouseButtonDown (0))
		{
			RaycastHit hit;
			if (Physics.Raycast (Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
			{
				if (hit.collider.tag == "Ground")
				{
					_navMeshAgent.destination = hit.point;
					_navMeshAgent.stoppingDistance = 0.0f;
				}
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
		if (!dead && current_hp <= 0)
		{
			_navMeshAgent.Stop ();
			_animator.SetTrigger("Death");
			_animator.SetInteger ("RandomDeath", Random.Range(0, 4));
			dead = true;
		}
	}
	
	// Update is called once per frame
	void Update () {
		FollowMouse();
		DeathAnimation();
		RunAnimation();
		RunSound();
	}
}
