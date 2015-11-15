using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DamScript : MonoBehaviour {

	void Update () {
		GetComponent<Text> ().text = PlayerScript.instance.minDamage.ToString () + " - " + PlayerScript.instance.maxDamage.ToString ();
	}
}
