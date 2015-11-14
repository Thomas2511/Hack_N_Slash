using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {

	public delegate void  EnemyEvent();
	public event EnemyEvent OnDeath;
	public int str;
	public int agi;
	public int con;
	public int armor;
	public int level;
	public int expGiven;
	public int moneyGiven;
	public int hp { get { return con * 5; } }
	public int minDamage { get { return Mathf.RoundToInt (str / 2); }}
	public int maxDamage { get { return minDamage + str; }}
	public enum EnemyType {
		NONE
	}
	public EnemyType type;
	public Animator animator;
	public NavMeshAgent agent;
	public AudioSource aS;
	public AudioClip death;
	public Curves.StatCurve[] statCurves;
	public AggroRange aggroRange;

	private int _framesToWait = 180;

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
		if (hp <= 0) {
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
		for (int i = 0; i < _framesToWait; i++) {
			yield return new WaitForEndOfFrame ();
			this.transform.position = new Vector3 (this.transform.position.x, this.transform.position.y - 0.0125f, this.transform.position.z);
		}
		Destroy (gameObject);
	}

	void CheckAggroRange () {
		if (aggroRange.intruder) {
			agent.SetDestination (aggroRange.intruder.transform.position);
			agent.stoppingDistance = 2.0f;
			animator.SetBool ("Run", true);
		}
	}

	void Attack () {
		if (aggroRange.intruder) {
			float distanceToTarget;
			bool closeEnough = false;

			distanceToTarget = Vector3.Distance (this.transform.position, aggroRange.intruder.transform.position);
			closeEnough = distanceToTarget > agent.stoppingDistance ? false : true;
			if (closeEnough) {
				this.transform.LookAt (aggroRange.intruder.transform.position);
				animator.SetBool ("Run", false);
				animator.SetTrigger ("Attack");
				Damage ();
			}
		}
	}

	void Damage () {

	}

	void Start () {
		InstantiateStats ();
	}

	void Update () {
		if (hp > 0) {
			CheckHealth ();
			CheckAggroRange ();
			Attack ();
		}
	}
}
