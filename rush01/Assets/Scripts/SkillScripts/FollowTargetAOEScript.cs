using UnityEngine;
using System.Collections;

public class FollowTargetAOEScript : MonoBehaviour
{
	public Light				lightComponent;
	public ParticleSystem		particles;
	public GameObject			target;

	protected virtual void Awake()
	{
		this.lightComponent = GetComponentInChildren<Light>();
		this.particles = GetComponent<ParticleSystem>();
	}

	protected void Update()
	{
		if (target != null)
			transform.position = target.transform.position + new Vector3(0, 1, 0);
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

