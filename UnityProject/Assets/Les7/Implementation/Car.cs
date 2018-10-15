using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MVCExample {
	public class Car : AVehicleView {
		private void Awake() {
			Stats = new VehicleStats(1f, 2f);

			//TODO: Add visual features for pedestrian through composition
			MeshFilter filter = gameObject.AddComponent<MeshFilter>();
			MeshRenderer renderer = gameObject.AddComponent<MeshRenderer>();

			renderer.material = new Material(Shader.Find("Standard"));
			
			Mesh m = new Mesh();
			m.vertices = new Vector3[] { 
											new Vector3( 2, 0, 2 ),
											new Vector3( -2, 0, 2 ),
											new Vector3( 0, 0, -2 )  
										};
			m.triangles = new int[] { 0,2,1 };
			m.RecalculateTangents();
			filter.mesh = m;

			Collider c = gameObject.AddComponent<BoxCollider>();
			c.isTrigger = true;
			gameObject.layer = LayerMask.NameToLayer("Vehicles");
		}

		public override void HandleInput(float horizontal, float vertical, List<GameAction> actions) {
			base.HandleInput(horizontal, vertical, actions);
			
			foreach( GameAction act in actions ) {
				switch( act ) {
					case GameAction.USE:
						//get out of the damn car!
						IVehicleView ped = VehicleManager.GetDefaultView(transform.position);
						ped.SetController(ActiveController);
						SetController(null);
					break;
				}
			}
		}

		public override void StartControl() {}
		public override void EndControl() {}
	}
}