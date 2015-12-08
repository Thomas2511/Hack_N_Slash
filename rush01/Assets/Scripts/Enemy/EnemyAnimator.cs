using UnityEngine;
using System.Collections;

public class EnemyAnimator : MonoBehaviour {

	void ApplyDamage () {
		GetComponentInParent<Enemy> ().DamageCharacter();
	}

	void AttackSound () {
		GetComponentInParent<Enemy> ().AttackSound ();
	}
}
