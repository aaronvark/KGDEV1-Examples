using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//terrible, but I need this to preview the object's name
//
//Note: I tried to split this behaviour to a Partial Class, but this doesn't work
//			because of the compile order Unity maintains (Editor first, iirc)
//		The obvious solution is to write a proper editor class for the Scene Preview
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace DamageExample {
		public class Entity : MonoBehaviour, IDamageable {
		public event Callback onDeath;
		protected int health;

		protected virtual void Start() {
			health = 1;
		}

		public bool TakeDamage( IDamager source ) {
			//already dead! (also prevents loops between explosive barrels, for instance)
			if ( health <= 0 ) return false;

			health -= source.DmgInfo.Amount;
			
			if ( health <= 0 ) {
				Die();
				return true;
			}

			return false;
		}

		protected virtual void Die() {
			//perform death
			//is anybody listening? does anybody care?
			if ( onDeath != null ) onDeath();

			//could destroy, but perhaps we would like to "reset" level without reloading
			gameObject.SetActive(false);
		}

		protected virtual void OnDrawGizmos() {
//don't do this
#if UNITY_EDITOR
			Handles.Label(transform.position + Vector3.up * 2, name );
#endif
		}
	}
}