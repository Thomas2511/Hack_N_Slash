using UnityEngine;
using System.Collections;

public class BossScript : Enemy {
	public BurningGroundAOE	burningGround;
	protected override void Start () {
		dead = false;
		navMeshAgent.stoppingDistance = 12f;
		StartCoroutine(BurningGround());
	}

	IEnumerator BurningGround()
	{
		while (true)
		{
			if (this.enemyTarget != null)
			{
				Instantiate (burningGround, this.enemyTarget.transform.position, Quaternion.identity);
			}
			yield return new WaitForSeconds(10.0f);
		}	
	}
}
