using UnityEngine;
using System.Collections;

public class SimpleEffectScript : MonoBehaviour {
	public Light			lightComponent;
	// Use this for initialization
	protected virtual void Start () {
		this.lightComponent = GetComponentInChildren<Light>();
		if (lightComponent != null)
			StartCoroutine (FadeIntensity());
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
