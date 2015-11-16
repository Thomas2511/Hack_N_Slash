using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EnemyName: MonoBehaviour {
	
	void Update () {
		if (PlayerScript.instance._enemyTarget)
			GetComponent<Text> ().text = PlayerScript.instance._enemyTarget.name;
		else
			GetComponent<Text> ().text = "";
	}
}
