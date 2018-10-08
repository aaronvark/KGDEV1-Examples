using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Top Down Shooter Controls (for orthographic top down)
/// WSAD + Look-At-Mouse
/// </summary>
public class TDSMovement : MonoBehaviour {
	const float SPEED = 5f;
	static Vector3 viewportCenter = new Vector3(.5f, .5f, 0);

	Camera mainCam;
	Transform mTransform;

	// Use this for initialization
	void Start() {
		mainCam = Camera.main;
		mTransform = transform;
	}

	// Update is called once per frame
	void Update() {
		//get main camera ray at mouse, get origin at our own height, and look at it
		Ray camRay = mainCam.ScreenPointToRay(Input.mousePosition);
		Vector3 lookAt = camRay.origin;
		lookAt.y = mTransform.position.y;
		mTransform.LookAt(lookAt);

		//if you want to walk "forward", just use Space.Self here (which allows for awesome circle-strafing)
		Vector3 movement = Vector3.zero;
		movement.x = Input.GetAxis("Horizontal");
		movement.z = Input.GetAxis("Vertical");
		mTransform.Translate(movement * SPEED * Time.deltaTime, Space.World);
	}
}
