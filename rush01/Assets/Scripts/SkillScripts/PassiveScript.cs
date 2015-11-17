using UnityEngine;
using System.Collections;

public class PassiveScript : SkillScript {
	public bool			conBoost;
	public bool			strBoost;
	public bool			agiBoost;
	public bool			damageBoost;
	public bool			hpBoost;
	public bool			mpBoost;
	public bool			toggled;

	public override bool SelectSkill ()
	{
		ApplyEffect (Vector3.zero, null);
		return false;
	}

	IEnumerator UseMana ()
	{
		while (true)
		{
			PlayerScript.instance.current_mana = Mathf.Clamp(PlayerScript.instance.current_mana - manaCost, 0, PlayerScript.instance.manaMax);
			if (PlayerScript.instance.current_mana == 0)
			{
				if (toggled)
					ToggleOff();
				break ;
			}
			yield return new WaitForSeconds(1.0f);
		}
	}

	void ToggleOff ()
	{
		StopAllCoroutines ();
		PlayerScript player = PlayerScript.instance;
		if (conBoost)
			player.base_con -= damage;
		if (strBoost)
			player.base_str -= damage;
		if (agiBoost)
			player.base_agi -= damage;
		if (damageBoost)
			player.bonus_damage -= damage;
		if (hpBoost)
		{
			player.bonus_hp -= damage;
			player.current_hp -= damage;
		}
		if (mpBoost)
		{
			player.bonus_mana -= damage;
			player.current_mana -= damage;
		}
		toggled = false;
	}

	void ToggleOn ()
	{
		if (manaOverTime)
			StartCoroutine (UseMana ());
		PlayerScript player = PlayerScript.instance;
		if (conBoost)
			player.base_con += damage;
		if (strBoost)
			player.base_str += damage;
		if (agiBoost)
			player.base_agi += damage;
		if (damageBoost)
			player.bonus_damage += damage;
		if (hpBoost)
		{
			player.bonus_hp += damage;
			player.current_hp += damage;
		}
		if (mpBoost)
		{
			player.bonus_mana += damage;
			player.current_mana += damage;
		}
		toggled = true;
	}

	public override void ApplyEffect (Vector3 target, GameObject origin)
	{
		if (toggled)
			ToggleOff();
		else
			ToggleOn ();
	}
}
