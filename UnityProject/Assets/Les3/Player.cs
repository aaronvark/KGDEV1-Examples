using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DamageExample {
	public class Player : MonoBehaviour {
		public Gun Weapon {
			get;
			private set;
		}

		private void Awake() {
			Weapon = new Gun();
		}

		private void Update() {
			HandleWeapons();
		}

		protected void HandleWeapons() {
			//if pressed fire button
			if ( Input.GetMouseButton(0) ) {
				//Fire gun
				//Debug.Log("FIRE!");
				Weapon.Fire(transform.position, transform.forward);
			}
		}
	}
}