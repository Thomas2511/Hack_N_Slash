using UnityEngine;
using System.Collections;

public class EnemyAnimator : MonoBehaviour {
	public bool		dancing;

	void ApplyDamage () {
		GetComponentInParent<Enemy> ().DamageCharacter();
	}

	void AttackSound () {
		GetComponentInParent<Enemy> ().AttackSound ();
	}

	void Start()
	{
		if (dancing)
			GetComponent<Animator> ().SetTrigger ("Dance");
	}
}
