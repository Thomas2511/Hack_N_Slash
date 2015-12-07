using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(TextMesh))]
public class DamageTextScript : MonoBehaviour {
	TextMesh				text;
	public float			speed = 0.1f;

	// Use this for initialization
	void Awake () {
		text = GetComponent<TextMesh> ();
		Invoke ("Destroy", 2);
	}

	void SetText(string text, bool heal)
	{
		this.text.text = text;
		if (heal)
			this.text.color = Color.green;
	}

	void Destroy()
	{
		Destroy (gameObject);
		Destroy (this);
	}
	
	// Update is called once per frame
	void Update () {
		this.transform.LookAt (Camera.main.transform.position - this.transform.position);
		this.transform.localRotation = Camera.main.transform.rotation;
		this.transform.Translate(Vector3.up * speed);
		speed = Mathf.Lerp (speed, 0, 0.2f);
		this.text.color = new Color(this.text.color.r, this.text.color.g, this.text.color.b, Mathf.Lerp (this.text.color.a, 0, 0.4f));
	}
}
