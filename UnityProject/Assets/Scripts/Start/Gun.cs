using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HKU.KGDEV1.Start {
	public class Gun : MonoBehaviour {
		const int NUM_OBJECTS = 10;
		const float REFIRE_TIME = .1f;
		
		public GameObject bulletPrefab;

		Bullet[] objects;
		float lastFireTime = 0;
		int currentBullet = 0;
		Transform mTransform;

		void Awake() {
			//don't call transform constantly (slow)
			//make sure to set mTransform during Awake (not Start, which may not happen for disabled objects)
			mTransform = transform;
		}

		// Use this for initialization
		void Start() {
			//can't use a gun without bullets
			if (bulletPrefab == null) {
				Debug.LogError("No bullet prefab assigned.");
				enabled = false;
				return;
			}

			//init bullets
			objects = new Bullet[NUM_OBJECTS];
			for (int i = 0; i < NUM_OBJECTS; ++i) {
				objects[i] = Instantiate(bulletPrefab).GetComponent<Bullet>();
				objects[i].gameObject.SetActive(false);
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
				Bullet bullet = objects[currentBullet];
				bullet.gameObject.SetActive(true);
				bullet.Fired(mTransform.position, mTransform.forward);

				//cycle through our bullets, make sure to loop around
				if (++currentBullet == NUM_OBJECTS) {
					currentBullet = 0;
				}
				//register that we fired
				lastFireTime = Time.time;
			}
		}
	}
}