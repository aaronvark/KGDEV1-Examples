using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractFactory {

	public static AbstractFactory GetInstance() {
		switch (Application.platform) {
		#if !DISABLE_SYSTEM
			case RuntimePlatform.WindowsPlayer:
				return new WindowsImplementation();
			case RuntimePlatform.IPhonePlayer:
				return new IPhoneImplementation();
			case RuntimePlatform.Android:
				return new AndroidImplementation();
		#endif
			default:
				//Does nothing, always "succeeds" its requested actions
				//This is for working in editor mainly.
				return new DummyImplementation();
		}
	}

	//abstract "function prototype", that needs to be implemented in the extending class (just like an interface)
	public abstract void DoSomething();

}

public class WindowsImplementation : AbstractFactory {
	//implementing an abstract function requires the "override keyword"
	public override void DoSomething() {
		//do a windows thing
		Debug.Log("Do the Windows hussle");
	}
}
public class IPhoneImplementation : AbstractFactory {
	public override void DoSomething() {
		//do an iPhone thing
		Debug.Log("Do the iPhone hussle");
	}
}
public class AndroidImplementation : AbstractFactory {
	public override void DoSomething() {
		//do an Android thing
		Debug.Log("Do the Android hussle");
	}
}
public class DummyImplementation : AbstractFactory {
	public override void DoSomething() {
		//do nothing
		Debug.Log("Don't do anything");
	}
}