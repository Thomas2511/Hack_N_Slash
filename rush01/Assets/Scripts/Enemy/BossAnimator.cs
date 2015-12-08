using UnityEngine;
using System.Collections;

public class BossAnimator : MonoBehaviour {
	
	void ApplyDamage () {
		GetComponentInParent<BossScript> ().DamageCharacter();
	}
	
	void AttackSound () {
		GetComponentInParent<BossScript> ().AttackSound ();
	}
}