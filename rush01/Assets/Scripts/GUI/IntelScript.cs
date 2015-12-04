using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class IntelScript : StatScript {

	void Start()
	{
		stringStat = "INT";
	}
	
	protected override void Update ()
	{
		UpdateStats ();
		base.Update ();
	}
	
	protected override void UpdateStats ()
	{
		baseStat = PlayerScript.instance.intel;
		totalStat = PlayerScript.instance.intelTotal;
		bonusStat = PlayerScript.instance.buffs.intel;
		percentStat = PlayerScript.instance.buffs.pIntel;
	}

	public void IncreaseIntel()
	{
		float	manaProportion = 0.0f;

		if (PlayerScript.instance.statPoints > 0)
		{
			manaProportion = PlayerScript.instance.current_mana / (float)PlayerScript.instance.manaMax;
			PlayerScript.instance.intel++;
			PlayerScript.instance.current_mana = (int)Mathf.Clamp(PlayerScript.instance.manaMax * manaProportion, 0, PlayerScript.instance.manaMax);
			PlayerScript.instance.statPoints--;
		}
	}
}
