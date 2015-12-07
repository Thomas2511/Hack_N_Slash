using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EnemyHealthBar : MonoBehaviour {

	private Enemy _enemy;

	void Update () {
		_enemy = GetComponentInParent<Enemy> ();
		GetComponent<Image> ().fillAmount = (_enemy.current_hp * 100f / _enemy.hpMax) / 100f;
	}
}
