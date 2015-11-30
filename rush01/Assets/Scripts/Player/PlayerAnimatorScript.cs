using UnityEngine;
using System.Collections;

public class PlayerAnimatorScript : MonoBehaviour {

	public void applyDamage()
	{
		PlayerScript.instance.applyDamage ();
	}

	public void attackBegin()
	{
		PlayerScript.instance.attackBegin ();
	}

	public void attackOver()
	{
		PlayerScript.instance.attackOver ();
	}

	public void equip()
	{
		PlayerScript.instance.equip();
	}

	public void unequip()
	{
		PlayerScript.instance.unequip ();
	}
}
