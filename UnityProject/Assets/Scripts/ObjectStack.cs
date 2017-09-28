using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KGDEV1 {
	public class ObjectStack<T> where T : Object {
		List<T> list;
		
		//optional parameter size
		public ObjectStack(/*int variabeleA,*/ int size = 64 ) {//, int geenVaste = 10) {
			list = new List<T>(size);
		}

		public void Push(T obj) {
			list.Add(obj);
		}

		public T Pop() {
			T ret = Peek();
			list.RemoveAt(list.Count - 1);
			return ret;
		}

		public T Peek() {
			return list[list.Count - 1];
		}

		public void Clear() {
			list.Clear();
		}

		//overloaded version of clear to get the rest of the stack
		//out parameter
		public void Clear( out T[] remainder ) {
			remainder = list.ToArray();
			list.Clear();
		}
	}

	public static class StackUseExample {
		public static void DoExample() {
			ObjectStack<GameObject> myStack = new ObjectStack<GameObject>();

			myStack.Push(new GameObject("1"));
			myStack.Push(new GameObject("2"));
			myStack.Push(new GameObject("3"));
			myStack.Push(new GameObject("4"));

			//returns and removes the last element
			GameObject obj = myStack.Pop();
			Debug.Log(obj.name);	//4

			//peek doesn't remove the element
			obj = myStack.Peek();
			//GameObject.Destroy(obj);
			Debug.Log(obj.name);	//3
			obj = myStack.Pop();
			//so this is the same one
			Debug.Log(obj.name);    //3
			GameObject.Destroy(obj);

			GameObject[] remainder;
			//will put 1 & 2 into remainder
			myStack.Clear( out remainder );

			//the game objects still exist though! they are not destroyed
			foreach (GameObject g in remainder) {
				Debug.Log(g.name);
				GameObject.Destroy(g);
			}
		}
	}
}