using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerManaBar : MonoBehaviour {
	
	void Update () {
		GetComponent<Image>().fillAmount = (PlayerScript.instance.current_mana * 100f / PlayerScript.instance.manaMax) / 100f;
	}
}
