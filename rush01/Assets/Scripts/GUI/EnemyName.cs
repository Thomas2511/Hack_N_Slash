using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EnemyName: MonoBehaviour {
	
	void Update () {
		if (PlayerScript.instance._target)
			GetComponent<Text> ().text = PlayerScript.instance._target.name;
		else
			GetComponent<Text> ().text = "";
	}
}
