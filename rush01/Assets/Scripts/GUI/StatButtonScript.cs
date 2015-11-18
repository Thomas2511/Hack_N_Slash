using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class StatButtonScript : MonoBehaviour {
	void Update()
	{
		GetComponent<Button>().enabled = (PlayerScript.instance.statPoints > 0);
		GetComponent<Image>().enabled = (PlayerScript.instance.statPoints > 0);
	}
}
