using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ExpScript : MonoBehaviour {

	void Update () {
		GetComponent<Text> ().text = PlayerScript.instance.xp.ToString () + " / TODO NEXT LEVEL EXP";
	}
}
