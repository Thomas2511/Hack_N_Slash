using UnityEngine;
using System.Collections;

public class DirectAttackScript : SkillScript
{
	public ProjectileScript			projectile;
	// Use this for initialization
	void Start () {


	}

	public override bool SelectSkill ()
	{
		return true;
	}

	public override void ApplyEffect (Vector3 target, Vector3 origin)
	{
		Instantiate (projectile, origin, Quaternion.LookRotation(target - origin));
	}

	// Update is called once per frame
	void Update () {
	
	}
}
