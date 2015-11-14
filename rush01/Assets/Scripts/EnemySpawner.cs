using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour {

	public int density;
	public int radius;
	public float spawnTime;
	public Enemy.EnemyType type;
	public List<Enemy> enemies;
	
	private Enemy _spawn;
	private Enemy _clone;
	private int _spawnCount;

	void SelectEnemyPrefab () {
		switch (type) {
			default: //type = NONE
				_spawn = enemies[0];
			break;
		}
	}

	void SpawnEnemy () {
		Vector3 position;

		for (int i = 0; i < density; i++) {
			position = new Vector3 (this.transform.position.x + Random.Range (-radius, radius),
			                        this.transform.position.y,
			                        this.transform.position.z + Random.Range (-radius, radius));
			_clone = Instantiate (_spawn, position, Quaternion.Euler(0f, 0f + Random.Range(0, 360), 0f)) as Enemy;
			_clone.OnDeath += OnEnemyDeathListener;
			_clone.type = type;
			if (PlayerScript.instance != null)					// <--- TO REMOVE
				_clone.level = (uint) PlayerScript.instance.level;
			else 												// <--- TO REMOVE
				_clone.level = 1; 								// <--- TO REMOVE
		}
		_spawnCount = density;
	}

	void OnEnemyDeathListener () {
		_spawnCount--;
		if (_spawnCount <= 0)
			Invoke ("SpawnEnemy", spawnTime);
	}

	void Start () {
		SelectEnemyPrefab ();
		SpawnEnemy ();
	}
}
