using UnityEngine;
using System.Collections;

public abstract class SkillScript : MonoBehaviour
{
	public enum SkillType
	{
		TARGETED_AOE,
		SELF_AOE,
		DIRECT_ATTACK,
		PASSIVE
	}
	public SkillType		skillType;
	public bool				spellAttack;
	public int				attackAnimationIndex;
	public int				level;
	public int				range;
	public int				manaCost;
	public float			coolDown;
	public bool				onCoolDown;
	public abstract void	SelectSkill();
	public abstract	void	ApplyEffect(Vector3 target, Vector3 origin);
	public abstract void	UseSkill();

	protected virtual IEnumerator doCoolDown ()
	{
		onCoolDown = true;
		yield return new WaitForSeconds(coolDown);
		onCoolDown = false;
	}
}

