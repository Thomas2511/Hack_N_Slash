using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EnemyGUIHealthBar : MonoBehaviour {

	void Update () {
		if (PlayerScript.instance._enemyTarget) {
			GetComponent<Image>().enabled = true;
			GetComponent<Image> ().fillAmount = (PlayerScript.instance._enemyTarget.GetComponent<Enemy>().currentHp * 100f /
				PlayerScript.instance._enemyTarget.GetComponent<Enemy> ().hp) / 100f;
		} else {
			GetComponent<Image>().enabled = false;
		}
	}
}
