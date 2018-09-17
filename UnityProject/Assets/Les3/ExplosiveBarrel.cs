using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DamageExample {
	public class ExplosiveBarrel : Entity, IDamager {
		public const float RANGE = 5f;

		//single collider array used by all barrels to test physics overlaps
		//Note: do NOT use to store data across frames
		//should usually be static 
		// I removed this because of delayed handling with the IEnumerator
		Collider[] colliders = new Collider[64];

		///<summary>Again the issue with this is default value implementation (now null?)</summary>
		public DamageInfo DmgInfo {
			get;
			private set;
		}

		protected override void Start() {
			DmgInfo = new DamageInfo(10);
			health = 1;
		}

		protected override void Die() {
			//explode and hit things within RANGE meter
			//"1 << layerInt" creates an int-mask with only the bit for the specified layer set to 1, so we only target one layer
			/*
			int count = Physics.OverlapSphereNonAlloc(transform.position, RANGE, colliders, 1 << LayerMask.NameToLayer("Damageable") );
			for( int i = 0; i < count; ++i ) {
				IDamageable hit = colliders[i].GetComponent<IDamageable>();
				if ( hit.TakeDamage(this) ) {
					Debug.Log( name+" destroyed "+colliders[i].name );
				}
			}

			base.Die();
			*/
			
			//Co-routine as a delayed example
			StartCoroutine(ExplosionAsync());
		}

		IEnumerator ExplosionAsync() {
			int count = Physics.OverlapSphereNonAlloc(transform.position, RANGE, colliders, 1 << LayerMask.NameToLayer("Damageable") );
			for( int i = 0; i < count; ++i ) {
				IDamageable hit = colliders[i].GetComponent<IDamageable>();
				if ( hit.TakeDamage(this) ) {
					yield return new WaitForSeconds(1f);
					Debug.Log( name+" destroyed "+colliders[i].name );
				}
			}

			base.Die();

			yield return null;
		}

		protected override void OnDrawGizmos() {
			Gizmos.color = Color.red;
			Gizmos.DrawWireSphere(transform.position, RANGE);
			base.OnDrawGizmos();
		}
	}
}