using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerLvl : MonoBehaviour {

	void Update () {
		GetComponent<Text> ().text = "Lvl " + PlayerScript.instance.level.ToString ();
	}
}
