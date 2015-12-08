using UnityEngine;
using System.Collections;

public class SelfTargetedAOESkillScript : SkillScript {
	public GameObject 	spell;
	public bool			percentDamage = false;

	public override bool SelectSkill ()
	{
		if (onCoolDown || PlayerScript.instance.current_mana < manaCost)
			return false;
		UseSkill ();
		return true;
	}

	public override void ApplyEffect (Vector3 target, GameObject origin)
	{
		base.ApplyEffect(target, origin);
		Instantiate (spell, target + new Vector3(0, 1.5f, 0), Quaternion.identity);
		if (origin.tag == "Player")
		{
			PlayerScript player = origin.GetComponent<PlayerScript>();
			player.ReceiveDamage(percentDamage ? (int)(player.hpMax *  damage / 100.0f) : damage);
		}
	}
}
