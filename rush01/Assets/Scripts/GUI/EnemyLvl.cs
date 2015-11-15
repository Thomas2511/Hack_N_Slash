using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EnemyLvl : MonoBehaviour {

	void Update () {
		if (PlayerScript.instance._target)
			GetComponent<Text> ().text = PlayerScript.instance._target.GetComponent<Enemy> ().level.ToString ();
		else
			GetComponent<Text> ().text = "";
	}
}
