using UnityEngine;
using System.Collections;

public class AOEAttackScript : SimpleEffectScript {

	public SphereCollider	sphereCollider;
	public int				damage;
	public float			radius { get { return this.sphereCollider.radius; } set { this.sphereCollider.radius = value; }}

	protected override void Awake()
	{
		this.sphereCollider = GetComponent<SphereCollider>();
		Invoke("Destroy", GetComponent<ParticleSystem>().duration);
		this.sphereCollider.radius = radius;
		base.Awake();
	}

	public void SetDamage(int damage)
	{
		this.damage = damage;
	}

	public void SetRadius(float radius)
	{
		this.sphereCollider.radius = radius;
	}

	protected virtual void Destroy()
	{
		StopAllCoroutines ();
		Destroy (gameObject);
		Destroy (this);
	}

	protected virtual IEnumerator FadeIntensity ()
	{
		while (true)
		{
			yield return new WaitForEndOfFrame();
			lightComponent.intensity -= 0.05f;
		}
	}

	void OnTriggerEnter(Collider col)
	{
		if (col.gameObject.tag == "Enemy")
			col.gameObject.GetComponent<Enemy>().RecieveDamage(damage);
	}
}
