using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HKU.KGDEV1.End {

	public class Gun : MonoBehaviour {
		private const int NUM_OBJECTS = 100;	//this could be part of the pool as well
		private const float REFIRE_TIME = .1f;
		
		//Guns know what they're firing
		// also applies when we build a larger game with ammo
		//  then the gun can "ask" the ammo (or it receives ammo), what to fire
		public GameObject bulletPrefab;
		private float lastFireTime = 0;
		private Transform mTransform;

		//This is the easiest way to share the same pool between guns
		// you just have to make sure not to create it more than once
		private static ObjectPool<Bullet> bulletPool;

		private void Awake() {
			//Tip: don't call transform constantly (slow)
			//Instead: Make sure to set mTransform during Awake
			// - not Start, which may not happen for disabled objects
			// - Awake is called when spawning an object, even when it is disabled (on the prefab)
			mTransform = transform;
		}

		// Use this for initialization
		private void Start() {
			//Can't use a gun without bullets
			if (bulletPrefab == null) {
				Debug.LogError("No bullet prefab assigned.");
				enabled = false;
				return;
			}

			//Create object pool
			//We can do this with new() because the ObjectPool is not a MonoBehaviour
			// which also makes it easier to use the class in different contexts
			if ( bulletPool == null ) {
				bulletPool = new ObjectPool<Bullet>(bulletPrefab, NUM_OBJECTS);
			}
		}

		// Update is called once per frame (you should really know this by now)
		private void LateUpdate() {
			//Get button, fire
			if ( Input.GetMouseButton(0)) {
				Fire();
			}
		}

		private void Fire() {
			//Since Time.time keeps going up, left-hand side will increase over time
			// this means we don't need to keep count of time at all
			if (Time.time - lastFireTime >= REFIRE_TIME) {
				//Get bullet from object pool
				Bullet bullet = bulletPool.GetObject();
				//Fire it in the right direction
				bullet.Fired(mTransform.position, mTransform.forward);

				//Register that we fired, at a specific time
				lastFireTime = Time.time;
			}
		}
	}
}
