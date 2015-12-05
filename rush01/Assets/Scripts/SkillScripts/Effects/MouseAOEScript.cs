using UnityEngine;
using System.Collections;

public class MouseAOEScript : MonoBehaviour {
	public delegate void		MouseEvent(Vector3 pos);
	public event MouseEvent		onMouseClick;
	public event MouseEvent		onCancel;
	public LayerMask			layerMask;
	public int					range;
	public Projector[]			projectors;

	void Start()
	{
		projectors = GetComponentsInChildren<Projector> ();
		foreach (Projector projector in projectors) {
			projector.orthographicSize = range;
		}
	}

	void FollowMouse ()
	{
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
		if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
		{
			transform.position = hit.point + new Vector3(0, 10, 0);
		}
	}

	void InputManager ()
	{
		if (Input.GetMouseButtonUp (0))
		{
			onMouseClick(this.transform.position - new Vector3(0, 10, 0));
			StartCoroutine (DestroyMe());
		}
		if (Input.GetMouseButtonUp (1))
		{
			onCancel(Vector3.zero);
			StartCoroutine (DestroyMe ());
		}
	}

	IEnumerator DestroyMe ()
	{
		yield return new WaitForEndOfFrame();
		Destroy (gameObject);
		Destroy (this);
	}

	void Update () {
		FollowMouse();
		InputManager();
	}
}
