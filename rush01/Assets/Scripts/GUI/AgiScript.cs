using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AgiScript : MonoBehaviour {

	void Update () {
		GetComponent<Text> ().text = "AGI : " + PlayerScript.instance.agi.ToString("0")
			+ ((PlayerScript.instance.buffs.agi != 0) ? " (" + ((PlayerScript.instance.buffs.agi > 0) ? "+" : "") + PlayerScript.instance.buffs.agi + ")" : "");
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
