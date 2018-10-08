using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HKU.KGDEV1.End {
	public class Bullet : MonoBehaviour, IPoolable {
		//I made a separate region for IPoolable to keep it together clearly
		#region IPoolable
		public event PoolEvent onShouldDeactivate;
		public void Activate() {
			gameObject.SetActive(true);		//simply turn it on
		}

		public void Deactivate() {
			gameObject.SetActive(false);	//simply turn it off
		}
		#endregion

		private const float SPEED = 10;
		private const float MAX_LIFETIME = 3;
		private Transform mTransform;
		private float fireTime = 0;

		public void Fired( Vector3 position, Vector3 forward ) {
			//set our velocity, etc.
			//this is called from the gun, that knows which way it is pointing (responsibilities!)
			mTransform.position = position;
			mTransform.forward = forward;
			fireTime = Time.time;
		}

		private void Awake() {
			//again, store transform to prevent frame-lookups
			mTransform = transform;
		}

		private void Update() {
			//Move forward (we're already pointing in the right direction)
			mTransform.Translate(Vector3.forward * Time.deltaTime * SPEED, Space.Self);
			//Optional: add some kind of timer to disable the bullet automatically
			// 				(don't destroy it if you want it pooled)
			if ( Time.time - fireTime > MAX_LIFETIME ) {
				if ( onShouldDeactivate != null ) {
					onShouldDeactivate(this);
				}
			}
		}
	}
}