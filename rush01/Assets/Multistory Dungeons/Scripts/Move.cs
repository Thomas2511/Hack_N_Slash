using UnityEngine;
using System.Collections;

public class Move : MonoBehaviour {

	private float speed = 7.0f;
	private float gravity = -0.4f;

	void FixedUpdate () {

		float horizontalMovement = Input.GetAxis("Horizontal");
		float verticalMovement = Input.GetAxis("Vertical");

		Vector3 movement = new Vector3 (horizontalMovement, gravity, verticalMovement);
		GetComponent<Rigidbody>().velocity = movement * speed;

	}
}
