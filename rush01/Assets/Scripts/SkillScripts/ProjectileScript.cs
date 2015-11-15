using UnityEngine;
using System.Collections;

public class ProjectileScript : MonoBehaviour
{
	void Awake()
	{
		Invoke("Destroy", 5);
	}

	void Destroy()
	{
		Destroy (gameObject);
		Destroy (this);
	}

	void OnParticleCollision(GameObject other)
	{
		// TODO degats
	}
}

