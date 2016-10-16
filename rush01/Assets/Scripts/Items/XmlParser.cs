using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;

public static class XmlParser {

	public static List<PrefabPath> parseLootPrefabPaths(string content) {
		List<PrefabPath> prefabPaths = new List<PrefabPath> ();
		XmlDocument xmlDoc = new XmlDocument ();

		xmlDoc.LoadXml (content);
		XmlNodeList nodeList = xmlDoc.DocumentElement.SelectNodes("/items/item");
		foreach (XmlNode node in nodeList) {
			prefabPaths.Add (new PrefabPath(
				node.SelectSingleNode("id").InnerText,
				node.SelectSingleNode("path").InnerText,
				_stringToLootType(node.SelectSingleNode("type").InnerText)
				));
		}
		return prefabPaths;
	}

	public static List<EnemyToLootTable> parseLootTables(string content) {
		List<EnemyToLootTable> enemyToLootTables = new List<EnemyToLootTable> ();
		XmlDocument xmlDoc = new XmlDocument ();

		xmlDoc.LoadXml (content);
		XmlNodeList nodeList = xmlDoc.DocumentElement.SelectNodes("/allTables/lootTable");
		foreach (XmlNode node in nodeList) {
			string enemy = node.SelectSingleNode("enemy").InnerText;
			LootTable lootTable = new LootTable();
			foreach (XmlNode lootNode in node.SelectSingleNode("loots").ChildNodes) {
				lootTable.loots.Add (new Loot(
					lootNode.SelectSingleNode("id").InnerText,
					float.Parse(lootNode.SelectSingleNode("dropRate").InnerText),
					int.Parse (lootNode.SelectSingleNode("amount").InnerText),
					_stringToLootType(lootNode.SelectSingleNode("type").InnerText)
					));
			}
			enemyToLootTables.Add (new EnemyToLootTable(lootTable, enemy));
		}
		return enemyToLootTables;
	}

	static LootType _stringToLootType(string str)
	{
		switch (str) {
		case "sword":
			return LootType.SWORD;
		case "potion":
			return LootType.POTION;
		default:
			return LootType.NONE;
		}
	}

}

public class PrefabPath {
	public string id { get; private set; }
	public string path { get; private set; }
	public LootType type { get; private set; }
	
	public PrefabPath(string id, string path, LootType type) {
		this.id = id;
		this.path = path;
		this.type = type;
	}
}

public class EnemyToLootTable {
	public LootTable loots { get; private set; }
	public string enemy { get; private set; }

	public EnemyToLootTable(LootTable loots, string enemy) {
		this.loots = loots;
		this.enemy = enemy;
	}
}
