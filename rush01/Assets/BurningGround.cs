using UnityEngine;
using System.Collections;

public class BurningGround : MonoBehaviour {
	public Light				lightComponent;
	public int					damage;
	public float				coolDown;
	public ParticleSystem		particles;
	public GameObject			target;
	public SphereCollider		sphereCollider;
	public bool					onCoolDown;
	public float				radius { get { return sphereCollider.radius; } set { sphereCollider.radius = value; }}
	
	protected virtual void Awake()
	{
		this.lightComponent = GetComponentInChildren<Light>();
		this.sphereCollider = GetComponent<SphereCollider>();
		this.particles = GetComponentInChildren<ParticleSystem>();
		StartCoroutine (CooldownRoutine());
		Invoke ("Destroy", 10.0f);
	}
	
	IEnumerator CooldownRoutine ()
	{
		while (true)
		{
			onCoolDown = true;
			yield return new WaitForSeconds(coolDown);
			yield return new WaitForEndOfFrame();
			onCoolDown = false;
			yield return new WaitForEndOfFrame();
		}
	}
	
	protected void Update()
	{
		if (target != null)
			transform.position = target.transform.position + new Vector3(0, 1, 0);
	}
	
	void OnTriggerStay(Collider col)
	{
		if (col.tag == "Player" && !onCoolDown)
			PlayerScript.instance.current_hp = Mathf.Clamp(PlayerScript.instance.current_hp - damage, 0, PlayerScript.instance.hpMax);
	}
	
	protected virtual IEnumerator FadeIntensity ()
	{
		particles.loop = false;
		while (lightComponent != null && lightComponent.intensity > 0)
		{
			yield return new WaitForEndOfFrame();
			lightComponent.intensity -= 0.5f;
		}
		Destroy (gameObject);
		Destroy (this);
	}
	
	public void Destroy()
	{
		StartCoroutine (FadeIntensity ());
	}
}
