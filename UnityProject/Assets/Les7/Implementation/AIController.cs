using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MVCExample {
	public class AIController : AVehicleController {
		protected override void Awake() {
			//no input system
			activeView = VehicleManager.GetDefaultView();
			activeView.SetController(this);
		} 

		public void Update() {
			if ( activeView != null ) {
				float h, v;
				h = Mathf.Sin(Time.time);
				v = Mathf.Cos(Time.time);
				
				actions.Clear();

				if ( Time.frameCount % 60 == 0 ) {
					actions.Add(GameAction.USE);
				}

				activeView.HandleInput(h, v, actions);
			}
		}
	}
}