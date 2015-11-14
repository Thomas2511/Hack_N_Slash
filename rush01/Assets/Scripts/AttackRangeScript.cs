using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AttackRangeScript : MonoBehaviour {
	private SphereCollider		_collider;
	public List<GameObject>		targets;

	void Start()
	{
		_collider = GetComponent<SphereCollider>();
	}

	void OnTriggerEnter(Collider col)
	{
		targets.Add (col.gameObject);
	}

	void OnTriggerExit(Collider col)
	{
		targets.Remove (col.gameObject);
	}

	public bool isInRange(GameObject target)
	{
		return (targets.Find (x => x == target) != null);
	}

	public void SetColliderRange(float radius)
	{
		_collider.radius = radius;
	}
}
