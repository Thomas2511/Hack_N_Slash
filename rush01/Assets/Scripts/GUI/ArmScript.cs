using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ArmScript : MonoBehaviour {

	void Update () {
		GetComponent<Text> ().text = PlayerScript.instance.armor.ToString();
	}
}
