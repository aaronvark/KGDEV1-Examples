using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IFeedable {
	///<summary>Returns true if feeding successful</summary>
	bool Feed();
}

public class DierenBeurs : MonoBehaviour {

	private int ledenVanDeBeurs = 1;
	public int LedenVanDeBeurs {
		get {
			//tellen van hoe vaak dit is gebruikt?
			//notify mensen die op dit event zijn geregistreerd
			return ledenVanDeBeurs;
		}
		//protected set;
		private set {
			if ( value == 0 ) {
				//dissolve organisation
			}
			ledenVanDeBeurs = value;
		}
	}

	// Use this for initialization
	void Start () {
		//mag niet als class abstract is
		//Animal myAnimal = new Animal();

		IFeedable[] animals = new IFeedable[20];
		for( int i = 0; i < 10; ++i ) {
			animals[i] = new Dog();
		}
		for( int i = 10; i < 20; ++i ) {
			animals[i] = new Cat();
		}

		foreach( IFeedable a in animals ) {
			a.Feed();
		}
	}
}

abstract class Animal : IFeedable {
	public bool Feed() {
		return false;
	}

	public virtual void Eat() {
		Debug.Log("Om nom nom");
	}
}

class Dog : Animal {
	//new public void Eat() {
	public override void Eat() {
		Debug.Log("Om nom HONDENVOER");
		base.Eat();
	}
}

class Cat : Animal {

}