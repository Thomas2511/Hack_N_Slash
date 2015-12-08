using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EnemyName: MonoBehaviour {
	
	void Update () {
		GetComponent<Text> ().text = (PlayerScript.instance.enemyTarget != null) ? PlayerScript.instance.enemyTarget.GetComponent<Enemy>().enemyName : "";
	}
}
