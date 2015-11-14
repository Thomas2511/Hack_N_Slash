using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Curves : MonoBehaviour {

	public static Curves instance;
	public enum Stat
	{
		STRENGTH,
		AGILITY,
		CONSTITUTION,
		ARMOR,
		EXPERIENCE,
		MONEY
	}
	public enum Curve
	{
		LOG,
		LINEAR,
		LINEAR2X,
		SQR
	}
	
	[System.Serializable]
	public class StatCurve
	{	
		public Stat stat;
		public Curve curve;
	}

	public static Dictionary<Curve, System.Func<uint, uint, uint>> table = new Dictionary<Curve, System.Func<uint, uint, uint>>
	{
		{Curve.LOG, UseLog},
		{Curve.LINEAR, UseLinear},
		{Curve.LINEAR2X, UseLinear2X},
		{Curve.SQR, UseSqr}
	};

	public static uint ApplyCurve(Curve curve, uint level, uint stat)
	{
		return table[curve](stat, level);
	}

	public static uint UseLog (uint stat, uint level) {
		return stat + (uint) Mathf.Log (level);
	}

	public static uint UseLinear (uint stat, uint level) {
		return stat + level;
	}

	public static uint UseLinear2X(uint stat, uint level)
	{
		return stat + level * 2;
	}

	public static uint UseSqr (uint stat, uint level) {
		return stat * level * level;
	}
}
