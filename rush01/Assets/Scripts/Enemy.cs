using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {

	public delegate void  EnemyEvent();
	public event EnemyEvent OnDeath;
	public uint str;
	public uint agi;
	public uint con;
	public uint armor;
	public uint level;
	public uint expGiven;
	public uint moneyGiven;
	public uint hp { get { return con * 5; } }
	public uint currentHp;
	public uint minDamage { get { return (uint) Mathf.RoundToInt (str / 2); }}
	public uint maxDamage { get { return minDamage + str; }}
	public bool dead;
	public GameObject intruder;
	public enum EnemyType {
		NONE
	}
	public EnemyType type;
	public Animator animator;
	public NavMeshAgent agent;
	public AudioSource aS;
	public AudioClip death;
	public Curves.StatCurve[] statCurves;

	private uint _framesToWait = 600;

	void OnTriggerStay (Collider col) {
		if (col.gameObject.tag == "Player")
			intruder = col.gameObject;
	}

	void OnTriggerExit (Collider col) {
		if (col.gameObject.tag == "Player")
			intruder = null;
	}

	void InstantiateStats () {
		foreach (Curves.StatCurve statCurve in statCurves) {
			switch (statCurve.stat) {
				case Curves.Stat.STRENGTH:
					str = Curves.ApplyCurve(statCurve.curve, level, str);
					break;

				case Curves.Stat.AGILITY:
					agi = Curves.ApplyCurve(statCurve.curve, level, agi);
					break;

				case Curves.Stat.CONSTITUTION:
					con = Curves.ApplyCurve(statCurve.curve, level, con);
					break;

				case Curves.Stat.ARMOR:
					armor = Curves.ApplyCurve(statCurve.curve, level, armor);
					break;

				case Curves.Stat.EXPERIENCE:
					expGiven = Curves.ApplyCurve(statCurve.curve, level, expGiven);
					break;

				default:
					break;
			}
		}
	}

	void CheckHealth () {
		if (currentHp <= 0 && !dead) {
			dead = true;
			OnDeath();
			animator.SetTrigger ("Death");
			animator.SetInteger ("RandomDeath", Random.Range(0, 4));
			aS.clip = death;
			aS.Play ();
			StartCoroutine (BodyDissolve());
		}
	}

	IEnumerator BodyDissolve ()
	{
		Destroy (agent);
		Destroy (GetComponent<Rigidbody> ());
		Destroy (GetComponent<BoxCollider> ());
		Destroy (GetComponentInChildren<Canvas> ().gameObject);
		float length = 0.0f;
		foreach (AnimatorClipInfo animinfo in animator.GetCurrentAnimatorClipInfo(0))
		{
			if (animinfo.clip.name.StartsWith ("Standing_React_Death_"))
				length = animinfo.clip.length;
		}
		yield return new WaitForSeconds (length);
		for (int i = 0; i < _framesToWait; i++) {
			yield return new WaitForEndOfFrame ();
			this.transform.position = new Vector3 (this.transform.position.x, this.transform.position.y - 0.00125f, this.transform.position.z);
		}
		Destroy (gameObject);
	}

	void CheckAggroRange () {
		if (intruder != null) {
			agent.SetDestination (intruder.transform.position);
			animator.SetBool ("Run", true);
		} else {
			agent.SetDestination (this.transform.position);
			animator.SetBool ("Run", false);
		}

	}

	void Attack () {
		if (intruder) {
			float distanceToTarget;
			bool closeEnough = false;

			distanceToTarget = Vector3.Distance (this.transform.position, intruder.transform.position);
			closeEnough = distanceToTarget > agent.stoppingDistance ? false : true;
			if (closeEnough) {
				Vector3 targetPosition = new Vector3 (intruder.transform.position.x, this.transform.position.y, intruder.transform.position.z);
				
				this.transform.LookAt (targetPosition);
				animator.SetBool ("Run", false);
				animator.SetTrigger ("Attack");
				Damage ();
			}
		}
	}

	void Damage ()
	{
	}

	public void RecieveDamage (int damage) {
		this.currentHp = (uint)Mathf.Clamp(this.currentHp - damage, 0, this.hp);
	}

	void Start () {
		currentHp = hp;
		dead = false;
		agent.stoppingDistance = 2.5f;
		InstantiateStats ();
	}

	void Update () {
		CheckHealth ();
		if (currentHp > 0) {
			CheckAggroRange ();
			Attack ();
		}
	}
}
