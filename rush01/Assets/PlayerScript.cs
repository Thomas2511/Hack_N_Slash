using UnityEngine;
using System.Collections;

public class PlayerScript : MonoBehaviour {
	private NavMeshAgent		_navMeshAgent;
	private Animator			_animator;

	public	bool				run; 
	// Use this for initialization
	void Start () {
		_navMeshAgent = GetComponent<NavMeshAgent>();
		_animator = GetComponent<Animator>();
	}

	void FollowMouse ()
	{
		if (Input.GetMouseButtonDown (0))
		{
			RaycastHit hit;
			if (Physics.Raycast (Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
			{
				if (hit.collider.tag == "Ground")
				{
					_navMeshAgent.destination = hit.point;
					_navMeshAgent.stoppingDistance = 0.0f;
				}
			}
		}
	}

	void RunAnimation ()
	{
		_animator.SetBool("Run", _navMeshAgent.velocity != Vector3.zero);
	}
	
	// Update is called once per frame
	void Update () {
		FollowMouse();

		RunAnimation();
	}
}
