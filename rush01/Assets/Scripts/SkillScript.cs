using UnityEngine;
using System.Collections;

public abstract class SkillScript
{
	public enum SkillType
	{
		TARGETED_AOE,
		SELF_AOE,
		DIRECT_ATTACK,
		PASSIVE
	}
	public SkillType		skillType;
	public int				level;
	public int				range;
	public float			coolDown;
	public bool				onCoolDown;
	public abstract void	UseSkill();
}

