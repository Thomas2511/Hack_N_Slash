using UnityEngine;
using System.Collections;

public class DirectAttackScript : SkillScript
{
	public ProjectileScript			projectile;

	public override bool SelectSkill ()
	{
		return (PlayerScript.instance.currentSkill != this);
	}

	public override void ApplyEffect (Vector3 target, Vector3 origin)
	{
		Instantiate (projectile, origin, Quaternion.LookRotation(target - origin));
	}
}
