using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HKU.KGDEV1.End {
	//This event is used to signal events about an IPoolable instance
	// - the instance here will generally always be "this" (the IPoolable sending the event) 
	public delegate void PoolEvent(IPoolable instance);

	//IPoolable interface defines how the ObjectPool will communicate with objects it manages
	public interface IPoolable {
		///<summary>Object should use this to indicate that it needs to be deactivated</summary>
		event PoolEvent onShouldDeactivate;
		///<summary>Should activate and return object to initial state, ready for use</summary>
		void Activate();
		///<summary>Should deactivate objects and clean up any resources used</summary>
		void Deactivate();
	}

	//This object pool is generic, but we have restricted the types that T can be to
	// classes that enherit from Component, and also implement IPoolable
	//
	//I've primarily done this because we want to "talk" to components (bullets, enemies, etc.)
	// even though the object pool will deal with GameObjects most of the time
	public class ObjectPool<T> where T : Component, IPoolable {
		//The only thing the object pool stores is references to T (e.g. Bullets)
		private Stack<T> items;

		//The constructor is used to create the object pool, and we need
		// the prefab to spawn, and the amount of them
		public ObjectPool( Object prefab, int size ) {
			//Giving a Stack (or Queue, List, Dictionary, etc.) a size
			// will make it more efficient (it will start at this size)
			//If you don't, it will grow a few times as you fill it
			// which basically copies the entire thing to a new array (which is a waste)
			items = new Stack<T>(size);
			GameObject g;
			for( int i = 0; i < size; ++i ) {
				//spawn the prefab
				g = GameObject.Instantiate(prefab) as GameObject;
				
				//get the component T (this works because -> "where T: Component"!)
				T obj = g.GetComponent<T>();

				//Listen to the IPoolable event
				// This makes them more loosely coupled, so we could use the IPoolable
				//  bullets without using the Object Pool
				obj.onShouldDeactivate += DeactivateObject;
				
				//Ádd the new item to the stack
				items.Push(obj);
				//Turn the GameObject off to start (otherwise we have un-used bullets floating around!)
				//Here you could also parent the objects to keep the Hierarchy "clean"
				g.SetActive(false);
			}
		}

		//Pops an item from the Stack, Activates it, and then returns that same item
		public T GetObject() {
			T retVal = items.Pop();
			retVal.Activate();
			return retVal;
		}
		
		//Deactivates when an IPoolable object indicates it should be deactivated
		//Since the ObjectPool does this, it keeps the IPoolable reasonably clean
		private void DeactivateObject( IPoolable obj ) {
			//I originally commented this because I did it twice, but the valid question remains:
			// is it more logical here, or in Push?
			//I Actually thing this one makes more sense! (so I uncommented again)
			obj.Deactivate();
			Push(obj as T);	//interesting problem
		}

		private void Push( T obj ) {
			//Doesn't make sense to do this here, given the function name
			//obj.Deactivate();
			items.Push(obj);
		}
	}
}