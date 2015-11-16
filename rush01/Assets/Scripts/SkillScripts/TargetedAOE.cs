using UnityEngine;
using System.Collections;

public class TargetedAOE : SkillScript {
	public MouseAOESpell		spellAOE;
	public MouseAOESpell		clone;
	public Vector3				AOEtarget;
	public GameObject			spell;


	void onMouseClick (Vector3 pos)
	{
		AOEtarget = pos;
		clone.onMouseClick -= onMouseClick;
		clone = null;
		UseSkill ();
	}

	public override void SelectSkill ()
	{
		if (clone != null)
		{
			clone.onMouseClick -= onMouseClick;
			Destroy (clone.gameObject);
			Destroy (clone);
		}
		clone = Instantiate(spellAOE);
		clone.range = range;
		clone.onMouseClick += onMouseClick;
	}

	public override void ApplyEffect (Vector3 target, Vector3 playerPos)
	{
		Instantiate (spell, AOEtarget + new Vector3(0, 0.5f, 0), Quaternion.LookRotation(Vector3.up));
	}

	public override void UseSkill ()
	{
		StartCoroutine (doCoolDown ());
		Animator animator = PlayerScript.instance.GetComponent<Animator>();
		animator.SetInteger("AttackType", attackAnimationIndex);
		if (spellAttack) animator.SetTrigger ("SpellAttack");
		else animator.SetTrigger ("WeaponAttack");
	}

	
	// Update is called once per frame
	void Update () {
	
	}
}
