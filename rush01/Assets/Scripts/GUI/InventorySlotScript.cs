using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;

public class InventorySlotScript : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler, IPointerClickHandler
{
	public WeaponScript			weapon = null;
	public GameObject			weaponIcon = null;
	public static GameObject	iconBeingDragged = null;
	public static WeaponScript	weaponBeingDragged = null;
	private Vector3				startPosition;

	public bool addWeapon (WeaponScript weapon)
	{

		this.weapon = weapon;
		this.weaponIcon = Instantiate(this.weapon.weaponIcon);
		weaponIcon.transform.SetParent (this.transform);
		weaponIcon.transform.localPosition = Vector3.zero;
		if (PlayerScript.instance.weapon == null) PlayerScript.instance.attachWeapon (weapon);
		else this.weapon.gameObject.SetActive (false);
		return true;
	}
	
	public void OnBeginDrag (PointerEventData eventData)
	{
		if (weaponIcon != null)
		{
			iconBeingDragged = weaponIcon;
			weaponBeingDragged = weapon;
			startPosition = iconBeingDragged.transform.position;
		}
	}
	
	public void OnDrag (PointerEventData eventData)
	{
		if (iconBeingDragged != null)
			iconBeingDragged.transform.position = Input.mousePosition;
	}

	public void OnEndDrag (PointerEventData eventData)
	{
		if (iconBeingDragged != null)
		{
			RaycastHit hit;
			if (Physics.Raycast (Camera.main.ScreenPointToRay(Input.mousePosition), out hit) && !(PlayerScript.instance.weapon == weapon && weapon.equipped))
			{
				if (hit.collider.tag == "Ground")
				{
					if (PlayerScript.instance.weapon == weapon)
					{
						weapon.transform.SetParent (null);
						PlayerScript.instance.weapon = null;
					}
					if (weapon != null)
					{
						weapon.gameObject.SetActive (true);
						weapon.equipped = false;
						weapon.GetComponent<Rigidbody>().isKinematic = false;
						weapon.GetComponent<Rigidbody>().position = hit.point;
						weapon.GetComponent<Rigidbody>().velocity = Vector3.zero;
						weapon = null;
					}
					Destroy (weaponIcon);
					return ;
				}
			}
			if (iconBeingDragged.transform.parent == this.transform)
				iconBeingDragged.transform.position = startPosition;
			iconBeingDragged = null;
		}
	}
	
	public void OnDrop (PointerEventData eventData)
	{
		if (iconBeingDragged != null && iconBeingDragged.transform.parent.gameObject != this.gameObject)
		{
			GameObject parent = iconBeingDragged.transform.parent.gameObject;
			GameObject weaponIcon = this.weaponIcon;
			WeaponScript weapon = this.weapon;
			iconBeingDragged.transform.SetParent (this.transform);
			iconBeingDragged.transform.localPosition = Vector3.zero;
			this.weapon = weaponBeingDragged;
			this.weaponIcon = iconBeingDragged;
			if (weaponIcon != null)
			{
				weaponIcon.transform.SetParent (parent.transform);
				weaponIcon.transform.localPosition = Vector3.zero;
			}
			parent.GetComponent<InventorySlotScript>().weapon = weapon;
			parent.GetComponent<InventorySlotScript>().weaponIcon = weaponIcon;
		}
	}
	
	public void OnPointerClick (PointerEventData eventData)
	{
		if (weapon != null && eventData.button == PointerEventData.InputButton.Right)
		{
			weapon.gameObject.SetActive(true);
			PlayerScript.instance.attachWeapon (weapon);
		}
	}
}
