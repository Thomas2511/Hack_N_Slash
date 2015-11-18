using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerExpBar : MonoBehaviour {

	void Update () {
		GetComponent<Image>().fillAmount = (PlayerScript.instance.xp * 100f / PlayerScript.instance.GetNextLevelXp()) / 100f;
	}
}
