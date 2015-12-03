using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class IntelScript : MonoBehaviour {

	void Update () {
		GetComponent<Text> ().text = "INT : " + PlayerScript.instance.intel.ToString("0");
	}

	public void IncreaseIntel()
	{
		float	manaProportion = 0.0f;

		if (PlayerScript.instance.statPoints > 0)
		{
			manaProportion = PlayerScript.instance.current_mana / (float)PlayerScript.instance.manaMax;
			PlayerScript.instance.intel++;
			PlayerScript.instance.current_mana = (int)Mathf.Clamp(PlayerScript.instance.manaMax * manaProportion, 0, PlayerScript.instance.manaMax);
			PlayerScript.instance.statPoints--;
		}
	}
}
