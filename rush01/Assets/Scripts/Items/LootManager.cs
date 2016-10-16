using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public enum LootType {
	SWORD,
	POTION,
	NONE
}

public class LootManager : MonoBehaviour {
	public static LootManager instance;
	public Dictionary<string, Potion> potionPrefabs;
	public Dictionary<string, WeaponScript> weaponPrefabs;

	void Start () {
		instance = this;
		TextAsset content = Resources.Load("Conf/Items") as TextAsset;
		List<PrefabPath> prefabPaths = XmlParser.parseLootPrefabPaths (content.text);

		foreach (PrefabPath prefabPath in prefabPaths) {

			switch (prefabPath.type) {
				case LootType.SWORD:
					WeaponScript ws = Resources.Load (prefabPath.path) as WeaponScript;
					weaponPrefabs.Add (prefabPath.id, ws);
				break;
				case LootType.POTION:
					Potion potion = Resources.Load (prefabPath.path) as Potion;
					potionPrefabs.Add (prefabPath.id, potion);
				break;
			}
		}
	}

	public void DropLoot (Enemy enemy) {
		int roll = UnityEngine.Random.Range(1,101);
		WeaponScript weapon;

		if (roll >= 70) {
			Instantiate (potion, enemy.transform.position, Quaternion.identity);
		}
		if (roll == 100) {
			weapon = Instantiate (weaponPrefabs[0], enemy.transform.position, Quaternion.Euler (90.0f, 0.0f, 0.0f)) as WeaponScript;
			weapon.damage += Mathf.RoundToInt (PlayerScript.instance.level * ((UnityEngine.Random.Range (0, 76) / 100f) + 1f));
		} else if (roll >= 97) {
			weapon = Instantiate (weaponPrefabs[1], enemy.transform.position, Quaternion.Euler (90.0f, 0.0f, 0.0f)) as WeaponScript;
			weapon.damage += Mathf.RoundToInt (PlayerScript.instance.level * ((UnityEngine.Random.Range (0, 51) / 100f) + 1f));
		} else if (roll >= 90) {
			weapon = Instantiate (weaponPrefabs[UnityEngine.Random.Range (2, 4)], enemy.transform.position, Quaternion.Euler (90.0f, 0.0f, 0.0f)) as WeaponScript;
			weapon.damage += Mathf.RoundToInt (PlayerScript.instance.level * ((UnityEngine.Random.Range (0, 34) / 100f) + 1f));
		} else if (roll >= 80) {
			weapon = Instantiate (weaponPrefabs[UnityEngine.Random.Range (4, 7)], enemy.transform.position, Quaternion.Euler (90.0f, 0.0f, 0.0f)) as WeaponScript;
			weapon.damage += Mathf.RoundToInt (PlayerScript.instance.level * ((UnityEngine.Random.Range (0, 26) / 100f) + 1f));
		} else if (roll >= 66) {
			weapon = Instantiate (weaponPrefabs[UnityEngine.Random.Range (7, 10)], enemy.transform.position, Quaternion.Euler (90.0f, 0.0f, 0.0f)) as WeaponScript;
			weapon.damage += PlayerScript.instance.level;
		}
	}
}
