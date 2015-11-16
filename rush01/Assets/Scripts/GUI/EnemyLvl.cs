using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EnemyLvl : MonoBehaviour {

	void Update () {
		if (PlayerScript.instance._enemyTarget)
			GetComponent<Text> ().text = PlayerScript.instance._enemyTarget.GetComponent<Enemy>().level.ToString ();
		else
			GetComponent<Text> ().text = "";
	}
}
