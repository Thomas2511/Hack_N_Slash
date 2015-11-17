using UnityEngine;
using System.Collections;

public class InventoryScript : MonoBehaviour {
	public static InventoryScript		instance;
	public InventorySlotScript[]		slots;

	void Awake()
	{
		instance = this;
		slots = GetComponentsInChildren<InventorySlotScript>();
	}

	public bool addWeapon(WeaponScript weapon)
	{
		foreach (InventorySlotScript slot in slots)
		{
			if (slot.weapon == null)
				return slot.addWeapon(weapon);
		}
		return false;
	}
}
