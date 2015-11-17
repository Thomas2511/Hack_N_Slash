using UnityEngine;
using System.Collections;

public class SimpleEffectScript : MonoBehaviour {
	public Light			lightComponent;
	// Use this for initialization
	protected virtual void Awake () {
		this.lightComponent = GetComponentInChildren<Light>();
		if (lightComponent != null)
			StartCoroutine (FadeIntensity());
	}

	protected virtual IEnumerator FadeIntensity ()
	{
		while (true)
		{
			yield return new WaitForEndOfFrame();
			lightComponent.intensity -= 0.05f;
		}
	}

	protected virtual void Destroy()
	{
		StopAllCoroutines ();
		Destroy (gameObject);
		Destroy (this);
	}
}
