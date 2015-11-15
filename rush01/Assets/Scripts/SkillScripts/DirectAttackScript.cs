using UnityEngine;
using System.Collections;

public class DirectAttackScript : SkillScript
{
	public ProjectileScript			projectile;
	// Use this for initialization
	void Start () {


	}

	IEnumerator doCoolDown ()
	{
		onCoolDown = true;
		yield return new WaitForSeconds(coolDown);
		onCoolDown = false;
	}
	
	public override void UseSkill (Animator animator)
	{
		Debug.Log ("test");
		StartCoroutine (doCoolDown());
		if (spellAttack)
		{
			animator.SetInteger("SpellAttackType", attackAnimationIndex);
			animator.SetTrigger ("SpellAttack");
		}
		else
		{
			animator.SetInteger ("WeaponAttackType", attackAnimationIndex);
			animator.SetTrigger ("WeaponAttack");
		}
	}

	public override void ApplyEffect (GameObject target, Vector3 playerPos)
	{
		if (projectile != null && target != null)
			Instantiate (projectile, playerPos, Quaternion.LookRotation(target.transform.position - playerPos));
	}

	// Update is called once per frame
	void Update () {
	
	}
}
