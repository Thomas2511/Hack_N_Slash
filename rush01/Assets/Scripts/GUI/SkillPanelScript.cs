using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class SkillPanelScript : MonoBehaviour, IDropHandler {
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
		_defaultColor = GetComponent<Image>().color;
		_defaultText = text.text;
	}

	public void OnDrop (PointerEventData eventData)
	{
		if (eventData.pointerDrag && SkillScript.itemBeingDragged != null)
		{
			if (draggingIcon != null)
				Destroy (draggingIcon);
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
		return (skillScript != null && ((skillScript.GetComponent<PassiveScript>() != null && skillScript.GetComponent<PassiveScript>().toggled)
		        || (skillScript.GetComponent<PassiveAOESpellScript>() != null && skillScript.GetComponent<PassiveAOESpellScript>().toggled)));
	}

	void UpdateText ()
	{
		text.text = (skillScript != null) ? skillScript.Skillname : _defaultText;
		text.color = (skillScript != null && skillScript == PlayerScript.instance.currentSkill) ? Color.red : Color.black;
		GetComponent<Image>().color = (ScriptIsToggledPassive()) ? Color.blue : _defaultColor;
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
				draggingIcon.GetComponent<Image>().color = new Color(1, 1, 1, 0.1f);
			cooldownPanel.fillAmount = ((skillScript.coolDown - skillScript.timeSinceCooldown) / skillScript.coolDown);
		}
	}

	void Update () {
		UpdateText();
		UpdateCooldownPanel ();
	}
}
