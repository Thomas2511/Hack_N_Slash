using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class StrScript : StatScript {

	void Start()
	{
		stringStat = "STR";
	}
	
	protected override void Update ()
	{
		UpdateStats ();
		base.Update ();
	}
	
	protected override void UpdateStats ()
	{
		baseStat = PlayerScript.instance.str;
		totalStat = PlayerScript.instance.strTotal;
		bonusStat = PlayerScript.instance.buffs.str;
		percentStat = PlayerScript.instance.buffs.pStr;
		
	}
	public void IncreaseStr()
	{
		if (PlayerScript.instance.statPoints > 0)
		{
			PlayerScript.instance.str++;
			PlayerScript.instance.statPoints--;
		}
	}
}
