using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InDeLes {

	public /*static*/ class Ding {
		//Er kan er maar een zijn (kijk eens naar Highlander)
		public static int numDingen = 0;

		//Singleton :static, globally accessible, reference to self
		private static Ding instance = null; //new Ding();
		public static Ding Instance {
			//lazy, just-in-time, initialization
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
			//zelfde variabele reference voor elke instance van Ding
			numDingen++;
		}
	}

	public class AnderDing {
		public AnderDing() {
			//In C++ kan dit volgens mij ook (language feature):
			//static int x = 5;
			// Tip: Dit heeft Ronimo dus specifiek banned in hun coding guidelines

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
			//Enemy gaat insta-dood on-spawn (niet echt een practical use-case)
			// - Zie Robocop 2
			if ( onDeath != null ) {
				//optioneel, maar wel handig: reference naar "sender" van het event meesturen
				onDeath(this, 9999999);
			}
		}
	}
}
