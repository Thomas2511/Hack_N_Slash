using UnityEngine;
using System.Collections;

public class BossFootScript : MonoBehaviour {

	void OnTriggerEnter(Collider col)
	{
		if (col.tag == "Ground")
		{
			Camera.main.GetComponent<CameraScript>().Shake ();
			if (GetComponent<AudioSource>())
				GetComponent<AudioSource>().Play();
		}
	}
}
