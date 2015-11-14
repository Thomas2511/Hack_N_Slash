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

	public static Dictionary<Curve, System.Func<int, int, int>> table = new Dictionary<Curve, System.Func<int, int, int>>
	{
		{Curve.LOG, UseLog},
		{Curve.LINEAR, UseLinear},
		{Curve.LINEAR2X, UseLinear2X},
		{Curve.SQR, UseSqr}
	};

	public static int ApplyCurve(Curve curve, int level, int stat)
	{
		return table[curve](stat, level);
	}

	public static int UseLog (int stat, int level) {
		return stat + (int) Mathf.Log (level);
	}

	public static int UseLinear (int stat, int level) {
		return stat + level;
	}

	public static int UseLinear2X(int stat, int level)
	{
		return stat + level * 2;
	}

	public static int UseSqr (int stat, int level) {
		return stat * level * level;
	}
}
