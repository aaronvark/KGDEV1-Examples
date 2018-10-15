using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MVCExample {
	public enum GameAction {
		USE
	}

	public class VehicleStats {
		public float speed = 1;
		public float size = 1;
		
		public VehicleStats( float speed, float size ) {
			this.speed = speed;
			this.size = size;
		}
	}
}