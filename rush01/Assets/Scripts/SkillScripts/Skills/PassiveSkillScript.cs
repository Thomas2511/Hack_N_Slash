using UnityEngine;
using System.Collections;

public class PassiveSkillScript : ToggleSkillScript {

	private void decreaseStat(ref float stat, int boostDamage, DamageType boostDamageType)
	{
		stat = (boostDamageType == DamageType.PERCENT) ?
			(Mathf.Clamp (stat - (stat / (boostDamage / 100.0f)) * (Mathf.Abs (boostDamage) / 100.0f), 0, int.MaxValue)):
			Mathf.Clamp (stat - boostDamage, 0, int.MaxValue);
	}

	private void increaseStat(ref float stat, int boostDamage, DamageType boostDamageType)
	{
		stat = (boostDamageType == DamageType.PERCENT) ?
			(Mathf.Clamp (stat + (stat * (boostDamage / 100.0f)), 0, int.MaxValue)):
			Mathf.Clamp (stat + boostDamage, 0, int.MaxValue);
	}

	protected override void ToggleOff ()
	{
		base.ToggleOff ();
		PlayerScript.instance.buffs.RemoveBuff (psc);
	}

	protected override void ToggleOn ()
	{
		base.ToggleOn ();
		PlayerScript.instance.buffs.AddBuff(psc);
	}
	
	public override void SpendSkillPoint()
	{
		bool		retoggle = false;

		if (toggled)
		{
			ToggleOff ();
			retoggle = true;
		}
		base.SpendSkillPoint ();
		if (retoggle)
			ToggleOn ();
	}
}
