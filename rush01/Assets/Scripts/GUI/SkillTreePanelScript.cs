using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SkillTreePanelScript : MonoBehaviour {

	public bool					active;	
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.N))
		{
			if (active)
			{
				GetComponent<CanvasGroup>().alpha = 0;
				GetComponent<CanvasGroup>().blocksRaycasts = false;
				active = false;
			}
			else
			{
				GetComponent<CanvasGroup>().alpha = 1;
				GetComponent<CanvasGroup>().blocksRaycasts = true;
				active = true;
			}
		}
	}
}