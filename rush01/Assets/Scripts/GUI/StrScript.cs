using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class StrScript : MonoBehaviour {

	void Update () {
		GetComponent<Text> ().text = PlayerScript.instance.str.ToString ();
	}
}
