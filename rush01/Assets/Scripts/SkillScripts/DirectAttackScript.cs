using UnityEngine;
using System.Collections;

public class DirectAttackScript : SkillScript
{
	public ProjectileScript			projectile;
	// Use this for initialization
	void Start () {


	}

	public override void SelectSkill ()
	{
		return ;
	}


	public override void UseSkill ()
	{
		Animator animator = PlayerScript.instance.GetComponent<Animator>();
		StartCoroutine (doCoolDown());
		if (spellAttack)
		{
			animator.SetInteger("AttackType", attackAnimationIndex);
			animator.SetTrigger ("SpellAttack");
		}
		else
		{
			animator.SetInteger ("AttackType", attackAnimationIndex);
			animator.SetTrigger ("WeaponAttack");
		}
	}

	public override void ApplyEffect (Vector3 target, Vector3 origin)
	{
		Instantiate (projectile, origin, Quaternion.LookRotation(target - origin));
	}

	// Update is called once per frame
	void Update () {
	
	}
}
