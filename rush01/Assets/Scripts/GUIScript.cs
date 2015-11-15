using UnityEngine;
using System.Collections;

public class GUIScript : MonoBehaviour {

	public GUISkin skin;

	void OnGUI () {
		GUI.skin = skin;
		GUI.Button (new Rect (25, 25, 100, 30), "Test");
	}

}
