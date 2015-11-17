using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SkillTreePanelScript : MonoBehaviour {
	public GameObject			panel;
	public bool					active;	
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.N))
		{
			panel.SetActive (active);
			active = !active;
		}
	}
}