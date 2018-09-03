using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Conventions {
	public class NewBehaviourScript : MonoBehaviour {
		//hard cap: 1000 lines cap for length, unless you have a really good reason

		//Language: American English, initialize, color, center

		//PascalCaseForFunctions
		//camelCaseForVariable

		//first variables, then functions

		//variable/function? order
		//public above

		//public variables always properties?
		// -> always use properties for public access to variables
		//		Discuss next week
		public int SomeProperty {
			get;
			protected set;
		}


		//other groupings left to developer

		//multi-declarations allowed when they make sense (comment!)
		private int health, strength, charisma, devskills;
		private float semiEatenBananas = 2.5f, sandwiches = 3.6f;	//multi-init niet



		//spaces before & after operators
		int x=5; //bad
		private int y = 5; //good


		//comment about function (double enter before and after)
		private void MyAwesomeFunction(int x, int y, int z) {
			//try to keep lines within the standard resolution width of 1920x1080, because otherwise people will not be able to read your super important comments about why you went to school at all

			bool condition = true;
			//standard if-statement (allowed, duh)
			if ( condition ) {
				int x = 5;
			}

			//two-line if-statement (streng afraden)
			if ( condition )
				int x = 5;

			//one-line if-statement (mag, aangeraden voor korte instructies)
			if ( condition ) int x = 5;

			//ternare operator -> nested = banned
			bool doIt = x > 5 ? ( y < 5 ? false : true ) : false;
		}


		// Use this for initialization
		private void Start () {
			
		}

		
		// Update is called once per frame
		private void Update () {
			
		}
	}
}