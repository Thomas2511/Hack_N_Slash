using UnityEngine;
using System.Collections;

public class PassiveAOESpellScript : SkillScript {
	public FollowTargetAOEScript	spell;
	public FollowTargetAOEScript	clone;

	public override bool SelectSkill ()
	{
		if (clone != null)
		{
			clone.Destroy ();
			return false;
		}	
		UseSkill ();
		return true;
	}
	
	public override void ApplyEffect (Vector3 target, Vector3 origin)
	{
		clone = Instantiate (spell);
		clone.transform.position = PlayerScript.instance.transform.position;
		clone.target = PlayerScript.instance.gameObject;
	}
}
