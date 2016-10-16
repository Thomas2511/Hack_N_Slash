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
	public Dictionary<string, GameObject> potionPrefabs;
	public Dictionary<string, GameObject> weaponPrefabs;

	public Dictionary<string, LootTable> lootTables;

	void Start () {
		instance = this;
		potionPrefabs = new Dictionary<string, GameObject> ();
		weaponPrefabs = new Dictionary<string, GameObject> ();
		lootTables = new Dictionary<string, LootTable> ();
		_loadPrefabs();
		_loadLootTables ();
	}

	private void _loadPrefabs() {
		TextAsset content = Resources.Load("Conf/Items") as TextAsset;
		List<PrefabPath> prefabPaths = XmlParser.parseLootPrefabPaths (content.text);
		
		foreach (PrefabPath prefabPath in prefabPaths) {
			
			switch (prefabPath.type) {
			case LootType.SWORD:
				Debug.Log (prefabPath.path);
				GameObject ws = Resources.Load (prefabPath.path) as GameObject;
				weaponPrefabs.Add (prefabPath.id, ws);
				break;
			case LootType.POTION:
				Debug.Log (prefabPath.path);
				GameObject potion = Resources.Load (prefabPath.path) as GameObject;
				potionPrefabs.Add (prefabPath.id, potion);
				break;
			}
		}
	}

	void _loadLootTables ()
	{
		TextAsset content = Resources.Load("Conf/LootTables") as TextAsset;
		List<EnemyToLootTable> enemyToLootTables = XmlParser.parseLootTables(content.text);

		foreach (EnemyToLootTable enemyToLootTable in enemyToLootTables) {
			lootTables.Add (enemyToLootTable.enemy, enemyToLootTable.loots);
		}
	}

	public void DropLoot (Enemy enemy) {
		string enemyName = enemy.enemyName;

		if (lootTables.ContainsKey (enemyName)) {
			LootTable lootTable = lootTables [enemyName];
			foreach (Loot loot in lootTable.loots) {

				for (int i = 0; i < loot.amount; i++) {
					int roll = UnityEngine.Random.Range (1, 1001);
					if (roll <= loot.dropRate) {
						switch (loot.type) {
						case LootType.SWORD:
							if (weaponPrefabs.ContainsKey (loot.id))
								Instantiate (weaponPrefabs [loot.id], enemy.transform.position, Quaternion.Euler (90.0f, 0.0f, 0.0f));
							break;
						case LootType.POTION:
							if (potionPrefabs.ContainsKey (loot.id))
								Instantiate (potionPrefabs [loot.id], enemy.transform.position, Quaternion.identity);
							break;
						}
					}
				}
			}
		}
	}
}
