using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(Rigidbody))]
public class Enemy : CharacterScript {

	public delegate void  EnemyEvent();
	public event EnemyEvent OnDeath;
	public string enemyName;
	public int[] spawnStats;
	public int expGiven;
	
	public enum EnemyType {
		SKELETON,
		NONE
	}
	public Dictionary<EnemyType, int[]> baseStats = new Dictionary<EnemyType, int[]>
	{
		{EnemyType.SKELETON, new int[5] {1, 1, 1, 5, 5}}
	};
	public EnemyType type;
	public AudioSource aS;
	public AudioSource swordS;
	public AudioClip death;
	public AudioClip move;
	public AudioClip attack;
	public Curves.StatCurve[] statCurves;

	private uint _framesToWait = 600;

	protected void OnTriggerEnter (Collider col) {
		if (col.gameObject.tag == "Player")
			enemyTarget = col.gameObject.GetComponent<CharacterScript>();
	}

	protected void OnTriggerExit (Collider col) {
		if (col.gameObject.tag == "Player")
			enemyTarget = null;
	}

	protected void InstantiateStats () {
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
				case Curves.Stat.EXPERIENCE:
					expGiven = Curves.ApplyCurve(statCurve.curve, level, expGiven);
					break;
				case Curves.Stat.MONEY:
					money = Curves.ApplyCurve(statCurve.curve, level, money);
					break;
				default:
					break;
			}
		}
	}

	protected void CheckHealth () {
		if (current_hp <= 0 && !dead) {
			dead = true;
			OnDeath();
			animator.SetTrigger ("Death");
			aS.clip = death;
			aS.Play ();
			PlayerScript.instance.xp += expGiven;
			LootManager.instance.DropLoot(this);
			StartCoroutine (BodyDissolve());
		}
	}

	protected IEnumerator BodyDissolve ()
	{
		navMeshAgent.enabled = false;
		GetComponent<Rigidbody>().isKinematic = true;
		GetComponent<BoxCollider> ().enabled = false;
		Destroy (GetComponentInChildren<Canvas> ().gameObject);
		float length = 0.0f;
		foreach (AnimatorClipInfo animinfo in animator.GetCurrentAnimatorClipInfo(0))
		{
			if (animinfo.clip.name.StartsWith ("Death"))
				length = animinfo.clip.length;
		}
		yield return new WaitForSeconds (length);
		for (int i = 0; i < _framesToWait; i++) {
			yield return new WaitForEndOfFrame ();
			this.transform.position = new Vector3 (this.transform.position.x, this.transform.position.y - 0.00125f, this.transform.position.z);
		}
		Destroy (gameObject);
	}

	protected void CheckAggroRange () {
		if (enemyTarget != null) {
			navMeshAgent.SetDestination (enemyTarget.transform.position);
			if (!aS.isPlaying) {
				aS.clip = move;
				aS.Play ();
			}
			animator.SetBool ("Move", true);
		} else {
			navMeshAgent.SetDestination (this.transform.position);
			aS.Stop ();
			animator.SetBool ("Move", false);
		}

	}

	protected void Attack () {
		if (enemyTarget && !_onCoolDown) {
			if (isInRange (enemyTarget)) {
				Vector3 targetPosition = new Vector3 (enemyTarget.transform.position.x, this.transform.position.y, enemyTarget.transform.position.z);		
				this.transform.LookAt (targetPosition);
				animator.SetBool ("Move", false);
				animator.SetTrigger ("Attack");
				//StartCoolDown();
			}
		}
	}

	public override bool isInRange (CharacterScript target)
	{
		return !(Vector3.Distance (this.transform.position, target.transform.position) > navMeshAgent.stoppingDistance);
	}

	public override void DamageSound ()
	{
		if (!swordS.isPlaying )
			swordS.Play ();
	}

	public override void AttackSound () {
		aS.clip = attack;
		aS.Play ();
	}

	protected override void Start () {
		spawnStats = baseStats [type];
		str = spawnStats [0];
		agi = spawnStats [1];
		con = spawnStats [2];
		expGiven = spawnStats [3];
		money = spawnStats [4];
		InstantiateStats ();
		dead = false;
		base.Start ();
		navMeshAgent.stoppingDistance = 2.0f;
	}

	protected virtual void Update () {
		CheckHealth ();
		if (current_hp > 0) {
			CheckAggroRange ();
			Attack ();
		}
	}
}
