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

	private Dictionary<Curve, System.Func<int, int, int>> table;

	void Start () {
		instance = this;
		table = new Dictionary<Curve, System.Func<int, int, int>>();
		table [Curve.LOG] = UseLog;
		table [Curve.LINEAR] = UseLinear;
		table [Curve.LINEAR2X] = UseLinear2X;
		table [Curve.SQR] = UseSqr;
	}

	public int ApplyCurve(Curve curve, int level, int stat)
	{
		return table[curve](stat, level);
	}

	public int UseLog (int stat, int level) {
		return stat + (int) Mathf.Log (level);
	}

	public int UseLinear (int stat, int level) {
		return stat + level;
	}

	public int UseLinear2X(int stat, int level)
	{
		return stat + level * 2;
	}

	public int UseSqr (int stat, int level) {
		return stat * level * level;
	}

}
