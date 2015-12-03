using UnityEngine;
using System.Collections;

public class PassiveSkillScript : ToggleSkillScript {
	[Range(0, 5)]
	public int			conBoostIndex = 0;
	[Range(0, 5)]
	public int			strBoostIndex = 0;
	[Range(0, 5)]
	public int			agiBoostIndex = 0;
	[Range(0, 5)]
	public int			damageBoostIndex = 0;
	[Range(0, 5)]
	public int			intelBoostIndex = 0;
	[Range(0, 5)]
	public int			hpBoostIndex = 0;
	[Range(0, 5)]
	public int			mpBoostIndex = 0;

	private void decreaseStat(ref int stat, int boostDamage, DamageType boostDamageType)
	{
		stat = (boostDamageType == DamageType.PERCENT) ?
			Mathf.Clamp (Mathf.RoundToInt (stat - stat * (boostDamage / 100.0f)), 0, int.MaxValue) :
			Mathf.Clamp (stat - boostDamage, 0, int.MaxValue);
	}

	private void increaseStat(ref int stat, int boostDamage, DamageType boostDamageType)
	{
		stat = (boostDamageType == DamageType.PERCENT) ?
			Mathf.Clamp (Mathf.RoundToInt (stat - stat * (boostDamage / 100.0f)), 0, int.MaxValue) :
				Mathf.Clamp (stat - boostDamage, 0, int.MaxValue);
	}

	protected override void ToggleOff ()
	{
		float			hpProportion = 0.0f;
		float			mpProportion = 0.0f;
		int[]			boostDamage = {0, damage, damage2, damage3, damage4, damage5};
		DamageType[]	boostDamageType = {DamageType.UNTYPED, damageType, damage2Type, damage3Type, damage4Type, damage5Type};

		base.ToggleOff ();
		PlayerScript player = PlayerScript.instance;
		if (boostDamage[conBoostIndex] != 0)
		{
			hpProportion = player.current_hp / (float)player.hpMax;
			increaseStat (ref player.con, boostDamage[conBoostIndex], boostDamageType[conBoostIndex]);
			player.current_hp = (int)Mathf.Clamp(player.hpMax * hpProportion, 0, player.hpMax);
		}
		if (boostDamage[strBoostIndex] != 0)
			increaseStat	(ref player.str, boostDamage[strBoostIndex], boostDamageType[strBoostIndex]);
		if (boostDamage[agiBoostIndex] != 0)
			player.agi = Mathf.Clamp (player.agi - boostDamage[agiBoostIndex], 0, int.MaxValue);
		if (boostDamage[damageBoostIndex] != 0)
			player.bonus_damage = Mathf.Clamp (player.bonus_damage - boostDamage[damageBoostIndex], 0, int.MaxValue);
		if (boostDamage[intelBoostIndex] != 0)
		{
			mpProportion = player.current_mana / (float)player.manaMax;
			player.intel = Mathf.Clamp (player.intel - boostDamage[intelBoostIndex], 0, int.MaxValue);
			player.current_mana = (int)Mathf.Clamp (player.manaMax * mpProportion, 0, int.MaxValue);
		}
		if (boostDamage[hpBoostIndex] != 0)
		{
			player.bonus_hp = Mathf.Clamp (player.bonus_hp - boostDamage[hpBoostIndex], 0, int.MaxValue);
			player.current_hp = Mathf.Clamp (player.current_hp - boostDamage[hpBoostIndex], 0, player.hpMax);
		}
		if (boostDamage[mpBoostIndex] != 0)
		{
			player.bonus_mana = Mathf.Clamp (player.bonus_mana - boostDamage[mpBoostIndex], 0, player.manaMax);
			player.current_mana = Mathf.Clamp (player.current_mana - boostDamage[mpBoostIndex], 0, player.manaMax);
		}
	}

	protected override void ToggleOn ()
	{
		float			hpProportion = 0.0f;
		float			mpProportion = 0.0f;
		int[]			boostDamage = {0, damage, damage2, damage3, damage4, damage5};

		base.ToggleOn ();
		PlayerScript player = PlayerScript.instance;
		if (boostDamage[conBoostIndex] != 0)
		{
			hpProportion = player.current_hp / (float)player.hpMax;
			player.con = Mathf.Clamp (player.con + boostDamage[conBoostIndex], 0, int.MaxValue);
			player.current_hp = (int)Mathf.Clamp(player.hpMax * hpProportion, 0, player.hpMax);
		}
		if (boostDamage[strBoostIndex] != 0)
			player.str = Mathf.Clamp (player.str + boostDamage[strBoostIndex], 0, int.MaxValue);
		if (boostDamage[agiBoostIndex] != 0)
			player.agi = Mathf.Clamp (player.agi + boostDamage[agiBoostIndex], 0, int.MaxValue);
		if (boostDamage[damageBoostIndex] != 0)
			player.bonus_damage = Mathf.Clamp (boostDamage[damageBoostIndex] + damage, 0, int.MaxValue);
		if (boostDamage[intelBoostIndex] != 0)
		{
			mpProportion = player.current_mana / (float)player.manaMax;
			player.intel = Mathf.Clamp (player.intel + boostDamage[intelBoostIndex], 0, int.MaxValue);
			player.current_mana = (int)Mathf.Clamp (player.manaMax * mpProportion, 0, int.MaxValue);
		}
		if (boostDamage[hpBoostIndex] != 0)
		{
			player.bonus_hp = Mathf.Clamp (player.bonus_hp + boostDamage[hpBoostIndex], 0, int.MaxValue);
			player.current_hp = Mathf.Clamp(player.current_hp + boostDamage[hpBoostIndex], 0, player.hpMax);
		}
		if (boostDamage[mpBoostIndex] != 0)
		{
			player.bonus_mana = Mathf.Clamp (player.bonus_mana + boostDamage[mpBoostIndex], 0, int.MaxValue);
			player.current_mana = Mathf.Clamp(player.current_mana + boostDamage[mpBoostIndex], 0, player.manaMax);
		}
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
