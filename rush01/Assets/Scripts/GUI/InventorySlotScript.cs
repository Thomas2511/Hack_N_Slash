using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(Image))]
public class InventorySlotScript : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
	public WeaponScript			weapon = null;
	public GameObject			weaponIcon = null;
	public Sprite				emptySprite;
	public GameObject			tooltip;
	public static GameObject	iconBeingDragged = null;
	public static WeaponScript	weaponBeingDragged = null;

	void Start()
	{
		emptySprite = GetComponent<Image> ().sprite;
	}

	public bool addWeapon (WeaponScript weapon)
	{
		this.weapon = weapon;
		weaponIcon = this.weapon.weaponIcon;
		GetComponent<Image> ().sprite = weapon.weaponIcon.GetComponent<Image> ().sprite;
		if (PlayerScript.instance.weapon == null) PlayerScript.instance.attachWeapon (weapon);
		else this.weapon.gameObject.SetActive (false);
		GetComponent <Image> ().color = Color.white;
		return true;
	}
	
	public void OnBeginDrag (PointerEventData eventData)
	{
		if (weapon != null)
		{
			iconBeingDragged = Instantiate(weaponIcon, this.transform.position, Quaternion.identity) as GameObject;
			iconBeingDragged.transform.SetParent (this.transform);
			weaponBeingDragged = weapon;
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
					GetComponent<Image>().sprite = emptySprite;
					GetComponent <Image> ().color = Color.clear;
				}
			}
			Destroy (iconBeingDragged);
			iconBeingDragged = null;
			weaponBeingDragged = null;
		}
	}
	
	public void OnDrop (PointerEventData eventData)
	{
		if (iconBeingDragged != null && iconBeingDragged.transform.parent.gameObject != this.gameObject)
		{
			GameObject parent = iconBeingDragged.transform.parent.gameObject;
			GameObject weaponIcon = this.weaponIcon;
			WeaponScript weapon = this.weapon;
			parent.GetComponent<Image>().sprite = this.GetComponent<Image>().sprite;
			parent.GetComponent<Image>().color = this.GetComponent<Image>().color;
			Destroy (iconBeingDragged);
			this.weapon = weaponBeingDragged;
			this.weaponIcon = weaponBeingDragged.weaponIcon;
			GetComponent<Image> ().sprite = this.weaponIcon.GetComponent<Image> ().sprite;
			GetComponent <Image> ().color = Color.white;
			if (weaponIcon != null)
			{
				parent.GetComponent<Image>().sprite = weaponIcon.GetComponent<Image>().sprite;
			}
			parent.GetComponent<InventorySlotScript>().weapon = weapon;
			parent.GetComponent<InventorySlotScript>().weaponIcon = weaponIcon;
		}
	}
	
	public void OnPointerClick (PointerEventData eventData)
	{
		if (weapon != null && weapon != PlayerScript.instance.weapon && eventData.button == PointerEventData.InputButton.Right)
		{
			weapon.gameObject.SetActive(true);
			PlayerScript.instance.attachWeapon (weapon);
		}
	}

	public void OnPointerEnter (PointerEventData data) {
		if (weapon != null)
			weapon.activeTooltip (this.transform.position + new Vector3(0, -10, 0));
	}

	public void OnPointerExit (PointerEventData data) {
		if (weapon != null)
			weapon.OnMouseExit ();
	}
}
