using UnityEngine;
using System.Collections;

public class AggroRange : MonoBehaviour {

	public GameObject intruder;

	void OnTriggerStay (Collider col) {
		Debug.Log (col.name);
		if (col.gameObject.tag == "Player")
			intruder = col.gameObject;
	}

	void OnTriggerExit (Collider col) {
		if (col.gameObject.tag == "Player")
			intruder = null;
	}
}
