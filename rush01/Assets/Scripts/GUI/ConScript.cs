using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ConScript : MonoBehaviour {

	void Update () {
		GetComponent<Text> ().text = "CON : " + PlayerScript.instance.con.ToString ();
	}

	public void IncreaseCon()
	{
		if (PlayerScript.instance.statPoints > 0)
		{
			PlayerScript.instance.con++;
			PlayerScript.instance.statPoints--;
		}
	}
}
