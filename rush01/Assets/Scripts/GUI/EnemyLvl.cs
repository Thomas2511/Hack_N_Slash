using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EnemyLvl : MonoBehaviour {

	void Update () {
		if (PlayerScript.instance.enemyTarget)
			GetComponent<Text> ().text = PlayerScript.instance.enemyTarget.GetComponent<Enemy>().level.ToString ();
		else
			GetComponent<Text> ().text = "";
	}
}
