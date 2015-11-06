using System;
using NUnit.Framework;
using CloudMineSDK.Services;
using CloudmineSDK.Model;
using CloudmineSDK.Services;
using System.Collections.Generic;
using Newtonsoft.Json;
using CloudMineSDKNUnitTests.Mocks;

namespace NetSDKTests
{
	[TestFixture()]
	public class CMSerializeObjectACLTests
	{
		string expectedSerializedValue =
		"{\"field1\":\"value1\",\"__id__\":\"key1\",\"__class__\":\"CMOBjectMock\",\"__access__\":[\"2343234aswefwae\",\"waef23rf2w38fh\"]}";

		CMOBjectMock mockWithACL = new CMOBjectMock()
		{
			ID = "key1",
			field1 = "value1",
			AccessListIDs = new string[] { "2343234aswefwae", "waef23rf2w38fh" }
		};

		[Test()]
		public void CanSerializeCMObjectWithACL()
		{
			// only set the type if not already set and T is not CMobject
			if (string.IsNullOrEmpty(mockWithACL.Class) && typeof(CMOBjectMock).Name != typeof(CMObject).Name)
				mockWithACL.Class = typeof(CMOBjectMock).Name;

			string serializedValue = JsonConvert.SerializeObject(mockWithACL);

			Assert.AreEqual(serializedValue, expectedSerializedValue);
		}
	}

	[TestFixture()]
	public class CMSerializeACLTests
	{
		string expectedSerializedACL = "";

		//[Test()]
		//public void CanSerializeCMAccessList
		//{
		//	//Assert.AreEqual("","");
		//}
	}

	[TestFixture()]
	public class CMDeSerializeACLTests
	{
		string expectedSerializedACL = "";

		//[Test()]
		//public void CanDeSerializeCMAccessList
		//{
		//	//Assert.AreEqual("","");
		//}
	}
}
