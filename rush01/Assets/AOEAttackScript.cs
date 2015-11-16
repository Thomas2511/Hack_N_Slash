using UnityEngine;
using System.Collections;

public class AOEAttackScript : MonoBehaviour {

	public Light		lightComponent;
	void Awake()
	{
		this.lightComponent = GetComponentInChildren<Light>();
		Invoke("Destroy", GetComponent<ParticleSystem>().duration);
		if (lightComponent != null)
			StartCoroutine (FadeIntensity());
	}

	void Destroy()
	{
		StopAllCoroutines ();
		Destroy (gameObject);
		Destroy (this);
	}

	IEnumerator FadeIntensity ()
	{
		while (true)
		{
			yield return new WaitForEndOfFrame();
			lightComponent.intensity -= 0.05f;
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
