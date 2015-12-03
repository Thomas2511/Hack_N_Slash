using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

[RequireComponent(typeof(Image))]
public abstract class SkillScript : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler
{
	public enum SkillType
	{
		TARGETED_AOE,
		SELF_AOE,
		PASSIVE_AOE,
		DIRECT_ATTACK,
		PASSIVE,
	}
	public enum DamageType
	{
		UNTYPED,
		MAGICAL,
		PHYSICAL,
		PERCENT
	}
	public SkillType			skillType;
	public bool					spellAttack;
	[Range(-1, 4)]
	public int					level;
	public string				Skillname;
	public int					range { get { return skillStats[Mathf.Clamp (level, 0, 4)].range; }}
	public int					manaCost { get { return skillStats[Mathf.Clamp (level, 0, 4)].manaCost; }}
	public float				coolDown { get { return skillStats[Mathf.Clamp (level, 0, 4)].coolDown; }}
	public int					damage { get { return skillStats[Mathf.Clamp (level, 0, 4)].damage; }}
	public DamageType			damageType;
	public int					damage2 { get { return skillStats[Mathf.Clamp (level, 0, 4)].damage2; }}
	public DamageType			damage2Type;
	public int					damage3 { get { return skillStats[Mathf.Clamp (level, 0, 4)].damage3; }}
	public DamageType			damage3Type;
	public int					damage4 { get { return skillStats[Mathf.Clamp (level, 0, 4)].damage4; }}
	public DamageType			damage4Type;
	public int					damage5 { get { return skillStats[Mathf.Clamp (level, 0, 4)].damage5; }}
	public DamageType			damage5Type;
	public int					AOE { get { return skillStats[Mathf.Clamp (level, 0, 4)].AOE; }}
	public int					attackAnimationIndex { get { return skillStats[Mathf.Clamp (level, 0, 4)].attackAnimationIndex; }}
	public float				damageMultiplier { get { return skillStats[Mathf.Clamp (level, 0, 4)].damageMultiplier; }}
	public bool					onCoolDown;
	public int					tooltipTextIndex;
	private string				_tooltipText;
	public bool					animated = true;
	public bool					manaOverTime;
	public int					levelUnlocked;

	// UI
	public GameObject			button;
	public GameObject       	dragging_icon;
	public static GameObject    itemBeingDragged;
	public float				timeOfCooldown;
	public float				timeSinceCooldown { get { return Time.time - timeOfCooldown; }}


	public SkillStat[]			skillStats = new SkillStat[5];

	public abstract bool		SelectSkill();

	protected virtual void	Start()
	{
		button = GetComponentInChildren<Button>().gameObject;
		TextAsset textAsset = Resources.Load("Skilltext") as TextAsset;
		string[] file = Regex.Split(textAsset.text, @"#\[[0-9]+\]#\n");
		_tooltipText = (file.Length > tooltipTextIndex) ? file[tooltipTextIndex] : "<i>This text is a placeholder.</i>";
	}

	public void OnBeginDrag (PointerEventData eventData)
	{
		if (this.level >= 0)
		{
			itemBeingDragged = GameObject.Instantiate(dragging_icon);
			itemBeingDragged.GetComponent<DraggingIconScript>().originScript = this;
			itemBeingDragged.transform.SetParent (this.transform);
			itemBeingDragged.GetComponent<Image>().sprite = GetComponent<Image>().sprite;
			itemBeingDragged.transform.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
		}
	}

	public void OnDrag (PointerEventData eventData)
	{
		if (itemBeingDragged != null)
			itemBeingDragged.transform.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
	}

	public void OnEndDrag (PointerEventData eventData)
	{
		if (itemBeingDragged != null && !itemBeingDragged.GetComponent<DraggingIconScript>().dragSuccessful)
		{
			Destroy (itemBeingDragged);
		}
	}
	
	public virtual void		UseSkill()
	{
		if (animated)
		{
			Animator animator = PlayerScript.instance.animator;
			animator.SetInteger("AttackType", attackAnimationIndex);
			if (spellAttack)
			{
				PlayerScript.instance.StopMoving ();
				if (animator.GetBool ("HasWeapon"))
					animator.SetTrigger ("Equip");
				animator.SetTrigger ("SpellAttack");
			}
			else
			{
				PlayerScript.instance.StopMoving ();
				if (!animator.GetBool ("HasWeapon"))
					animator.SetTrigger ("Equip");
				animator.SetTrigger ("WeaponAttack");
			}
		}
		else
			ApplyEffect (Vector3.zero, null);
	}
	public virtual void ApplyEffect(Vector3 target, GameObject origin)
	{
		SpendMana ();
		StartCoroutine (doCoolDown ());
	}

	protected virtual void SpendMana()
	{
		PlayerScript.instance.current_mana = Mathf.Clamp(PlayerScript.instance.current_mana - manaCost, 0, PlayerScript.instance.manaMax);
	}

	protected virtual IEnumerator doCoolDown ()
	{
		onCoolDown = true;
		timeOfCooldown = Time.time;
		yield return new WaitForSeconds(coolDown);
		onCoolDown = false;
	}

	protected string damageText(int damageValue, DamageType damageTypeValue)
	{
		return "<b><color=" + ((damageValue >= 0) ? "red" : "lime") + ">" + Mathf.Abs (damageValue).ToString() + ((damageTypeValue == DamageType.PERCENT) ? " %" : "") + "</color></b>";
	}

	protected string replaceVariables(string tooltipText)
	{
		string newText = tooltipText;
		newText = newText.Replace ("<<level>>", (level + 1 > 0) ? "(Level " + (level + 1).ToString () + ")" : "");
		newText = newText.Replace ("<<damage>>", damageText (damage, damageType));
		newText = newText.Replace ("<<damage2>>", damageText (damage2, damage2Type));
		newText = newText.Replace ("<<damage3>>", damageText (damage3, damage3Type));
		newText = newText.Replace ("<<damage4>>", damageText (damage4, damage4Type));
		newText = newText.Replace ("<<damage5>>", damageText (damage5, damage5Type));
		newText = newText.Replace ("<<damage_modifier>>", "<b><color=orange>(" + ((int)(PlayerScript.instance.minDamage * damageMultiplier)).ToString () + " - " + ((int)(PlayerScript.instance.maxDamage * damageMultiplier)).ToString () + ")</color></b>");
		newText = newText.Replace ("<<cooldown>>", coolDown > 0.0f ? coolDown.ToString ("0.0") + " sec Cooldown" : "No Cooldown");
		newText = newText.Replace ("<<mana_cost>>", (manaCost > 0) ? manaCost.ToString () + " Mana" + (manaOverTime ? " Per Second" : "") : "No Mana Cost");
		return newText;
	}

	protected void activeTooltip()
	{
		GameObject tooltip = GameObject.FindGameObjectWithTag("Tooltip");
		tooltip.GetComponent<CanvasGroup>().alpha = 1;
		Text text = tooltip.GetComponentInChildren<Text>();
		text.text = replaceVariables(_tooltipText);
		tooltip.transform.SetParent (this.transform);
		tooltip.transform.localPosition = new Vector3(0, -60, 0);
	}

	protected virtual void Update()
	{
		Image image = GetComponent<Image>();
		button.SetActive (PlayerScript.instance.skillPoints > 0 && levelUnlocked <= PlayerScript.instance.level && level <= 3);
		image.color = (levelUnlocked > PlayerScript.instance.level || level < 0)
			? new Color(image.color.r, image.color.g, image.color.b, 0.5f) : new Color(image.color.r, image.color.g, image.color.b, 1f);
	}

	public virtual void SpendSkillPoint()
	{
		if (level <= 3 && PlayerScript.instance.skillPoints > 0)
		{
			PlayerScript.instance.skillPoints--;
			level++;
		}
	}

	public void OnPointerEnter (PointerEventData data) {
		Invoke ("activeTooltip", 1);
	}
	
	public void OnPointerExit (PointerEventData data) {
		GameObject tooltip = GameObject.FindGameObjectWithTag("Tooltip");
		CancelInvoke ("activeTooltip");
		tooltip.GetComponent<CanvasGroup>().alpha = 0;
		tooltip.GetComponentInChildren<Text>().text = "";
		tooltip.transform.SetParent (null);
	}

	[System.Serializable]
	public class SkillStat
	{
		public int			range;
		public int			coolDown;
		public int			manaCost;
		public int			damage;
		public int			damage2;
		public int			damage3;
		public int			damage4;
		public int			damage5;
		public int			AOE;
		public int			attackAnimationIndex;
		public float		damageMultiplier = 1.0f;
		public float		duration;
	}
}

