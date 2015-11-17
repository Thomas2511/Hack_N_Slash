using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EnemyName: MonoBehaviour {
	
	void Update () {
		GetComponent<Text> ().text = (PlayerScript.instance._enemyTarget != null) ? PlayerScript.instance._enemyTarget.GetComponent<Enemy>().enemyName : "";
	}
}
