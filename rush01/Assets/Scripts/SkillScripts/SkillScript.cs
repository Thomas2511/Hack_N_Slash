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
	public float			coolDown;
	public bool				onCoolDown;
	public abstract	void	ApplyEffect(GameObject target);
	public abstract void	UseSkill(Animator animator);
}

