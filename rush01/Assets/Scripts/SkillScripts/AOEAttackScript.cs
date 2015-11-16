using UnityEngine;
using System.Collections;

public class AOEAttackScript : MonoBehaviour {

	public Light		lightComponent;
	protected virtual void Awake()
	{
		this.lightComponent = GetComponentInChildren<Light>();
		Invoke("Destroy", GetComponent<ParticleSystem>().duration);
		if (lightComponent != null)
			StartCoroutine (FadeIntensity());
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
}
