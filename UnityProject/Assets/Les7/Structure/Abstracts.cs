using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MVCExample {
	public abstract class AVehicleController : MonoBehaviour, IVehicleController {
		protected IVehicleView activeView;
		protected IActionMapper inputSystem;
		protected List<GameAction> actions = new List<GameAction>();

		protected virtual void Awake() {
			inputSystem = InputManager.GetInputSystem();
			activeView = VehicleManager.GetDefaultView(transform.position);
			activeView.SetController(this);
		}

		public void LostControl( IVehicleView target ) {
			if ( target == activeView ) {
				//TODO: make sure we can get a default view somewhere (like when you're a pedestrian in GTA2)
				activeView = VehicleManager.GetDefaultView(target.Position);
				activeView.SetController(this);
			}
		}

		public void GainedControl( IVehicleView target ) {
			if ( activeView == target ) {
				//Debug.Log("same one");
				return;
			}

			if ( activeView != null ) {
				//perhaps disable this view?
			}

			activeView = target;
		}
	}

	public abstract class AVehicleView : MonoBehaviour, IVehicleView {		
		public VehicleStats Stats { 
			get;
			protected set;
		}

		public Vector3 Position {
			get {
				return transform.position;
			}
		}

		public IVehicleController ActiveController { 
			get; private set;
		}

		public virtual void HandleInput( float horizontal, float vertical, List<GameAction> actions ) {
			transform.Translate( horizontal * Stats.speed, 0, vertical * Stats.speed );
		}

		public virtual void SetController( IVehicleController controller ) {
			if ( ActiveController == controller ) {
				return;										//nothing changes
			}

			if ( controller == null ) {						//we're being assigned nothing, so end control of this view
				EndControl();
			}
			else {
				controller.GainedControl( this );			//notify our new controller that we're ready to be used
			}

			if ( ActiveController != null ) {				//we had a controller
				ActiveController.LostControl( this );		//tell it that it has lost control of this view
			}
			else {
				StartControl();								//we didn't have a controller, so start control
			}

			ActiveController = controller;					//store active controller
		}

		public abstract void EndControl();
		public abstract void StartControl();
	}
}