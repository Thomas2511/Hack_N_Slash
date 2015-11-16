using UnityEngine;
using System.Collections;

public class DirectAttackScript : SkillScript
{
	public ProjectileScript			projectile;

	public override bool SelectSkill ()
	{
		if (PlayerScript.instance.weapon == null && !spellAttack)
			return false;
		return (PlayerScript.instance.currentSkill != this);
	}

	public override void ApplyEffect (Vector3 target, Vector3 origin)
	{
		if (this.spellAttack)
			Instantiate (projectile, origin, Quaternion.LookRotation(target - origin));
	}
}
