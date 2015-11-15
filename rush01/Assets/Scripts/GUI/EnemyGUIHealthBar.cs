using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EnemyGUIHealthBar : MonoBehaviour {

	void Update () {
		if (PlayerScript.instance._target) {
			GetComponent<Image>().enabled = true;
			GetComponent<Image> ().fillAmount = (PlayerScript.instance._target.GetComponent<Enemy> ().currentHp * 100f /
				PlayerScript.instance._target.GetComponent<Enemy> ().hp) / 100f;
		} else {
			GetComponent<Image>().enabled = false;
		}
	}
}
