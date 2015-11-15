using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerLvl : MonoBehaviour {

	void Update () {
		GetComponent<Text> ().text = PlayerScript.instance.level.ToString ();
	}
}
