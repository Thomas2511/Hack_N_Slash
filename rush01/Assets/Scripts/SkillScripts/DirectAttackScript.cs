using UnityEngine;
using System.Collections;

public class DirectAttackScript : SkillScript
{
	public ProjectileScript			projectile;

<<<<<<< HEAD

	}

	public override bool SelectSkill ()
	{
		return true;
=======
	public override bool SelectSkill ()
	{
		if ((PlayerScript.instance.weapon == null && !spellAttack) || PlayerScript.instance.current_mana < manaCost)
			return false;
		return (PlayerScript.instance.currentSkill != this);
	}

	public override void ApplyEffect (Vector3 target, GameObject origin)
	{
		if (this.spellAttack)
		{
			Instantiate (projectile, origin.transform.position, Quaternion.LookRotation(target - origin.transform.position));
			projectile.damage = damage;
		}
>>>>>>> new_afaucher
	}
}
