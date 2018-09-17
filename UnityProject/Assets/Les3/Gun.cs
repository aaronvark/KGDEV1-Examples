using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DamageExample {
	public class Gun : IDamager {
		private float refireTime = 1f;
		private float lastTimeFired = 0;

		public DamageInfo DmgInfo {
			get;
			private set;
		}

		public Gun() {
			DmgInfo = new DamageInfo(1);
			//so we can instantly fire
			lastTimeFired = Time.time - refireTime;
		}
		
		//could also think of making a distinction between primary and secondary fire functions
		public void Fire( Vector3 position, Vector3 direction ) {
			//check for "can we fire this gun" (reload, refiretime) here
			if ( Time.time - lastTimeFired < refireTime ) return;

			//check line of aim for physics objects
			RaycastHit hitInfo;
			//TODO: figure out how to get the position of the gun
			if ( Physics.Raycast(position, direction, out hitInfo)) {
				IDamageable thingHit = hitInfo.collider.GetComponent<IDamageable>();
				if ( thingHit != null ) {
					if ( thingHit.TakeDamage(this) ) {
						//register kill-stats, points, combo-multipliers, whatever!
						Debug.Log("You destroyed: "+hitInfo.collider.name);
					}
				}
			}

			//register that we fired
			lastTimeFired = Time.time;
		}
	}
}