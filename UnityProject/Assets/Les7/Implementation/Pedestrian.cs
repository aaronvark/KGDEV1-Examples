using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MVCExample {
	public class Pedestrian : AVehicleView {
		private void Awake() {
			Stats = new VehicleStats(.1f, 1f);

			//TODO: Add visual features for pedestrian through composition
			//Ideally this would be defined in the model, especially if it is shared between different types of views (like these meshes are!)
			MeshFilter filter = gameObject.AddComponent<MeshFilter>();
			MeshRenderer renderer = gameObject.AddComponent<MeshRenderer>();

			renderer.material = new Material(Shader.Find("Standard"));
			
			Mesh m = new Mesh();
			m.vertices = new Vector3[] { 
											new Vector3( 1, 0, 1 ) * .5f,		//topright
											new Vector3( -1, 0, 1 ) * .5f,		//topleft
											new Vector3( 1, 0, -1 ) * .5f,		//bottomright
											new Vector3( -1, 0, -1 ) * .5f,		//bottomleft
										};
			m.triangles = new int[] { 
										0, 2, 1, 
										1, 2, 3	
									};
			m.RecalculateTangents();
			filter.mesh = m;
		}

		public override void HandleInput(float horizontal, float vertical, List<GameAction> actions) {
			base.HandleInput(horizontal, vertical, actions);

			foreach( GameAction act in actions ) {
				switch( act ) {
					case GameAction.USE:
						//see if we can interact with another IVehicleView nearby
						Collider[] vehicles = Physics.OverlapSphere(transform.position, Stats.size, 1 << LayerMask.NameToLayer("Vehicles") );
						if ( vehicles.Length > 0 ) {
							//just respond to the first one for now
							IVehicleView view = vehicles[0].GetComponent<IVehicleView>();
							//Debug.Log( "Should never be null in this case: "+ActiveController );
							view.SetController(ActiveController);
							SetController(null);
						}
					break;
				}
			}
		}

		public override void StartControl() {

		}

		public override void EndControl() {
			Destroy(gameObject);
		}
	}
}