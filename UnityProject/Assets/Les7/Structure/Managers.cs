using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MVCExample {
	public static class InputManager {
		public static IActionMapper GetInputSystem() {
			return new KeyboardInput();
		}
	}

	public static class VehicleManager {
		public static IVehicleView GetDefaultView( Vector3? position = null ) {
			GameObject g = new GameObject("ped");
			if ( position != null ) {
				g.transform.position = position.Value;
			}
			return g.AddComponent<Pedestrian>();
		}
	}
}