using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ArmScript : MonoBehaviour {

	void Update () {
		GetComponent<Text> ().text = "Mana : " + PlayerScript.instance.manaMax.ToString ();
	}

	public void IncreaseMana()
	{
		if (PlayerScript.instance.statPoints > 0)
		{
			PlayerScript.instance.base_mana += 5;
			PlayerScript.instance.statPoints--;
		}
	}
}
