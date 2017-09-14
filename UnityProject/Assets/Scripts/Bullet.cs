using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {
	const float SPEED = 10;
	const float MAX_LIFETIME = 3;

	public Transform mTransform;
	float fireTime = 0;

	void Awake() {
		mTransform = transform;
	}

	public void Fired( Vector3 position, Vector3 forward ) {
		//set our velocity, etc.
		mTransform.position = position;
		mTransform.forward = forward;
		fireTime = Time.time;
	}

	void Update() {
		mTransform.Translate(Vector3.forward * Time.deltaTime * SPEED, Space.Self);
		//optional: add some kind of timer to disable the bullet automatically (don't destroy it!)
		if ( Time.time - fireTime > MAX_LIFETIME ) {
			gameObject.SetActive(false);
		}
	}
}
