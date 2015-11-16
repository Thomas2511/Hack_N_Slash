using UnityEngine;
using System.Collections;

public class MouseAOESpell : MonoBehaviour {
	public delegate void		MouseEvent(Vector3 pos);
	public event MouseEvent		onMouseClick;
	public LayerMask			layerMask;
	public int					range;
	// Use this for initialization
	void Start () {
	
	}

	void FollowMouse ()
	{
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
		
		if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
		{
			transform.position = hit.point + new Vector3(0, 8, 0);
			GetComponent<Light>().cookieSize = range;
		}
	}

	void InputManager ()
	{
		if (Input.GetMouseButtonUp (0))
		{
			onMouseClick(this.transform.position - new Vector3(0, 8, 0));
			StartCoroutine (DestroyMe());
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
