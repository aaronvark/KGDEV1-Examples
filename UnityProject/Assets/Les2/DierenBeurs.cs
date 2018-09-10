using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DierenBeurs : MonoBehaviour {

	// Use this for initialization
	void Start () {
		//mag niet als class abstract is
		//Animal myAnimal = new Animal();

		Animal[] animals = new Animal[20];
		for( int i = 0; i < 10; ++i ) {
			animals[i] = new Dog();
		}
		for( int i = 10; i < 20; ++i ) {
			animals[i] = new Cat();
		}

		foreach( Animal a in animals ) {
			a.Eat();
		}
	}
}

abstract class Animal {
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