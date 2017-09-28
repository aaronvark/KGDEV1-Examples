using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbstractFactoryExample : MonoBehaviour {
	void Start() {
		AbstractFactory instance = AbstractFactory.GetInstance();
		instance.DoSomething();
	}
}