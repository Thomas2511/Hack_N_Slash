using UnityEngine;
using System.Collections;

public class CanvasRotateTowardCam : MonoBehaviour {

	void LateUpdate () {
		this.transform.rotation = Camera.main.transform.rotation;
	}
}
