using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;

public class InventorySlotScript : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
	public WeaponScript		weapon;
	public Image			weaponIcon;

	public bool addWeapon (WeaponScript weapon)
	{
		this.weapon = weapon;
		this.weaponIcon = Instantiate(this.weapon.weaponIcon).GetComponent<Image>();
		weaponIcon.transform.SetParent (this.transform);
		weaponIcon.transform.localPosition = Vector3.zero;
		this.weapon.gameObject.SetActive (false);
		return true;
	}
	
	public void OnBeginDrag (PointerEventData eventData)
	{
		throw new System.NotImplementedException ();
	}
	
	public void OnDrag (PointerEventData eventData)
	{
		throw new System.NotImplementedException ();
	}

	public void OnEndDrag (PointerEventData eventData)
	{
		throw new System.NotImplementedException ();
	}
	
	public void OnDrop (PointerEventData eventData)
	{
		throw new System.NotImplementedException ();
	}
}
