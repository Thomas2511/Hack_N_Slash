using UnityEngine;
using System.Collections;

public class LootManager : MonoBehaviour {
	public static LootManager instance;
	public Potion potion;
	public WeaponScript[] weapons;

	void Start () {
		instance = this;
	}

	public void DropLoot (Enemy enemy) {
		int roll = Random.Range(1,101);
		WeaponScript weapon;

		if (roll >= 70) {
			Instantiate (potion, enemy.transform.position, Quaternion.identity);
		}
		if (roll == 100) {
			weapon = Instantiate (weapons[0], enemy.transform.position, Quaternion.Euler (90.0f, 0.0f, 0.0f)) as WeaponScript;
			weapon.damage += Mathf.RoundToInt (PlayerScript.instance.level * ((Random.Range (0, 76) / 100f) + 1f));
		} else if (roll >= 97) {
			weapon = Instantiate (weapons[1], enemy.transform.position, Quaternion.Euler (90.0f, 0.0f, 0.0f)) as WeaponScript;
			weapon.damage += Mathf.RoundToInt (PlayerScript.instance.level * ((Random.Range (0, 51) / 100f) + 1f));
		} else if (roll >= 90) {
			weapon = Instantiate (weapons[Random.Range (2, 4)], enemy.transform.position, Quaternion.Euler (90.0f, 0.0f, 0.0f)) as WeaponScript;
			weapon.damage += Mathf.RoundToInt (PlayerScript.instance.level * ((Random.Range (0, 34) / 100f) + 1f));
		} else if (roll >= 80) {
			weapon = Instantiate (weapons[Random.Range (4, 7)], enemy.transform.position, Quaternion.Euler (90.0f, 0.0f, 0.0f)) as WeaponScript;
			weapon.damage += Mathf.RoundToInt (PlayerScript.instance.level * ((Random.Range (0, 26) / 100f) + 1f));
		} else if (roll >= 66) {
			weapon = Instantiate (weapons[Random.Range (7, 10)], enemy.transform.position, Quaternion.Euler (90.0f, 0.0f, 0.0f)) as WeaponScript;
			weapon.damage += PlayerScript.instance.level;
		}
	}
}
