using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;

public class AgiScript : StatScript {

	void Start()
	{
		stringStat = "AGI";
	}

	protected override void Update ()
	{
		UpdateStats ();
		base.Update ();
	}

	protected override void UpdateStats ()
	{
		baseStat = PlayerScript.instance.agi;
		totalStat = PlayerScript.instance.agiTotal;
		bonusStat = PlayerScript.instance.buffs.agi;
		percentStat = PlayerScript.instance.buffs.pAgi;

	}

	public void IncreaseAgi()
	{
		if (PlayerScript.instance.statPoints > 0)
		{
			PlayerScript.instance.agi++;
			PlayerScript.instance.statPoints--;
		}
	}
}
