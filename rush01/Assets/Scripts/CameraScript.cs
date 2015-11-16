using UnityEngine;
using System.Collections;

public class CameraScript : MonoBehaviour {
	public Vector3 offset;
	// Use this for initialization
	void Start () {
		offset = new Vector3(0, 5, -8);
	}
	
	// Update is called once per frame
	void Update()
	{
		transform.position = PlayerScript.instance.transform.position + offset;
	}
}
