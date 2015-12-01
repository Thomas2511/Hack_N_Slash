using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ConScript : MonoBehaviour {

	void Update () {
		GetComponent<Text> ().text = "CON : " + PlayerScript.instance.con.ToString ();
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
