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
	public Curves.StatCurve[]	statCurves;

	private int _framesToWait = 180;

	void InstantiateStats () {
		foreach (Curves.StatCurve statCurve in statCurves) {

		}
	}

	void CheckHealth () {
		if (hp <= 0) {
			OnDeath();
			animator.SetTrigger ("Death");
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

	void Start () {
		InstantiateStats ();
	}

	void Update () {
		if (hp > 0) {

		}
	}
}
