using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class SkillPanelScript : MonoBehaviour, IDropHandler {
	public SkillScript			skillScript;
	// Use this for initialization
	void Start () {
	
	}

	#region IDropHandler implementation

	public void OnDrop (PointerEventData eventData)
	{
		if (eventData.pointerDrag)
		{
			SkillScript.itemBeingDragged.transform.SetParent (this.transform);
		}
	}

	#endregion
	
	// Update is called once per frame
	void Update () {
	
	}
}
