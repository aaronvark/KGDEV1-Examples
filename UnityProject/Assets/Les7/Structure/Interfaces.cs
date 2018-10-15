using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MVCExample {

	//view is the visual representation of a model (so, a car, truck, pedestrian, etc.)
	public interface IVehicleView {
		IVehicleController ActiveController { 
			get; 
		}

		Vector3 Position {
			get;
		}

		void HandleInput( float horizontal, float vertical, List<GameAction> actions );
		void SetController( IVehicleController controller );
	}

	public interface IVehicleController {
		void LostControl( IVehicleView target );
		void GainedControl( IVehicleView target );
	}

	public interface IActionMapper {
		List<GameAction> GetActions();
		void CalculateAxes( out float horizontal, out float vertical );
	}
}