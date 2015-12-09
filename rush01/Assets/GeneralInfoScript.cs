using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GeneralInfoScript : MonoBehaviour {
	public Text experienceText;
	public Text hpText;
	public Text mpText;
	public Text moneyText;
	public Text weaponText;
	public Text timePlayedText;
	public Text enemiesKilledText;

	private int time = 0;
	private int enemiesKilled = 0;
	// Use this for initialization
	void Start () {
		StartCoroutine (IncrementTime ());
	}

	IEnumerator IncrementTime()
	{
		while (true) {
			yield return new WaitForSeconds(1);
			time++;
		}
	}
	// Update is called once per frame
	void Update () {
		experienceText.text = "XP : " + PlayerScript.instance.xp + " / " + PlayerScript.instance.GetNextLevelXp ();
		hpText.text = "HP : " + PlayerScript.instance.current_hp + " / " + PlayerScript.instance.hpMax;
		mpText.text = "MP : " + PlayerScript.instance.current_mana + " / " + PlayerScript.instance.manaMax;
		moneyText.text = "Money : " + PlayerScript.instance.money;
		weaponText.text = "Weapon : " + (PlayerScript.instance.weapon ? PlayerScript.instance.weapon.name : "None");
		timePlayedText.text = "Time : " + (time / 3600).ToString() + " : " + ((time % 3600) / 60).ToString ("00") + " : " + (time % 60).ToString ("00");
		enemiesKilledText.text = "Enemies Killed : " + enemiesKilled;
	}
}
