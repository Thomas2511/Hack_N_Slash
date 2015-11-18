using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class StrScript : MonoBehaviour {

	void Update () {
		GetComponent<Text> ().text = "STR : " + PlayerScript.instance.str.ToString ();
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
