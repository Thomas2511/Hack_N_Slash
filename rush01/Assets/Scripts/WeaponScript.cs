using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class WeaponScript : MonoBehaviour
{
	public float		range;
	public float		attackSpeed;
	public int			damage;

	public float		coolDown;

	public bool			equipped;
	public GameObject 	weaponIcon;
}

