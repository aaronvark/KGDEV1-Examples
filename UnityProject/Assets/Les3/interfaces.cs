using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DamageExample {

	public delegate void Callback();

	public struct DamageInfo {
		///<summary>biggest downside to this is the default value is 0</summary>
		public int Amount {
			get;
			private set;
		}
		public DamageInfo(int amount) {
			Amount = amount;
		}
	}


	public interface IDamager {
		DamageInfo DmgInfo {
			get;
		}
	}


	public interface IDamageable {
		event Callback onDeath;
		bool TakeDamage(IDamager source);
	}
}