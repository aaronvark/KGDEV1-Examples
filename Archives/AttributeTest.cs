using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEditor;

namespace DataTest {

	#region base classes
	public class BaseData {

	}

	public class BaseClass {

	}
	#endregion


	#region behaviour classes
	[DataLink(typeof(AttributeData))]
	public class AttributeTest : BaseClass {
		
	}

	[DataLink(typeof(OtherData))]
	public class OtherTest  : BaseClass {

	}
	#endregion


	#region data classes
	public class AttributeData : BaseData {
		//data!
	}

	public class OtherData : BaseData {
		//data!
	}
	#endregion


	public class DataLinkAttribute : Attribute {

		public System.Type t;

		public DataLinkAttribute( System.Type t ) {
			this.t = t;
		}
	}

	[InitializeOnLoad]
	public static class Usage {
		static Usage() {
			AttributeTest someObject = new AttributeTest();
			OtherTest someObject2 = new OtherTest();

			//Debug.Log("BLA");
			DataLinkAttribute[] attributes = (DataLinkAttribute[])someObject.GetType().GetCustomAttributes(typeof(DataLinkAttribute), false);
			foreach( DataLinkAttribute dla in attributes ) {
				Debug.Log(dla.t);
			}

			attributes = (DataLinkAttribute[])someObject2.GetType().GetCustomAttributes(typeof(DataLinkAttribute), false);
			foreach( DataLinkAttribute dla in attributes ) {
				Debug.Log(dla.t);
			}
		}
	}

}