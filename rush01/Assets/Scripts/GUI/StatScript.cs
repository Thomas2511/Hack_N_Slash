using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;

public abstract class StatScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
	protected int baseStat;
	protected int totalStat;
	protected int bonusStat;
	protected int percentStat;
	protected string stringStat;

	protected abstract void UpdateStats();

	protected virtual void Update () {
		UpdateText();
	}

	protected void UpdateText()
	{
		GetComponent<Text> ().text = stringStat + " : " + totalStat;
	}

	protected void ActiveTooltip()
	{
		GameObject tooltip = GameObject.FindGameObjectWithTag("Tooltip");
		tooltip.GetComponent<CanvasGroup>().alpha = 1;
		Text text = tooltip.GetComponentInChildren<Text>();
		text.text += "<size=15>Base: " + baseStat.ToString () + "\n";
		text.text += (percentStat != 0) ? "<color=orange>%Bonus:" + (percentStat > 0 ? " + " : " - ") + Mathf.Abs(percentStat).ToString () + "% * </color>" + baseStat.ToString () + "\n" : "";
		text.text += (bonusStat != 0) ? "<color=lime>Bonus:" + (bonusStat > 0 ? " + " : " - ") + Mathf.Abs(bonusStat).ToString () + "</color>\n" : "";
		text.text += " => " + totalStat.ToString () + "</size>";
		tooltip.transform.position = this.transform.position + new Vector3(0, -60, 0);
	}

	public void OnPointerEnter (PointerEventData eventData)
	{
		Invoke ("ActiveTooltip", 1);
	}

	public void OnPointerExit (PointerEventData eventData)
	{
		GameObject tooltip = GameObject.FindGameObjectWithTag("Tooltip");
		CancelInvoke ("ActiveTooltip");
		tooltip.GetComponent<CanvasGroup>().alpha = 0;
		tooltip.GetComponentInChildren<Text>().text = "";
	}
}
