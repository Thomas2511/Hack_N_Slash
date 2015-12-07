using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class NextLevelButtonScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {

	// Use this for initialization
	void Start () {
	
	}

	public void OnPointerEnter (PointerEventData eventData)
	{
		GetComponentInParent<SkillScript> ().OnPointerExit (eventData);
		GetComponentInParent<SkillScript> ().tooltipEnabled = false;
		GetComponentInParent<SkillScript> ().activeTooltip (true);
	}

	public void OnPointerExit (PointerEventData eventData)
	{
		GetComponentInParent<SkillScript> ().OnPointerExit (eventData);
		GetComponentInParent<SkillScript> ().tooltipEnabled = true;
	}

	public void SpendSkillPoint()
	{
		GetComponentInParent<SkillScript> ().SpendSkillPoint ();
		GetComponentInParent<SkillScript> ().UpdateTooltip (true);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
