using UnityEngine;
using System.Collections;

public class EnemyAnimator : MonoBehaviour {

	void ApplyDamage () {
		GetComponentInParent<Enemy> ().Damage ();
	}

	void AttackSound () {
		GetComponentInParent<Enemy> ().AttackSound ();
	}
}
