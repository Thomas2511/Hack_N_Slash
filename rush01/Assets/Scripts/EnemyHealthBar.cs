using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EnemyHealthBar : MonoBehaviour {

	private Enemy _enemy;

	void Update () {
		_enemy = GetComponentInParent<Enemy> ();
		GetComponent<Image> ().fillAmount = (_enemy.currentHp * 100f / _enemy.hp) / 100f;
	}
}
