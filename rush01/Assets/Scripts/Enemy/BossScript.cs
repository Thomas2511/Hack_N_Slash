using UnityEngine;
using System.Collections;

public class BossScript : Enemy {
	public BurningGroundAOE	burningGround;
	protected override void Start () {
		dead = false;
		agent.stoppingDistance = 12f;
		StartCoroutine(BurningGround());
	}

	public override void Damage () {
		int val = 75 + agi - PlayerScript.instance.agi - Random.Range (1, 101);
		bool hit = val > 0 ? true : false;
		
		if (Vector3.Distance (this.transform.position, intruder.transform.position) <= 15.0f && hit) {
			PlayerScript.instance.DamagePlayer (GetDamage ());
			swordS.Play ();
		}
	}

	IEnumerator BurningGround()
	{
		while (true)
		{
			if (this.intruder != null)
			{
				Instantiate (burningGround, this.intruder.transform.position, Quaternion.identity);
			}
			yield return new WaitForSeconds(10.0f);
		}	
	}
}
