using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ManaTextOnBar : MonoBehaviour {

	void Update () {
		GetComponent<Text> ().text = PlayerScript.instance.current_mana.ToString () + " / " + PlayerScript.instance.manaMax.ToString ();
	}
}
