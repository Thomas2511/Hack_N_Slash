public class Loot {
	public string id { get; private set; }
	public float dropRate { get; private set; }
	public int amount { get; private set; }
	public LootType type { get; private set; }

	public Loot(string id, float dropRate, int amount, LootType type) {
		this.id = id;
		this.dropRate = dropRate;
		this.amount = amount;
		this.type = type;
	}
}
