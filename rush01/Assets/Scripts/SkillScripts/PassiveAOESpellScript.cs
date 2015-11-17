using UnityEngine;
using System.Collections;

public class PassiveAOESpellScript : SkillScript {
	public FollowTargetAOEScript			spell;
	public FollowTargetAOEScript			clone;
	public bool								toggled;

	public override bool SelectSkill ()
	{
		if (toggled)
		{
			ToggleOff ();
			return false;
		}
		if (onCoolDown || PlayerScript.instance.current_mana < manaCost)
			return false;
		UseSkill ();
		return true;
	}

	void ToggleOn()
	{
		StartCoroutine (UseMana());
		clone = Instantiate (spell);
		clone.transform.position = PlayerScript.instance.transform.position;
		clone.target = PlayerScript.instance.gameObject;
		clone.damage = damage;
		clone.radius = AOE;
		toggled = true;
	}

	void ToggleOff()
	{
		StopAllCoroutines ();
		if (clone != null)
			clone.Destroy ();
		toggled = false;
	}

	IEnumerator UseMana ()
	{
		while (true)
		{
			PlayerScript.instance.current_mana = Mathf.Clamp(PlayerScript.instance.current_mana - manaCost, 0, PlayerScript.instance.manaMax);
			if (PlayerScript.instance.current_mana == 0)
			{
				ToggleOff ();
				break ;
			}
			yield return new WaitForSeconds(1.0f);
		}
	}

	public override void ApplyEffect (Vector3 target, GameObject origin)
	{
		ToggleOn ();
	}
}
