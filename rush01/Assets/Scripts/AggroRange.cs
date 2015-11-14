using UnityEngine;
using System.Collections;

public class AggroRange : MonoBehaviour {

	public GameObject intruder;

	void OnTriggerStay (Collider col) {
		if (col.gameObject.tag == "Player")
			intruder = col.gameObject;
	}

	void OnTriggerExit (Collider col) {
		if (col.gameObject.tag == "Player")
			intruder = null;
	}
}
