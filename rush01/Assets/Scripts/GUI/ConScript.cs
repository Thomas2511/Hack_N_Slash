using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ConScript : StatScript {

	void Start()
	{
		stringStat = "CON";
	}
	
	protected override void Update ()
	{
		UpdateStats ();
		base.Update ();
	}
	
	protected override void UpdateStats ()
	{
		baseStat = PlayerScript.instance.con;
		totalStat = PlayerScript.instance.conTotal;
		bonusStat = PlayerScript.instance.buffs.con;
		percentStat = PlayerScript.instance.buffs.pCon;
		
	}

	public void IncreaseCon()
	{
		float		hpProportion = 0.0f;

		if (PlayerScript.instance.statPoints > 0)
		{
			hpProportion = PlayerScript.instance.current_hp / (float)PlayerScript.instance.hpMax;
			PlayerScript.instance.con++;
			PlayerScript.instance.current_hp = (int)Mathf.Clamp (PlayerScript.instance.hpMax * hpProportion, 0, PlayerScript.instance.hpMax);
			PlayerScript.instance.statPoints--;
		}
	}
}
