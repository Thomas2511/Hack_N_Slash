using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ConScript : MonoBehaviour {

	void Update () {
		GetComponent<Text> ().text = PlayerScript.instance.con.ToString ();
	}
}
