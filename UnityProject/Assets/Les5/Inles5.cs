using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InLes5 {

	public delegate void IStateEvent( IState state );
	//public delegate void AStateEvent( AState state );

	public interface IState {
		event IStateEvent OnState;
		void Start();
		void Run();
		void Complete();
	}

	public class DeadState : AState {
		public override void Run() {
			onState(new AliveState());
		}
	}

	public class AliveState : AState {
		public override void Run() {
			onState(new DeadState());
		}
	}

	
	public abstract class AState : IState {
		protected IStateEvent onState;
		public event IStateEvent OnState {
			add {
				onState += value;
			}
			remove {
				onState -= value;
			}
		}
		public virtual void Start() {}
		//There's a good reason, but I don't have time to explain
		// what I don't have time to understand.
		public abstract void Run();
		public virtual void Complete() {}
	}

	public class FiniteStateMachine {
		protected IState currentState;

		public void Start( IState startState ) {
			SetState(startState);
		}

		public void Run() {
			if ( currentState != null ) {
				currentState.Run();
			}
		}

		private void SetState( IState newState ) {
			//opruimen
			if ( currentState != null ) {
				currentState.Complete();
				currentState.OnState -= SetState;
			}

			//klaarzetten
			newState.Start();
			newState.OnState += SetState;

			//opslaan
			currentState = newState;
		}
	}

	//Generic class, T = Template
	public class MyList<T> {
		public T[] instances;
	}

	public class Singleton<T> where T : new() {
		private static T instance;
		public static T Instance {
			get {
				if ( instance == null ) instance = new T();
				return instance;
			}
		}
	}

	public class SomeSingleton : Singleton<SomeSingleton> {
		public SomeSingleton() {
			//iets doen
		}
	}
}
