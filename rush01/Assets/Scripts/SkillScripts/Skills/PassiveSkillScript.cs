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
		float			hpProportion = 0.0f;
		float			mpProportion = 0.0f;
		int[]			boostDamage = {0, damage, damage2, damage3, damage4, damage5};
		DamageType[]	boostDamageType = {DamageType.UNTYPED, damageType, damage2Type, damage3Type, damage4Type, damage5Type};

		base.ToggleOff ();
		PlayerScript player = PlayerScript.instance;
		if (boostDamage[conBoostIndex] != 0)
		{
			hpProportion = player.current_hp / (float)player.hpMax;
			decreaseStat (ref player.con, boostDamage[conBoostIndex], boostDamageType[conBoostIndex]);
			player.current_hp = (int)Mathf.Clamp(player.hpMax * hpProportion, 0, player.hpMax);
		}
		if (boostDamage[strBoostIndex] != 0)
			decreaseStat (ref player.str, boostDamage[strBoostIndex], boostDamageType[strBoostIndex]);
		if (boostDamage[agiBoostIndex] != 0)
			decreaseStat (ref player.agi, boostDamage[agiBoostIndex], boostDamageType[agiBoostIndex]);
		if (boostDamage[damageBoostIndex] != 0)
			decreaseStat (ref player.bonus_damage, boostDamage[damageBoostIndex], boostDamageType[damageBoostIndex]);
		if (boostDamage[intelBoostIndex] != 0)
		{
			mpProportion = player.current_mana / (float)player.manaMax;
			decreaseStat (ref player.intel, boostDamage[intelBoostIndex], boostDamageType[intelBoostIndex]);
			player.current_mana = (int)Mathf.Clamp (player.manaMax * mpProportion, 0, int.MaxValue);
		}
		if (boostDamage[hpBoostIndex] != 0)
		{
			player.bonus_hp = (boostDamageType[hpBoostIndex] == DamageType.PERCENT) ?
				Mathf.Clamp (Mathf.RoundToInt (player.bonus_hp - player.hpMax * (boostDamage[hpBoostIndex] / 100.0f)), 0, int.MaxValue) :
				Mathf.Clamp (player.bonus_hp - boostDamage[hpBoostIndex], 0, int.MaxValue);
			player.current_hp = (boostDamageType[hpBoostIndex] == DamageType.PERCENT) ?
				Mathf.Clamp (Mathf.RoundToInt (player.current_hp - player.hpMax * (boostDamage[hpBoostIndex] / 100.0f)), 0, int.MaxValue) :
				Mathf.Clamp (player.current_hp - boostDamage[hpBoostIndex], 0, int.MaxValue);
		}
		if (boostDamage[mpBoostIndex] != 0)
		{	
			player.bonus_mana = (boostDamageType[mpBoostIndex] == DamageType.PERCENT) ?
				Mathf.Clamp (Mathf.RoundToInt (player.bonus_mana - player.manaMax * (boostDamage[mpBoostIndex] / 100.0f)), 0, int.MaxValue) :
				Mathf.Clamp (player.bonus_mana - boostDamage[mpBoostIndex], 0, int.MaxValue);
			player.current_mana = (boostDamageType[mpBoostIndex] == DamageType.PERCENT) ?
				Mathf.Clamp (Mathf.RoundToInt (player.current_mana - player.manaMax * (boostDamage[mpBoostIndex] / 100.0f)), 0, int.MaxValue) :
				Mathf.Clamp (player.current_mana - boostDamage[mpBoostIndex], 0, int.MaxValue);
		}
	}

	protected override void ToggleOn ()
	{
		float			hpProportion = 0.0f;
		float			mpProportion = 0.0f;
		int[]			boostDamage = {0, damage, damage2, damage3, damage4, damage5};
		DamageType[]	boostDamageType = {DamageType.UNTYPED, damageType, damage2Type, damage3Type, damage4Type, damage5Type};
		
		base.ToggleOn();
		PlayerScript player = PlayerScript.instance;
		if (boostDamage[conBoostIndex] != 0)
		{
			hpProportion = player.current_hp / (float)player.hpMax;
			increaseStat (ref player.con, boostDamage[conBoostIndex], boostDamageType[conBoostIndex]);
			player.current_hp = (int)Mathf.Clamp(player.hpMax * hpProportion, 0, player.hpMax);
		}
		if (boostDamage[strBoostIndex] != 0)
			increaseStat (ref player.str, boostDamage[strBoostIndex], boostDamageType[strBoostIndex]);
		if (boostDamage[agiBoostIndex] != 0)
			increaseStat (ref player.agi, boostDamage[agiBoostIndex], boostDamageType[agiBoostIndex]);
		if (boostDamage[damageBoostIndex] != 0)
			increaseStat (ref player.bonus_damage, boostDamage[damageBoostIndex], boostDamageType[damageBoostIndex]);
		if (boostDamage[intelBoostIndex] != 0)
		{
			mpProportion = player.current_mana / (float)player.manaMax;
			increaseStat (ref player.intel, boostDamage[intelBoostIndex], boostDamageType[intelBoostIndex]);
			player.current_mana = (int)Mathf.Clamp (player.manaMax * mpProportion, 0, int.MaxValue);
		}
		if (boostDamage[hpBoostIndex] != 0)
		{
			player.bonus_hp = (boostDamageType[hpBoostIndex] == DamageType.PERCENT) ? Mathf.RoundToInt (player.bonus_hp + player.hpMax * (boostDamage[hpBoostIndex] / 100.0f))
				:player.bonus_hp + boostDamage[hpBoostIndex];
			player.current_hp = (boostDamageType[hpBoostIndex] == DamageType.PERCENT) ?
				Mathf.Clamp (Mathf.RoundToInt (player.current_hp + player.hpMax * (boostDamage[hpBoostIndex] / 100.0f)), 0, int.MaxValue) :
					Mathf.Clamp (player.current_hp + boostDamage[hpBoostIndex], 0, int.MaxValue);
		}
		if (boostDamage[mpBoostIndex] != 0)
		{	
			player.bonus_mana = (boostDamageType[mpBoostIndex] == DamageType.PERCENT) ? Mathf.RoundToInt (player.bonus_mana + player.manaMax * (boostDamage[mpBoostIndex] / 100.0f))
				: player.bonus_mana + boostDamage[mpBoostIndex];
			player.current_mana = (boostDamageType[mpBoostIndex] == DamageType.PERCENT) ? Mathf.Clamp (Mathf.RoundToInt (player.current_mana + player.manaMax * (boostDamage[mpBoostIndex] / 100.0f)), 0, int.MaxValue)
				: Mathf.Clamp (player.current_mana + boostDamage[mpBoostIndex], 0, int.MaxValue);
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
