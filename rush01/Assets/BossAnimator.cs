using UnityEngine;
using System.Collections;

public class BossAnimator : MonoBehaviour {
	
	void ApplyDamage () {
		GetComponentInParent<BossScript> ().Damage ();
	}
	
	void AttackSound () {
		GetComponentInParent<BossScript> ().AttackSound ();
	}
}