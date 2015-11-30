using UnityEngine;
using System.Collections;

public abstract class ToggleSkillScript: SkillScript {
	public bool								toggled;
	public override bool SelectSkill ()
	{
		if (onCoolDown || PlayerScript.instance.current_mana < manaCost)
			return false;
		UseSkill ();
		return false;
	}

	protected IEnumerator UseMana ()
	{
		while (true)
		{
			PlayerScript.instance.current_mana = Mathf.Clamp(PlayerScript.instance.current_mana - manaCost, 0, PlayerScript.instance.manaMax);
			if (PlayerScript.instance.current_mana == 0)
			{
				if (toggled)
					ToggleOff();
				break ;
			}
			yield return new WaitForSeconds(1.0f);
		}
	}

	protected virtual void ToggleOff()
	{
		StopCoroutine("UseMana");
		StartCoroutine (doCoolDown());
		toggled = false;
	}

	protected virtual void ToggleOn()
	{
		StartCoroutine ("UseMana", UseMana ());
		toggled = true;
	}

	public override void ApplyEffect (Vector3 target, GameObject origin)
	{
		if (toggled)
			ToggleOff();
		else
			ToggleOn ();
	}
}
