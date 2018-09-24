using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InDeLes {

	public /*static*/ class Ding {
		public static int numDingen = 0;

		//singleton
		private static Ding instance = null; //new Ding();
		public static Ding Instance {
			get {
				//besta ik al?
				if ( instance == null ) {
					//nee? maak mezelf aan
					instance = new Ding();
				}

				//return mezelf
				return instance;
			}
		}

		public Ding() {
			numDingen++;
		}
	}

	public class AnderDing {
		public AnderDing() {
			//C++:
			//static int x = 5;
			for( int i = 0; i < 5; ++i ) {
				Ding mijnDing = new Ding();
			}
			Debug.Log(Ding.numDingen);
		}
	}

	public delegate void DamageEvent(object sender, int amount);

	public class EnemyManager {
		public EnemyManager() {
			Enemy myEnemy = new Enemy();
			myEnemy.onDeath += SomeIdiotGotHit;
		}

		private void SomeIdiotGotHit( object sender, int thisMuch ) {
			Enemy e = sender as Enemy;
			e.onDeath -= SomeIdiotGotHit;
		}
	}

	public class Enemy {
		public DamageEvent onDeath;
		public Enemy() {
			if ( onDeath != null ) {
				onDeath(this, 9999999);
			}
		}
	}
}
