using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AgiScript : MonoBehaviour {

	void Update () {
		GetComponent<Text> ().text = PlayerScript.instance.agi.ToString();
	}
}
