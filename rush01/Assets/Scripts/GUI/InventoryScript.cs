using UnityEngine;
using System.Collections;

public class InventoryScript : MonoBehaviour {
	public static InventoryScript		instance;
	public InventorySlotScript[]		slots;
	public bool							active = true;

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

	void Update()
	{
		if (Input.GetKeyDown ("i"))
		{
			if (active)
			{
				GetComponent<CanvasGroup>().alpha = 0;
				GetComponent<CanvasGroup>().blocksRaycasts = false;
				active = false;
			}
			else
			{
				GetComponent<CanvasGroup>().alpha = 1;
				GetComponent<CanvasGroup>().blocksRaycasts = true;
				active = true;
			}
		}
	}

	public void onClick(int i)
	{
		return ;
	}
}
