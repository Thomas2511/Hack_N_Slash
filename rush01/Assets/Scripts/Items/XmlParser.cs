using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;

public static class XmlParser {

	public static List<PrefabPath> parseLootPrefabPaths(string content) {
		List<PrefabPath> prefabPaths = new List<PrefabPath> ();
		XmlDocument xmlDoc = new XmlDocument ();

		xmlDoc.LoadXml (content);
		XmlNodeList nodeList = xmlDoc.DocumentElement.SelectNodes("/items");
		foreach (XmlNode node in nodeList) {
			prefabPaths.Add (new PrefabPath(
				_stringToLootType(node.Name),
				node.SelectSingleNode("id").InnerText,
				node.SelectSingleNode("path").InnerText
				));
		}
		return prefabPaths;
	}

	public static List<LootTable> parseLootTables(string content) {
		List<LootTable> loots = new List<LootTable> ();
		XmlDocument xmlDoc = new XmlDocument ();

		xmlDoc.LoadXml (content);
		XmlNodeList nodeList = xmlDoc.DocumentElement.SelectNodes("/");
		foreach (XmlNode node in nodeList) {
			loots.Add (new EnemyToLootTable(

				));
		}
		return loots;
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
	public LootType type { get; private set; }
	public string id { get; private set; }
	public string path { get; private set; }

	public PrefabPath(LootType type, string id, string path) {
		this.type = type;
		this.id = id;
		this.path = path;
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
