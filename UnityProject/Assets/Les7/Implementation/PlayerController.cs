using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MVCExample {
	//controller talks to view
	public class PlayerController : AVehicleController {
		public void Update() {
			actions.Clear();

			if ( activeView != null ) {
				float h, v;
				inputSystem.CalculateAxes(out h, out v);
				activeView.HandleInput(h, v, inputSystem.GetActions());
			}
		}
	}
}