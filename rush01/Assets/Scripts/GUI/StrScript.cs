using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class StrScript : MonoBehaviour {

	void Update () {
		GetComponent<Text> ().text = "STR : " + PlayerScript.instance.str.ToString ();
	}
}
