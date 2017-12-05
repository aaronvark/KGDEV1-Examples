using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MPD {
	//This MonoBehaviour class uses the static constructor of InputManager to inject the input dependency
	// into its controller. The only thing we need to do is update the controller.
	public class Player : MonoBehaviour, IControllableEntity {
		PlayerController controller;

		void Start() {
			controller = new PlayerController (InputManager.GetInputSystem (), this);
		}

		void Update() {
			controller.Update ();
		}

		public void Move( float x, float y ) {
			transform.Translate (x, y, 0);
		}
	}
}