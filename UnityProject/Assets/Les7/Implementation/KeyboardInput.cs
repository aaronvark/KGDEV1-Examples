using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MVCExample {
	public class KeyboardInput : IActionMapper {
		private List<GameAction> actions = new List<GameAction>();
		
		public List<GameAction> GetActions() {
			actions.Clear();
			if ( Input.GetKeyDown(KeyCode.E)) actions.Add( GameAction.USE );
			return actions;
		}

		public void CalculateAxes( out float h, out float v ) {
			h = Input.GetAxis("Horizontal");
			v = Input.GetAxis("Vertical");
		}
	}
}
