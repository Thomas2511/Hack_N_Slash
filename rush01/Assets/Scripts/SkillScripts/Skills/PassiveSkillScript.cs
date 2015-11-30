using UnityEngine;
using System.Collections;

public class PassiveSkillScript : ToggleSkillScript {
	public bool			conBoost;
	public bool			strBoost;
	public bool			agiBoost;
	public bool			damageBoost;
	public bool			hpBoost;
	public bool			mpBoost;

	protected override void ToggleOff ()
	{
		base.ToggleOff ();
		PlayerScript player = PlayerScript.instance;
		if (conBoost)
			player.con = Mathf.Clamp (player.con - damage, 0, int.MaxValue);
		if (strBoost)
			player.str = Mathf.Clamp (player.str - damage, 0, int.MaxValue);
		if (agiBoost)
			player.agi = Mathf.Clamp (player.agi - damage, 0, int.MaxValue);
		if (damageBoost)
			player.bonus_damage = Mathf.Clamp (player.bonus_damage - damage, 0, int.MaxValue);
		if (hpBoost)
		{
			player.bonus_hp = Mathf.Clamp (player.bonus_hp - damage, 0, int.MaxValue);
			player.current_hp = Mathf.Clamp (player.current_hp - damage, 0, player.hpMax);
		}
		if (mpBoost)
		{
			player.bonus_mana = Mathf.Clamp (player.current_mana - damage, 0, int.MaxValue);
			player.current_mana = Mathf.Clamp (player.current_mana - damage, 0, player.manaMax);
		}
	}

	protected override void ToggleOn ()
	{
		base.ToggleOn ();
		PlayerScript player = PlayerScript.instance;
		if (conBoost)
			player.con = Mathf.Clamp (player.con + damage, 0, int.MaxValue);
		if (strBoost)
			player.str = Mathf.Clamp (player.str + damage, 0, int.MaxValue);
		if (agiBoost)
			player.agi = Mathf.Clamp (player.agi + damage, 0, int.MaxValue);
		if (damageBoost)
			player.bonus_damage = Mathf.Clamp (player.bonus_damage + damage, 0, int.MaxValue);
		if (hpBoost)
		{
			player.bonus_hp = Mathf.Clamp (player.bonus_hp + damage, 0, int.MaxValue);
			player.current_hp = Mathf.Clamp (player.current_hp + damage, 0, player.hpMax);
		}
		if (mpBoost)
		{
			player.bonus_mana = Mathf.Clamp (player.current_mana + damage, 0, int.MaxValue);
			player.current_mana = Mathf.Clamp (player.current_mana + damage, 0, player.manaMax);
		}
	}
}
