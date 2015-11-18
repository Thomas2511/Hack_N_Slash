using UnityEngine;
using System.Collections;

public class CameraScript : MonoBehaviour
{
	public Vector3 offset;

	// How long the object should shake for.
	public float shake = 0f;
	
	// Amplitude of the shake. A larger value shakes the camera harder.
	public float shakeAmount = 0.25f;
	public float decreaseFactor = 0.25f;
	
	void Update()
	{
		if (shake > 0)
		{
			transform.position = PlayerScript.instance.transform.position + offset + Random.insideUnitSphere * shakeAmount;
			
			shake -= Time.deltaTime * decreaseFactor;
		}
		else
		{
			shake = 0f;
			transform.position = PlayerScript.instance.transform.position + offset;
		}
	}

	public void Shake(float shake)
	{
		this.shake = shake;
	}

	public void Shake()
	{
		this.shake = 0.25f;
	}
}
