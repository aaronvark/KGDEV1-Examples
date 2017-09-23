using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KGDEV_1_3 {

	public class Examples {
		//This variable is exactly the same (not just value, but location in memory)
		// for every instance of Examples
		public static int someStaticVariable = 10;
		//This variable is different (location in memory) for every instance of Examples
		public int someVariable = 10;

		// Use this for initialization
		public Examples() {
			//Access directly through class, because compiler knows
			// "There can be only one" (highlander, movie, check it out)
			Examples.someStaticVariable = 20;
			Examples instance = new Examples();
			instance.someVariable = 30; 
		}
	}

	//delegate definition
	// defines "format" for functions, so it resembles a "function prototype"
	// format in this case = void ...(Enemy ...);, "..." can be different (names)
	public delegate void EnemyEvent(Enemy sender);

	public class Enemy : MonoBehaviour {
		//instance of the above EnemyEvent delegate
		//this can be called as if it were a function
		//and then performs all of the functions that are "on the list"
		public event EnemyEvent onDeath;
		private bool isDead = false;
		private int health = 0;

		void Start() {
			//It is bad if our design allows people to do this
			// "just don't" is not a good enough excuse
			// (yes, this is an extreme example)
			//Destroy(EnemyManager.Singleton.gameObject);
			EnemyManager.Register(this);
		}

		void Update() {
			//this makes sure the event only happens once
			// (health reaches 0, but isDead is still false)
			if ( !isDead && health <= 0 ) {
				//delegate instances with not registered functions are "null"
				// so always check before you call them.
				if ( onDeath != null ) {
					onDeath(this);
				}

				//set true to prevent it happening again
				isDead = true;
			}
		}
	}

	//singleton!
	//Could be a static class, so you don't need Singleton!
	// Guideline is: if it can be a static class instead, it probably should be
	public class EnemyManager {
		
		//This version of the singleton hides it from "public access"
		// which means we have to provide another way to "interface" with it
		// I like this way better, because it requires you to be more thoughtful
		// about what you expect from people when they use the singleton
		static EnemyManager singleton;

		//Here the property returns an instance of EnemyManager
		// You could also have it return an interface (that you'd need to create)
		// such as IEnemyManager, so you don't give people access to the full "object"
		// This is especially useful if it is a MonoBehaviour
		static EnemyManager Singleton {
			get {
				//"lazy initialisation" = create it when it is first requested
				if ( singleton == null ) {
					singleton = new EnemyManager();
					//if this is a MonoBehaviour
					//singleton = new GameObject("_enemyManager").AddComponent<EnemyManager>();
				}
				return singleton;
			}
		}
		
		//instance variable, that gets cleared upon scene load
		// in a static class, add the "static" keyword, and it stays saved throughout the
		// game's execution (sometimes you might want this, so it's a valid idea)
		List<Enemy> enemyList = new List<Enemy>();

		public static void Register(Enemy e) {
			Singleton.enemyList.Add(e);
			e.onDeath += Singleton.EnemyDied;
			//static class version
			//e.onDeath += EnemyDied;
			//enemyList.Add(e);
		}

		public static void Unregister(Enemy e) {
			Singleton.enemyList.Remove(e);
			//static class version
			//enemyList.Remove(e);
		}

		void EnemyDied( Enemy sender ) {
			//do something

			//Maybe tell other enemies that this enemy is dead
			// so they can respond somehow (aggro, run away, etc.)
			for( int i = 0; i < enemyList.Count; ++i ) {
				//enemyList[i].EnemyDied(sender);
			}
		}
	}
}