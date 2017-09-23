using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Example of a Singleton implementation for an Enemy Manager
/// Handles events pushed by enemies, and implements game rules concerning these events
/// 
/// This is a non-MonoBehaviour example, you could also do this with a MonoBehaviour
/// </summary>
public class EnemyManager {

	private static EnemyManager instance;
	protected static EnemyManager Instance {
		get {
			if (instance == null) {
				instance = new EnemyManager();
			}
			return instance;
		}
		//We probably don't even need the setter in this case
		private set {
			instance = value;
		}
	}

	private List<Enemy> enemies = new List<Enemy>();

	//These functions serve as public static access points, without granting full control of the instance
	public static void RegisterEnemy(Enemy e) {
		Instance.Add(e);
	}

	public static void UnregisterEnemy(Enemy e) {
		Instance.Remove(e);
	}

	void Add(Enemy e) {
		if (!enemies.Contains(e)) {
			enemies.Add(e);
		}
		e.onDeath += EnemyDeath;
		e.onDeath += EnemySpottedPlayer;
	}

	void Remove(Enemy e) {
		if (enemies.Contains(e)) {
			enemies.Remove(e);
		}
		e.onDeath -= EnemyDeath;
		e.onDeath -= EnemySpottedPlayer;
	}


	private void EnemyDeath(Enemy e) {
		//scare enemies in radius?
	}

	private void EnemySpottedPlayer(Enemy e) {
		//broadcast player location to other enemies in the area?
	}
}
