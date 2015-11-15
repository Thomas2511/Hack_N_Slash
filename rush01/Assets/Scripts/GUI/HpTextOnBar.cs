using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HpTextOnBar : MonoBehaviour {

	void Update () {
		GetComponent<Text> ().text = PlayerScript.instance.current_hp.ToString () + " / " + PlayerScript.instance.hpMax.ToString ();
	}
}
