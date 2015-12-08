using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Text.RegularExpressions;

public class WeaponScript : MonoBehaviour
{
	public enum WeaponRarity
	{
		COMMON,
		UNCOMMON,
		RARE,
		MYTHIC,
		LEGENDARY
	}
	public float		range;
	public float		attackSpeed;
	public int			damage;

	public float		coolDown;

	public bool			equipped;
	public GameObject 	weaponIcon;
	public WeaponRarity rarity;

	public int			tooltipTextIndex;
	public GameObject	tooltip; 
	private string		_tooltipText;


	protected virtual void	Start()
	{
		TextAsset textAsset = Resources.Load("Text/Weapontext") as TextAsset;
		string[] file = Regex.Split(textAsset.text, @"#\[[0-9]+\]#\n");
		_tooltipText = (file.Length > tooltipTextIndex) ? replaceVariables(file[tooltipTextIndex]) : "<i>This text is a placeholder.</i>";
		tooltip = GameObject.FindGameObjectWithTag ("Tooltip");
	}

	protected string replaceVariables(string tooltipText)
	{
		string newText = tooltipText;
		newText = newText.Replace ("<<damage>>", damage.ToString());
		newText = newText.Replace ("<<range>>", range.ToString());
		newText = newText.Replace ("<<attack_speed>>", attackSpeed.ToString());
		return newText;
	}
	public void activeTooltip(Vector3 offset, Transform parent)
	{
		tooltip.transform.position = Camera.main.WorldToScreenPoint (offset + parent.position);
		tooltip.GetComponent<CanvasGroup> ().alpha = 1;
		tooltip.GetComponentInChildren<Text>().text = _tooltipText;
	}

	public void activeTooltip()
	{
		activeTooltip (new Vector3 (0, -1f, 0), this.transform);
	}

	protected virtual IEnumerator waitForTooltip()
	{
		yield return new WaitForSeconds (0.35f);
		activeTooltip ();
	}

	public void OnMouseEnter () {
		StartCoroutine (waitForTooltip());
	}

	public void OnMouseExit () {
		StopAllCoroutines ();
		tooltip.GetComponent<CanvasGroup>().alpha = 0;
		tooltip.GetComponentInChildren<Text>().text = "";
	}
}