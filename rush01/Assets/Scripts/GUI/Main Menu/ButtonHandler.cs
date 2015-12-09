using UnityEngine;
using System.Collections;

public class ButtonHandler : MonoBehaviour {

	// Use this for initialization
	public void Newgame ()
	{
		Application.LoadLevel ("Dungeon2");
	}

	public void Loadgame ()
	{
	}

	public void Exitgame ()
	{
		Application.Quit ();
	}
}
