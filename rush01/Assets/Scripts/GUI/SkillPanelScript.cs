using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class SkillPanelScript : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler {
	public SkillScript			skillScript;
	public DraggingIconScript	draggingIcon;
	public Image				cooldownPanel;
	public Text					text;
	[Range (0, 3)]public int	index;
	private string				_defaultText;
	private Color				_defaultColor;
	// Use this for initialization
	void Start () {
		text = GetComponentInChildren<Text>();
		foreach(Image image in GetComponentsInChildren<Image>())
		{
			if (image.gameObject.name == "CooldownPanel")
				cooldownPanel = image;
		}
		_defaultColor = GetComponent<Image>().color;
		_defaultText = text.text;
	}

	public void OnDrop (PointerEventData eventData)
	{
		if (eventData.pointerDrag && SkillScript.itemBeingDragged != null)
		{
			if (draggingIcon != null)
			{
				Destroy (draggingIcon.gameObject);
				Destroy (draggingIcon);
			}
			draggingIcon = SkillScript.itemBeingDragged.GetComponent<DraggingIconScript>();
			draggingIcon.transform.SetParent (this.transform);
			draggingIcon.transform.localPosition = Vector3.zero;
			skillScript = draggingIcon.originScript;
			PlayerScript.instance.skills[index] = skillScript;
			SkillScript.itemBeingDragged = null;
			draggingIcon.dragSuccessful = true;
		}
	}

	bool ScriptIsToggledPassive()
	{
		return (skillScript != null && ((skillScript.GetComponent<PassiveSkillScript>() != null && skillScript.GetComponent<PassiveSkillScript>().toggled)
		        || (skillScript.GetComponent<PassiveAOEScript>() != null && skillScript.GetComponent<PassiveAOEScript>().toggled)));
	}

	void UpdateText ()
	{
		text.text = (skillScript != null) ? skillScript.Skillname : _defaultText;
		text.color = (skillScript != null && skillScript == PlayerScript.instance.currentSkill) ? Color.red : Color.white;
		GetComponent<Image>().color = (ScriptIsToggledPassive()) ? new Color(1, 1, 1, GetComponent<Image>().color.a) : _defaultColor;
	}

	void UpdateCooldownPanel()
	{
		if (skillScript == null || (skillScript != null && !skillScript.onCoolDown))
		{
			cooldownPanel.color = Color.clear;
			if (draggingIcon != null)
				draggingIcon.GetComponent<Image>().color = Color.white;
		}
		if (skillScript != null && skillScript.onCoolDown)
		{
			cooldownPanel.color = Color.white;
			if (draggingIcon != null)
				draggingIcon.GetComponent<Image>().color = new Color(0, 0, 0, 0.1f);
			cooldownPanel.fillAmount = ((skillScript.coolDown - skillScript.timeSinceCooldown) / skillScript.coolDown);
		}
	}

	public void OnPointerEnter (PointerEventData eventData)
	{
		if (skillScript != null) {
			skillScript.activeTooltip (new Vector3(0, 200, 0), this.transform);
		}
	}
	
	public void OnPointerExit (PointerEventData eventData)
	{
		if (skillScript != null) {
			skillScript.OnPointerExit(eventData);
		}
	}

	void Update () {
		UpdateText();
		UpdateCooldownPanel ();
	}
}
