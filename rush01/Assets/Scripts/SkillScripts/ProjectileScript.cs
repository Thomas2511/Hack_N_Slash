using UnityEngine;
using System.Collections;

public class ProjectileScript : MonoBehaviour
{
	public Light		lightComponent;
	public int			damage;
	void Awake()
	{
		this.lightComponent = GetComponentInChildren<Light>();
		Invoke("Destroy", 5);
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

	void OnParticleCollision(GameObject other)
	{
		lightComponent.enabled = true;
		lightComponent.gameObject.transform.position = other.transform.position;
		StartCoroutine (FadeIntensity());
		if (other.tag == "Enemy")
		{
			other.GetComponent<Enemy>().ReceiveDamage(damage);
		}
	}
}

