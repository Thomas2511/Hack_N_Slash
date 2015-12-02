using UnityEngine;
using System.Collections;

public class PassiveSkillScript : ToggleSkillScript {
	public bool			conBoost;
	public bool			strBoost;
	public bool			agiBoost;
	public bool			damageBoost;
	public bool			intelBoost;
	public bool			hpBoost;
	public bool			mpBoost;

	protected override void ToggleOff ()
	{
		float			hpProportion = 0.0f;
		float			mpProportion = 0.0f;

		base.ToggleOff ();
		PlayerScript player = PlayerScript.instance;
		if (conBoost)
		{
			hpProportion = player.current_hp / (float)player.hpMax;
			player.con = Mathf.Clamp (player.con - damage, 0, int.MaxValue);
			player.current_hp = (int)Mathf.Clamp(player.hpMax * hpProportion, 0, player.hpMax);
		}
		if (strBoost)
			player.str = Mathf.Clamp (player.str - damage, 0, int.MaxValue);
		if (agiBoost)
			player.agi = Mathf.Clamp (player.agi - damage, 0, int.MaxValue);
		if (damageBoost)
			player.bonus_damage = Mathf.Clamp (player.bonus_damage - damage, 0, int.MaxValue);
		if (intelBoost)
		{
			mpProportion = player.current_mana / (float)player.manaMax;
			player.intel = Mathf.Clamp (player.intel - damage, 0, int.MaxValue);
			player.current_mana = (int)Mathf.Clamp (player.manaMax * mpProportion, 0, int.MaxValue);
		}
		if (hpBoost)
		{
			player.bonus_hp = Mathf.Clamp (player.bonus_hp - damage, 0, int.MaxValue);
			player.current_hp = Mathf.Clamp (player.current_hp, 0, player.hpMax);
		}
		if (mpBoost)
		{
			player.bonus_mana = Mathf.Clamp (player.bonus_mana - damage, 0, player.manaMax);
			player.current_mana = Mathf.Clamp (player.current_mana - damage, 0, player.manaMax);
		}
	}

	protected override void ToggleOn ()
	{
		float			hpProportion = 0.0f;
		float			mpProportion = 0.0f;

		base.ToggleOn ();
		PlayerScript player = PlayerScript.instance;
		if (conBoost)
		{
			hpProportion = player.current_hp / (float)player.hpMax;
			player.con = Mathf.Clamp (player.con + damage, 0, int.MaxValue);
			player.current_hp = (int)Mathf.Clamp(player.hpMax * hpProportion, 0, player.hpMax);
		}
		if (strBoost)
			player.str = Mathf.Clamp (player.str + damage, 0, int.MaxValue);
		if (agiBoost)
			player.agi = Mathf.Clamp (player.agi + damage, 0, int.MaxValue);
		if (damageBoost)
			player.bonus_damage = Mathf.Clamp (player.bonus_damage + damage, 0, int.MaxValue);
		if (intelBoost)
		{
			mpProportion = player.current_mana / (float)player.manaMax;
			player.intel = Mathf.Clamp (player.intel + damage, 0, int.MaxValue);
			player.current_mana = (int)Mathf.Clamp (player.manaMax * mpProportion, 0, int.MaxValue);
		}
		if (hpBoost)
		{
			player.bonus_hp = Mathf.Clamp (player.bonus_hp + damage, 0, int.MaxValue);
			player.current_hp = Mathf.Clamp(player.current_hp + damage, 0, player.hpMax);
		}
		if (mpBoost)
		{
			player.bonus_mana = Mathf.Clamp (player.bonus_mana + damage, 0, int.MaxValue);
			player.current_mana = Mathf.Clamp(player.current_mana + damage, 0, player.manaMax);
		}
	}
}
