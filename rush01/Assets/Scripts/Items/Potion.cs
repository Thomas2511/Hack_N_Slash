using UnityEngine;
using System.Collections;

public class Potion : MonoBehaviour {

	void Lootable () {
		this.GetComponent<BoxCollider> ().enabled = true;
	}

	void Awake () {
		Invoke ("Lootable", 1.0f);
	}
}
