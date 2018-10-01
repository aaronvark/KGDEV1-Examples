using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Les5 {

	public delegate void IStateEvent(IState state);
	public delegate void AStateEvent(AState state);

	//Je kan een interface gebruiken
	public interface IState {
		event IStateEvent onState;
		void Start();
		void Run();
	}

	//Die je dan zo implementeerd voor een class die "telt"
	public class CountToHundredState : IState {
		public event IStateEvent onState;

		int count = 0;
		public void Start() {

		}

		public void Run() {
			count++;

			if ( count > 100 ) {
				//Go to some other state
				onState(new CountToHundredState());
			}
		}
	}

	//Maar ook een abstract class
	public abstract class AState {
		//Deze event is al bruikbaar, en hoef je dus nergens te implementeren!
		
		//Alleen classes die ervan van AState mogen onState invoken
		protected AStateEvent onState;	
		//Hier gebruiken we een "public event" om de delegate aan te spreken, zonder dat je hem mag "invoken"
		//Dit is waar de "event" keyword voor is, het is een soort "aanspreekpunt", zonder dat je direct toegang verleent
		//Meer info over delegates & events: http://csharpindepth.com/Articles/Chapter2/Events.aspx
		public event AStateEvent OnState {
			add {
				onState += value;
			}
			remove {
				onState -= value;
			}
		}

		//Start is virtual, en heeft een lege body. Implementatie is dus "optioneel"
		public virtual void Start() {}

		//Run geven we abstract omdat we vinden dat deze "moet" worden geimplementeerd, anders heeft de State geen nut?
		public abstract void Run();

		//En vaak zie je nog een soort opruim methode
		public virtual void Completed() {}
	}

	//En de implementatie daarvan is veel simpeler (dat is dan ook het voordeel!)
	public class CountToFiftyState : AState {
		int count = 0;

		public override void Run() {
			count++;
			if ( count > 50 ) {
				onState(new CountToFiftyState());
			}
		}
	}

	//En dan nog een voorbeeld van hoe je states kunt gebruiken:
	//Dit is een soort pattern, de "Finite State Machine" die je veel ziet in games
	public class FiniteStateMachine {
		protected AState currentState;

		public FiniteStateMachine( AState beginState ) {
			SetState(beginState);
		}

		private void SetState( AState newState ) {
			//Clean up old state
			if ( currentState != null ) {
				currentState.Completed();
				//Niet vergeten te stoppen met luisteren!
				currentState.OnState -= SetState;
			}

			//Initialize new state
			newState.Start();
			//We luisteren naar wanneer states aangeven dat er gewisseld moet worden
			newState.OnState += SetState;

			//Assign to current
			currentState = newState;
		}

		//Aangezien dit geen MonoBehaviour is moeten we dit nog wel ergens aanroepen.
		//Dit ontwerp maakt het wel makkelijker om met states te werken binnen MonoBehaviours, 
		// omdat je niet met components in de weer hoeft!
		public void Update( float deltaTime ) {	//deltaTime meegeven kan handig zijn om zelf snelheid te controleren
			if ( currentState != null ) {
				currentState.Run();
			}
		}
	}


	//Nog een gebruik van abstract
	public class ASomeImportantSystem {
		static ASomeImportantSystem system;

		//Dit wordt ook wel een "Factory Method" genoemd.
		//Je roept het aan, en het maakt een instance die je nodig hebt (zoals een fabriek).
		//Het komt in de buurt van een "Abstract Factory Pattern", maar niet helemaal.
		public static ASomeImportantSystem CreateSystem() {
			//ook een soort Singleton implementatie is mogelijk hier
			if ( system != null ) return system;

			switch( Application.platform ) {
				case RuntimePlatform.Android: 
					system = new ImportantAndroidSystem();
					break;
				case RuntimePlatform.IPhonePlayer:
					system = new ImportantiOSSystem();
					break;
				default:
					system = new ImportantDebugSystem();
					break;
			}

			return system;
		}

		public virtual void SomeImportantFunction() {

		}
	}

	#region AImportantSystem implementations

	public class ImportantAndroidSystem : ASomeImportantSystem {

	}

	public class ImportantiOSSystem : ASomeImportantSystem {

	}

	public class ImportantDebugSystem : ASomeImportantSystem {

	}

	#endregion

	//En dan waarom je dat nodig zou kunnen hebben:
	public class GameCode {
		ASomeImportantSystem system;
		public GameCode() {
			//Hier willen we vooral praten met het systeem vanuit het doel, maar de platform details zijn niet belangrijk
			//Need to know basis, en de "game code" doesn't need to know.
			system = ASomeImportantSystem.CreateSystem();
			system.SomeImportantFunction();
		}
	}

	//And if we have time, here's a cool example of combining Generics with Abstracts to make a generic abstract Singleton
	//You could use something like this for all Singletons in a project, to enforce a "style" of singleton
	public abstract class Singleton<T> where T : new() {
		protected static T instance;
		public static T Instance {
			get {
				if ( instance == null ) instance = new T();
				return instance;
			}
		}
	}

	//And using it looks like this. No need to keep reconstructing that pesky Property.
	public class SomeSingleton : Singleton<SomeSingleton> {

	}
}