using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Basic Finite State Machine tailored to run State objects on a target Entity
/// </summary>
public class FiniteStateMachine {
	State currentState;
	Entity target;

	public FiniteStateMachine(State startState, Entity target) {
		this.target = target;
		if (startState != null) {
			SetState(startState);
		}
	}

	public void SetState(State newState) {
		//complete previous
		if ( currentState != null ) {
			currentState.onState -= SetState;
			currentState.Complete(target);
		}
		
		//initialize new
		newState.Start(target);
		newState.onState += SetState;

		//store as current
		currentState = newState;
	}

	public void Run() {
		if ( currentState != null ) {
			currentState.Run(target);
		}
	}
}

/// <summary>
/// Can be used to send messages about states, such as the onState that transitions to a new state
/// </summary>
/// <param name="state"></param>
public delegate void StateEvent(State state);

/// <summary>
/// These state classes are abstract so that implementations can be "only what we need"
/// I often include some reference to a class (Actor, Entity, etc.) in the functions, that the state then "runs on"
/// </summary>
public abstract class State {
	/// <summary>
	/// Abstract class allows variables!
	/// </summary>
	public StateEvent onState;

	//Empty implementations for functions are virtual so we can override them
	public virtual void Start(Entity e)		{ }
	public virtual void Run(Entity e)		{ }
	public virtual void Complete(Entity e)	{ }
}

/// <summary>
/// Simple entity that runs a state machine and has health
/// </summary>
public class Entity : MonoBehaviour {
	public int Health {
		get;
		protected set;
	}

	FiniteStateMachine stateMachine;

	void Start() {
		stateMachine = new FiniteStateMachine(new AliveState(), this);
	}
}

/// <summary>
/// Minimal implementation, checks health, goes to dead state if health reaches zero
/// </summary>
public class AliveState : State {
	public override void Run(Entity e) {
		if ( e.Health <= 0 ) {
			onState(new DeadState());
		}
	}
}

/// <summary>
/// We need a class (even an empty one) so we have a state to transition to
/// </summary>
public class DeadState : State { }