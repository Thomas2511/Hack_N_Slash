using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(Animator))]
public abstract class CharacterScript : MonoBehaviour {
	public enum AttackType
	{
		WEAPON_ATTACK,
		SPELL_ATTACK,
		NONE
	}

	// Components
	public	NavMeshAgent		navMeshAgent { get; protected set; }
	public	Animator			animator { get; protected set;}
	public  SphereCollider		sphereCollider { get; protected set; }

	// Utility
	public 		bool			dead;
	protected	bool			_onCoolDown;
	public 		CharacterScript	enemyTarget;

	public	AttackType			_attackType;

	// Stats
	public	int					current_hp;
	[Tooltip("Strength value")]
	public	int					str;
	[Tooltip("Agility value")]
	public	int					agi;
	[Tooltip("Constitution value")]
	public	int					con;
	[Tooltip("Intelligence value")]
	public	int					intel;
	[Tooltip("Experience value")]
	public	long				xp;
	[Tooltip("Money value")]
	public	int					money;
	[Tooltip("Level value between 1 and 50")]
	[Range(1, 50)]
	public	int					level;
	public	int					current_mana;

	public	LayerMask			raycastLayerMask;

	// Calculated Stat
	public	int					strTotal { get  { return (int)(str + buffs.str + str * (buffs.pStr / 100.0f)); }}
	public	int					agiTotal { get	{ return (int)(agi + buffs.agi + agi * (buffs.pAgi / 100.0f)); }}
	public	int					conTotal { get	{ return (int)(con + buffs.con + con * (buffs.pCon / 100.0f)); }}
	public	int					intelTotal { get { return (int)(intel + buffs.intel + intel * (buffs.pIntel / 100.0f)); }}
	public	int					minDamage { get { return (int)((strTotal / 2.0f) + (strTotal / 2.0f) * (buffs.pDamage / 100.0f) + buffs.damage);}}
	public	int					maxDamage { get { return minDamage + weaponDamage;}}
	public	int					hpMax { get { return (int)((5 * conTotal) + (5 * conTotal) * (buffs.pHp / 100.0f) + buffs.hp); } }
	public	int					manaMax { get { return (int)((50 + 5 * (intelTotal)) + (50 + 5 * (intelTotal)) * (buffs.pMp / 100.0f) + buffs.mana); }}
	public virtual int			weaponDamage { get { return 0; } }
	public virtual float		attackCooldown { get { return 0; } }

	public	BuffScript			buffs = new BuffScript();


	// Use this for initialization
	protected virtual void Start () {
		current_hp = hpMax;
		current_mana = manaMax;
		_attackType = AttackType.NONE;
		navMeshAgent = GetComponent<NavMeshAgent>();
		animator = GetComponentInChildren<Animator>();
		sphereCollider = GetComponentInChildren<SphereCollider>();
		StartCoroutine (RegenMana ());
	}
		
	public int GetDamage()
	{
		return Random.Range (minDamage, maxDamage + 1);
	}

	bool targetInRange()
	{
		return (enemyTarget != null && isInRange(enemyTarget));
	}
	
	public abstract bool isInRange(CharacterScript target);

	public abstract void AttackSound ();

	public abstract void DamageSound ();

	public virtual void DamageCharacter() {
		if (enemyTarget != null) {
			float val = 75 + agi - enemyTarget.agi - Random.Range (1, 101);
			bool hit = val > 0 ? true : false;
		
			if (hit && targetInRange ()) {
				enemyTarget.ReceiveDamage (GetDamage ());
				DamageSound ();
			} else {
				enemyTarget.ReceiveDamage(0, true);
			}
		}
	}

	protected IEnumerator RegenMana () {
		while (true) {
			yield return new WaitForSeconds(1.0f);
			current_mana = Mathf.Clamp(current_mana + 1, 0, manaMax);
		}
	}

	protected virtual IEnumerator onCoolDown ()
	{
		_onCoolDown = true;
		yield return new WaitForSeconds(attackCooldown);
		_onCoolDown = false;
	}

	protected void StartCoolDown()
	{
		StartCoroutine (onCoolDown ());
	}

	public void ReceiveDamage (int damage, bool miss = false) {
		int save = current_hp;
		this.current_hp = Mathf.Clamp (this.current_hp - damage, 0, this.hpMax);
		GameObject clone = Instantiate (Resources.Load ("Prefabs/GUI/DamageText", typeof(GameObject)) as GameObject, this.transform.position + new Vector3(0, this.navMeshAgent.height, 0), Quaternion.identity) as GameObject;
		clone.GetComponent<DamageTextScript>().SetText ((!miss) ? ((save - current_hp) <= 0 ? Mathf.Abs(save - current_hp) : damage).ToString () : "Miss", (save - current_hp) < 0);	
	}
}