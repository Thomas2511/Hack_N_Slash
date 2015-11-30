using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class WeaponScript : MonoBehaviour
{
	public enum WeaponRarity
	{
		COMMON,
		UNCOMMON,
		RARE,
		MYTHIC,
		LEGENDARY
	}
	public float		range;
	public float		attackSpeed;
	public int			damage;

	public float		coolDown;

	public bool			equipped;
	public GameObject 	weaponIcon;
	public WeaponRarity rarity;
}

