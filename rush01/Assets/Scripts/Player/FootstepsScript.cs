using UnityEngine;
using System.Collections;

public class FootstepsScript : MonoBehaviour {
	public AudioClip[]		sounds;
	// Use this for initialization
	void Start () {
	
	}

	void OnCollisionEnter(Collision col) 
	{
		if (col.gameObject.tag == "Ground" && !GetComponent<AudioSource>().isPlaying)
		{
			GetComponent<AudioSource>().clip = sounds[Random.Range(0,sounds.Length)];
			GetComponent<AudioSource>().Play();
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
