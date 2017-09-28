using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KGDEV_1_4 {

	public delegate void StateEvent(State newState);

	public abstract class State {
		public StateEvent onState;

		public virtual void Start() {
		}
		public virtual void Run() {
		}
		public virtual void Completed() {
		} 
	}

	public class AliveState : State {
		public override void Run() {
			//how bepaal ik dat ik nog leef?
			//dit kunnen we nu nog niet bepalen...
			//if dead
			onState(new DeadState());
		}
	}

	public class DeadState : State {
	}

	public class FiniteStateMachine {
		private State currentState;

		public void SetState( State newState ) {
			//current complete
			if ( currentState != null ) {
				currentState.Completed();
				currentState.onState -= SetState;
			}

			//new start
			newState.Start();
			newState.onState += SetState;

			//store new as current
			currentState = newState;
		}

		public void Update() {
			if ( currentState != null ) {
				currentState.Run();
			}
		}
	}
}
