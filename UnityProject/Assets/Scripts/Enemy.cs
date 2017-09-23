using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void EnemyEvent(Enemy e);

/// <summary>
/// Simple example of an Enemy that self-registers with a Manager
/// The manager implements an Observer pattern with the events pushed by the Enemy
/// </summary>
public class Enemy : MonoBehaviour {
	public event EnemyEvent onDeath;
	public event EnemyEvent onSpottedPlayer;

	int health = 10;

	void Start() {
		EnemyManager.RegisterEnemy(this);
	}

	void OnDestroy() {
		EnemyManager.UnregisterEnemy(this);
	}

	void Update() {
		//if ( spotted player ) {
			if (onSpottedPlayer != null) {
				onSpottedPlayer(this);
			}
		//}

		if (health <= 0) {
			if (onDeath != null) {
				onDeath(this);
			}
		}
	}
}
