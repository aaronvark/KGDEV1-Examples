using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HKU.KGDEV1.End {
	public class Gun : MonoBehaviour {
		const int NUM_OBJECTS = 50;
		const float REFIRE_TIME = .02f;

		//guns share this bulletpool
		//TODO: find a way to share across "groups"
		// For instance: a static Dictionary<string,ObjectPool<Bullet>>
		// Biggest problem here is that static variables will persist, so we need to dispose it manually!
		static ObjectPool<Bullet> bulletPool;

		public GameObject bulletPrefab;

		float lastFireTime = 0;
		Transform mTransform;

		void Awake() {
			//don't call transform constantly (slow)
			//make sure to set mTransform during Awake (not Start, which may not happen for disabled objects)
			mTransform = transform;

			//can't use a gun without bullets
			if (bulletPrefab == null) {
				Debug.LogError("No bullet prefab assigned.");
				enabled = false;
				return;
			}

			if (bulletPool == null) {
				bulletPool = new ObjectPool<Bullet>();
				bulletPool.Initialize(bulletPrefab, NUM_OBJECTS);
			}
		}

		// Update is called once per frame
		void LateUpdate() {
			//get button, fire
			if ( Input.GetMouseButton(0)) {
				Fire();
			}
		}

		void Fire() {
			if (Time.time - lastFireTime >= REFIRE_TIME) {
				//activate current bullet, point it in the right direction
				Bullet bullet = bulletPool.GetObject();
				bullet.Fired( mTransform.position, mTransform.forward );

				//register that we fired
				lastFireTime = Time.time;
			}
		}
	}
}