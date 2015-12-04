using UnityEngine;
using System.Collections;

public class PassiveSkillScript : ToggleSkillScript {
	public BuffScript.PassiveStatChange psc;

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
		if (psc != null)
		{
			PlayerScript.instance.buffs.RemoveBuff (psc);
			psc = null;
		}
	}

	protected override void ToggleOn ()
	{
		PlayerScript player = PlayerScript.instance;
		base.ToggleOn ();
		PassiveStatTemplate pst = this.passiveStatTemplate;
		psc = new BuffScript.PassiveStatChange();
		psc.con = pst.con + (int)((pst.pCon / 100.0f) * player.con);
		psc.agi = pst.agi + (int)((pst.pAgi / 100.0f) * player.agi);
		psc.intel = pst.intel + (int)((pst.pIntel / 100.0f) * player.intel);
		psc.str = pst.str + (int)((pst.pStr / 100.0f) * player.str);
		psc.damage = pst.damage + (int)((pst.pDamage/ 100.0f) * player.maxDamage);
		psc.cooldownReduction += pst.cooldownReduction;
		psc.attackSpeed += pst.attackSpeed;
		psc.hp += pst.hp + (int)((pst.pHp / 100.0f) * player.hpMax);
		psc.mana += pst.mana + (int)((pst.pMp / 100.0f) * player.manaMax);
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
