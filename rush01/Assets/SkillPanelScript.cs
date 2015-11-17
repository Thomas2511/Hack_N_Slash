using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class SkillPanelScript : MonoBehaviour, IDropHandler {
	public SkillScript			skillScript;
	public DraggingIconScript	draggingIcon;
	public Text					text;
	private string				defaultText;
	// Use this for initialization
	void Start () {
		text = GetComponentInChildren<Text>();
		defaultText = text.text;
	}

	#region IDropHandler implementation

	public void OnDrop (PointerEventData eventData)
	{
		if (eventData.pointerDrag)
		{
			draggingIcon = SkillScript.itemBeingDragged.GetComponent<DraggingIconScript>();
			draggingIcon.transform.SetParent (this.transform);
			draggingIcon.transform.localPosition = Vector3.zero;
			skillScript = draggingIcon.originScript;
			draggingIcon.dragSuccessful = true;
		}
	}

	#endregion

	void UpdateText ()
	{
		text.text = (skillScript != null) ? skillScript.Skillname : defaultText;
	}

	void Update () {
		UpdateText();
	}
}
