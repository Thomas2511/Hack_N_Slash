using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.ImageEffects;
using System.Collections;

public class CameraScript : MonoBehaviour
{
	public Vector3		offset;

	// How long the object should shake for.
	public float		shake = 0f;
	
	// Amplitude of the shake. A larger value shakes the camera harder.
	public float		shakeAmount = 0.25f;
	public float		decreaseFactor = 0.25f;

	public bool			lookAtPlayer = false;

	private BlurOptimized	_bo;
	private Grayscale		_gs;
	private Text			_deathText;
	private RawImage		_deathShadow;	

	IEnumerator DeathFade()
	{
		while (true) {
			yield return new WaitForEndOfFrame ();
			_gs.effectAmount = Mathf.Clamp (_gs.effectAmount + 0.01f, 0, 2);
			_bo.blurSize = Mathf.Clamp (_bo.blurSize + 0.015f, 0, 3);
			_deathText.color = new Color(_deathText.color.r, _deathText.color.g, _deathText.color.b, Mathf.Clamp (_deathText.color.a + 0.002f, 0, 206.0f/255.0f));
			_deathShadow.color = new Color(_deathShadow.color.r, _deathShadow.color.g, _deathShadow.color.b, Mathf.Clamp (_deathShadow.color.a + 0.002f, 0, 144.0f/255.0f));
		}
	}

	void OnDeath () 
	{
		_gs.enabled = true;
		_bo.enabled = true;
		_deathText.enabled = true;
		_deathShadow.enabled = true;
		StartCoroutine("DeathFade", DeathFade());
	}

	void Start ()
	{
		PlayerScript.instance.Death += OnDeath;
		_gs = GetComponent<Grayscale> ();
		_bo = GetComponent<BlurOptimized> ();
		_deathText = GameObject.Find ("DeathText").GetComponent<Text>();
		_deathShadow = GameObject.Find ("DeathShadow").GetComponent<RawImage>();
	}

	void Update()
	{
		if (shake > 0)
		{
			transform.position = PlayerScript.instance.transform.position + offset + Random.insideUnitSphere * shakeAmount;
			
			shake -= Time.deltaTime * decreaseFactor;
		}
		else
		{
			shake = 0f;
			transform.position = PlayerScript.instance.transform.position + offset;
		}
	}

	public void Shake(float shake)
	{
		this.shake = shake;
	}

	public void Shake()
	{
		this.shake = 0.25f;
	}
}
