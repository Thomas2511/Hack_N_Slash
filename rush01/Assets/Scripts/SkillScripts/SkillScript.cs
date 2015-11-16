using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class SkillScript : MonoBehaviour
{
	public enum SkillType
	{
		TARGETED_AOE,
		SELF_AOE,
		PASSIVE_AOE,
		DIRECT_ATTACK,
		PASSIVE,
	}
	public SkillType		skillType;
	public bool				spellAttack;
	public int				attackAnimationIndex;
	[Range(0, 4)]
	public int				level;
	public int				range { get { return skillStats[level].range; }}
	public int				manaCost { get { return skillStats[level].manaCost; }}
	public float			coolDown { get { return skillStats[level].coolDown; }}
	public int				damage { get { return skillStats[level].damage; }}
	public int				AOE { get { return skillStats[level].AOE; }}
	public bool				onCoolDown;
	public string			toolTip;
	public SkillStat[]		skillStats = new SkillStat[5];
	public abstract bool	SelectSkill();
	public abstract	void	ApplyEffect(Vector3 target, Vector3 origin);

	public virtual void		UseSkill()
	{
		PlayerScript.instance.current_mana = Mathf.Clamp(PlayerScript.instance.current_mana - manaCost, 0, PlayerScript.instance.manaMax);
		StartCoroutine (doCoolDown ());
		Animator animator = PlayerScript.instance.GetComponent<Animator>();
		animator.SetInteger("AttackType", attackAnimationIndex);
		if (spellAttack) animator.SetTrigger ("SpellAttack");
		else animator.SetTrigger ("WeaponAttack");
	}

	protected virtual IEnumerator doCoolDown ()
	{
		onCoolDown = true;
		yield return new WaitForSeconds(coolDown);
		onCoolDown = false;
	}

	[System.Serializable]
	public class SkillStat
	{
		public int			range;
		public int			coolDown;
		public int			manaCost;
		public int			damage;
		public int			AOE;
	}

}

