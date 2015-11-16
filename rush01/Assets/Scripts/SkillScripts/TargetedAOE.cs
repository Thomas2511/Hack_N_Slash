using UnityEngine;
using System.Collections;

public class TargetedAOE : SkillScript {
	public MouseAOESpell			spellAOE;
	public static MouseAOESpell		clone;
	public Vector3					AOEtarget;
	public GameObject				spell;


	void onMouseClick (Vector3 pos)
	{
		AOEtarget = pos;
		clone.onMouseClick -= onMouseClick;
		clone.onCancel -= onCancel;
		clone = null;
		UseSkill ();
	}

	void onCancel(Vector3 pos)
	{
		clone.onMouseClick -= onMouseClick;
		clone.onCancel -= onCancel;
		clone = null;
		PlayerScript.instance.currentSkill = null;
	}

	public override bool SelectSkill ()
	{
		if (onCoolDown || PlayerScript.instance.current_mana < manaCost)
			return false;
		if (clone != null)
		{
			clone.onMouseClick -= onMouseClick;
			clone.onCancel -= onCancel;
			Destroy (clone.gameObject);
			Destroy (clone);
		}
		clone = Instantiate(spellAOE);
		clone.range = range;
		clone.onMouseClick += onMouseClick;
		clone.onCancel += onCancel;
		return true;
	}

	public override void ApplyEffect (Vector3 target, Vector3 playerPos)
	{
		Instantiate (spell, AOEtarget + new Vector3(0, 0.5f, 0), Quaternion.LookRotation(Vector3.up));
	}

	
	// Update is called once per frame
	void Update () {
	
	}
}
