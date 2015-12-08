using UnityEngine;
using System.Collections;

public class DirectAttackSkillScript : SkillScript
{
	public ProjectileEffectScript		projectile;

	public override bool SelectSkill ()
	{
		if ((PlayerScript.instance.weapon == null && !spellAttack) || PlayerScript.instance.current_mana < manaCost)
			return false;
		return (PlayerScript.instance.currentSkill != this);
	}

	public override void ApplyEffect (Vector3 target, GameObject origin)
	{
		base.ApplyEffect (target, origin);
		if (this.spellAttack)
		{
			Instantiate (projectile, origin.transform.position, Quaternion.LookRotation(target - origin.transform.position));
			projectile.damage = damage;
		}
		else
			PlayerScript.instance.enemyTarget.GetComponent<Enemy>().ReceiveDamage ((int)(PlayerScript.instance.GetDamage () * damageMultiplier));
	}
}
