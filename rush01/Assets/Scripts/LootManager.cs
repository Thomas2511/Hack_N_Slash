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


		if (roll >= 70) {
			Instantiate (potion, enemy.transform.position, Quaternion.identity);
		}
		if (roll == 100) {
			Instantiate (weapons[0], enemy.transform.position, Quaternion.Euler (90.0f, 0.0f, 0.0f));
		} else if (roll >= 97) {
			Instantiate (weapons[1], enemy.transform.position, Quaternion.Euler (90.0f, 0.0f, 0.0f));
		} else if (roll >= 90) {
			Instantiate (weapons[Random.Range (2, 4)], enemy.transform.position, Quaternion.Euler (90.0f, 0.0f, 0.0f));
		} else if (roll >= 80) {
			Instantiate (weapons[Random.Range (4, 7)], enemy.transform.position, Quaternion.Euler (90.0f, 0.0f, 0.0f));
		} else if (roll >= 66) {
			Instantiate (weapons[Random.Range (7, 10)], enemy.transform.position, Quaternion.Euler (90.0f, 0.0f, 0.0f));
		}
	}
}
