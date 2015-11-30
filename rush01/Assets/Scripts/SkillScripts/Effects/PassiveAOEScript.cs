using UnityEngine;
using System.Collections;

public class PassiveAOEScript : ToggleSkillScript {
	public FollowTargetAOEScript			spell;
	public FollowTargetAOEScript			clone;

	protected override void ToggleOn()
	{
		base.ToggleOn();
		clone = Instantiate (spell);
		clone.transform.position = PlayerScript.instance.transform.position;
		clone.target = PlayerScript.instance.gameObject;
		clone.damage = damage;
		clone.radius = AOE;
	}

	protected override void ToggleOff()
	{
		base.ToggleOff ();
		if (clone != null)
			clone.Destroy ();
	}
}
