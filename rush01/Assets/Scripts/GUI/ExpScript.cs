using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ExpScript : MonoBehaviour {

	void Update () {
		GetComponent<Text> ().text = "XP : " + PlayerScript.instance.xp.ToString () + " / " + PlayerScript.instance.GetNextLevelXp ();
	}
}
