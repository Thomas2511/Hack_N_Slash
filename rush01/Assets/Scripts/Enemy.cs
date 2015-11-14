using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {

	public delegate void  EnemyEvent();
	public event EnemyEvent OnDeath;
	public int str;
	public int agi;
	public int con;
	public int minDamage;
	public int maxDamage;
	public int dodge;
	public int hp;
	public int armor;
	public int level;
	public int expGiven;
	public enum EnemyType {
		NONE
	}
	public EnemyType type;
	public Animator animator;
	public NavMeshAgent agent;

	private int _framesToWait = 180;

	void CheckHealth () {
		if (hp <= 0) {
			hp = 0;
			OnDeath();
			animator.SetTrigger ("Death");
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
	
	}

	void Update () {
	
	}
}
