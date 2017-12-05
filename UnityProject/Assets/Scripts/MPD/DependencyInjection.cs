using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MPD {

	public enum GameAction {
		MOVE_VERTICAL,
		MOVE_HORIZONTAL,
	}

	//"clearly" defined interface
	public interface IInputSystem {
		float GetAxis( GameAction action );
	}

	public interface IControllableEntity {
		void Move( float x, float y );
	}

	public class PlayerController {
		
		IInputSystem input;
		IControllableEntity target;

		//this gets called where the player is created
		public PlayerController( IInputSystem injectedDependency, IControllableEntity injectedTarget ) {
			input = injectedDependency;
			target = injectedTarget;
		}

		public void Update() {
			float moveX = input.GetAxis (GameAction.MOVE_HORIZONTAL);
			float moveY = input.GetAxis (GameAction.MOVE_VERTICAL);

			target.Move (moveX, moveY);
		}
	}

	//Here's a common problem we get in Unity, because of MonoBehaviour:

	//We get into a spot of bother here (below) because there is no constructor for MonoBehaviour
	// and we can't just inject the dependency in the Start or Awake functions
	//	One thing you might try is adding a comment that tells people to do this, but
	//   the problem remains: people might do something that breaks this

	//An alternative solution would be: don't use the monobehaviour version, but instead use the 
	// non-mono version on another MonoBehaviour class (like a manager), that can properly inject the dependency
	//	This also means you get forced to think about your structure more, which might improve the overall design.
	//	 (or it might overcomplicate it, beware)

	/// <summary>
	/// Call Init() right after adding this to any gameObject
	/// </summary>
	public class PlayerControllerMono : MonoBehaviour {

		//we can use this to throw errors, or place ASSERTS
		bool initialized = false;
		IInputSystem input;

		//we want people to call this once they've added it to a gameObject
		public void Init( IInputSystem injectedDependency ) {
			input = injectedDependency;
			initialized = true;
		}

		void Update() {
			Debug.Assert (initialized);

			//would be nice to avoid the null check
			if (input != null) {
				float moveX = input.GetAxis (GameAction.MOVE_HORIZONTAL);
			}
		}
	}

	//How we create the input system could be through a factory method in some manager class 
	// where we can use the current platform, or preprocessor directives, to get the desired system
	public class UnityInputSystem : IInputSystem {
		public float GetAxis( GameAction action ) {
			//could also store some kind of conversion Dictionary to get the "names" of axes
			switch (action) {
			case GameAction.MOVE_HORIZONTAL:
				return Input.GetAxis ("Horizontal");
			case GameAction.MOVE_VERTICAL:
				return Input.GetAxis ("Vertical");
			default:
				return 0;
			}
		}
	}


	//Example of a static construction method for use with the non-mono PlayerController
	public static class InputManager {
		static IInputSystem instance;

		public static IInputSystem GetInputSystem() {
			if (instance == null) {
				instance = CreateInstance ();
			}

			return instance;
		}

		static IInputSystem CreateInstance() {
			//platform dependent code
			return new UnityInputSystem();
		}
	}

}