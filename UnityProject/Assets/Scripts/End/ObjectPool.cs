using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HKU.KGDEV1.End
{
	//I decided to force a Component pool, because in my experience this is the most useful.
	//People can use Transform if they just need access to the objects.
	//If they want to pool something that's not a GameObject, then this is not the class for them.
	// -> Perhaps this should be renamed to a "PrefabPool"?
	public class ObjectPool<T> where T : Component {

		//store things in a parent for a clean Hierarchy
		Transform parent;
		//I decided to just store GameObjects, and not do all sorts of difficult things with T here
		// I've seen people use logic to determine if T is a Component or a GameObject, but it gets really messy really fast
		// and you rarely need it.
		GameObject[] objects;
		//The above does mean you need a separate list of T so you don't have to GetComponent constantly (even though you could)
		T[] components;

		//management variables
		int current = 0;
		int size;

		public void Initialize(GameObject prefab, int size) {
			//create parent if it doesn't exist, and I'm using the type to make it clearly visible which pool is which
			if (parent == null) {
				parent = new GameObject("_objectPool<" + typeof(T).ToString() + ">").transform;
			}
			this.size = size;

			//spawn all the stuff (prepare arrays, instantiate objects, get components, deactivate)
			objects = new GameObject[size];
			components = new T[size]; 
			for (int i = 0; i < size; ++i) {
				objects[i] = GameObject.Instantiate(prefab);
				objects[i].transform.parent = parent;
				components[i] = objects[i].GetComponent<T>();
				objects[i].SetActive(false);
			}
		}

		//this function is extremely clean because we did two things:
		// 1. Didn't leave amiguity about what kind of things we are pooling (GameObjects)
		// 2. Forced T to be a Component, and stored this lists of components ahead of time
		public T GetObject() {
			GameObject obj = objects[current];
			T retVal = components[current];
			obj.SetActive(true);

			//cycle through our bullets, make sure to loop around (you can do this with % but this is easier to read)
			//alternative % approach: currentSize = ++currentSize % size;
			//	explanation: first increment currentSize, then % against size (will go to 0 if it reaches size), then store back in currentSize
			//	common mistake: using currentSize++ (increment AFTER evaluation), instead of ++currentSize (increment BEFORE evaluation)
			if (++current == size) {
				current = 0;
			}

			return retVal;
		}
    }
}
