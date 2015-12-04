using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BuffScript
{
	[System.Serializable]
	public class PassiveStatChange
	{
		public int			str;
		public int			agi;
		public int			con;
		public int			intel;
		public int			hp;
		public int			mana;
		public int			damage;
		public int			attackSpeed;
		public float		cooldownReduction;
		public int			pCon;
		public int			pStr;
		public int			pAgi;
		public int			pIntel;
		public int			pDamage;
		public int			pHp;
		public int			pMp;
	}

	public List<PassiveStatChange>	buffs;

	public BuffScript()
	{
		buffs = new List<PassiveStatChange>();
	}

	public int		str
	{
		get
		{
			int ret = 0;
			foreach (PassiveStatChange buff in buffs)
				ret += buff.str;
			return ret;
		}
		private set {}
	}

	public int		agi
	{
		get
		{
			int ret = 0;
			foreach (PassiveStatChange buff in buffs)
				ret += buff.agi;
			return ret;
		}
		private set {}
	}

	public int		con
	{
		get
		{
			int ret = 0;
			foreach (PassiveStatChange buff in buffs)
				ret += buff.con;
			return ret;
		}
		private set {}
	}

	public int		intel
	{
		get
		{
			int ret = 0;
			foreach (PassiveStatChange buff in buffs)
				ret += buff.intel;
			return ret;
		}
		private set {}
	}

	public int		hp
	{
		get
		{
			int ret = 0;
			foreach (PassiveStatChange buff in buffs)
				ret += buff.hp;
			return ret;
		}
		private set {}
	}

	public int		mana
	{
		get
		{
			int ret = 0;
			foreach (PassiveStatChange buff in buffs)
				ret += buff.mana;
			return ret;
		}
		private set {}
	}

	public int		damage
	{
		get
		{
			int ret = 0;
			foreach (PassiveStatChange buff in buffs)
				ret += buff.damage;
			return ret;
		}
		private set {}
	}


	public int		attackSpeed
	{
		get
		{
			int ret = 0;
			foreach (PassiveStatChange buff in buffs)
				ret += buff.attackSpeed;
			return ret;
		}
		private set {}
	}

	public float	cooldownReduction
	{
		get
		{
			float ret = 0.0f;
			foreach (PassiveStatChange buff in buffs)
				ret += buff.cooldownReduction;
			return ret;
		}
		private set {}
	}

	public int		pCon
	{
		get
		{
			int ret = 0;
			foreach (PassiveStatChange buff in buffs)
				ret += buff.pCon;
			return ret;
		}
		private set {}
	}

	public int		pStr
	{
		get
		{
			int ret = 0;
			foreach (PassiveStatChange buff in buffs)
				ret += buff.pStr;
			return ret;
		}
		private set {}
	}

	public int		pIntel
	{
		get
		{
			int ret = 0;
			foreach (PassiveStatChange buff in buffs)
				ret += buff.pIntel;
			return ret;
		}
		private set {}
	}

	public int		pAgi
	{
		get
		{
			int ret = 0;
			foreach (PassiveStatChange buff in buffs)
				ret += buff.pAgi;
			return ret;
		}
		private set {}
	}

	public int		pDamage
	{
		get
		{
			int ret = 0;
			foreach (PassiveStatChange buff in buffs)
				ret += buff.pDamage;
			return ret;
		}
		private set {}
	}

	public int		pHp
	{
		get
		{
			int ret = 0;
			foreach (PassiveStatChange buff in buffs)
				ret += buff.pHp;
			return ret;
		}
		private set {}
	}

	public int		pMp
	{
		get
		{
			int ret = 0;
			foreach (PassiveStatChange buff in buffs)
				ret += buff.pMp;
			return ret;
		}
		private set {}
	}

	public void AddBuff (PassiveStatChange psc)
	{
		float			hpProportion = 0.0f;
		float			mpProportion = 0.0f;
		PlayerScript player = PlayerScript.instance;

		if (psc.con != 0 || psc.pCon != 0)
			hpProportion = player.current_hp / (float)player.hpMax;
		if (psc.intel != 0 || psc.pIntel != 0)
			mpProportion = player.current_mana / (float)player.manaMax;
		buffs.Add (psc);
		if (psc.hp != 0 || psc.pHp != 0)
			player.current_hp = Mathf.Clamp (player.current_hp + psc.hp, 0, int.MaxValue);
		if (psc.mana != 0 || psc.pMp != 0)
			player.current_mana = Mathf.Clamp (player.current_mana + psc.mana, 0, int.MaxValue);
		if (psc.con != 0 || psc.pCon != 0)
			player.current_hp = Mathf.RoundToInt (Mathf.Clamp (player.hpMax * hpProportion, 0, int.MaxValue));
		if (psc.intel != 0 || psc.pIntel != 0)
			player.current_mana = Mathf.RoundToInt (Mathf.Clamp (player.manaMax * mpProportion, 0, int.MaxValue));
	}

	public void RemoveBuff(PassiveStatChange psc)
	{
		float			hpProportion = 0.0f;
		float			mpProportion = 0.0f;
		PlayerScript player = PlayerScript.instance;

		if (buffs.Find (x => x == psc) == null)
			return ;
		if (psc.con != 0 || psc.pCon != 0)
			hpProportion = player.current_hp / (float)player.hpMax;
		if (psc.intel != 0 || psc.pIntel != 0)
			mpProportion = player.current_mana / (float)player.manaMax;
		buffs.Remove(psc);
		if (psc.hp != 0 || psc.pHp != 0)
			player.current_hp = Mathf.Clamp (player.current_hp - psc.hp, 0, int.MaxValue);
		if (psc.mana != 0 || psc.pMp != 0)
			player.current_mana = Mathf.Clamp (player.current_mana - psc.mana, 0, int.MaxValue);
		if (psc.con != 0 || psc.pCon != 0)
			player.current_hp = Mathf.RoundToInt (Mathf.Clamp (player.hpMax * hpProportion, 0, int.MaxValue));
		if (psc.intel != 0 || psc.pIntel != 0)
			player.current_mana = Mathf.RoundToInt (Mathf.Clamp (player.manaMax * mpProportion, 0, int.MaxValue));
	}
	
}
