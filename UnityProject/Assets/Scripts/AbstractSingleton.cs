using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class communicates the desire to always have singletons that can be "new()"'d, which excludes Components or MonoBehaviours
/// And it's a way to prevent having to re-do your singleton code constantly...
/// </summary>
/// <typeparam name="T"></typeparam>
public abstract class AbstractSingleton<T> where T : new() {
	private static T instance;
	protected static T Singleton {
		get {
			if ( instance == null ) {
				instance = new T();
			}
			return instance;
		}
	}
}
