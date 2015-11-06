using System;
using System.IO;
using System.Text;
using CloudmineSDK.Model;
using CloudMineSDK.Model.Responses;
using CloudMineSDK.Services;
using CloudmineSDK.Services;
using NUnit.Framework;
using System.Net;
using CloudMineSDKNUnitTests.Mocks;

namespace NetSDKTests
{
	[TestFixture()]
	public class CMDeserializeObjectACLTests
	{

		private const string cmObjectResponseWithACL =
			@"{
				""success"": {
					""key1"": {
						""field"":""value"",
						""field1"":""value1"",
						""field2"":""value2"",
					}, 
					""key2"": { 
						""__id__"":""key1"",
						""__class__"":""mock"", 
						""field1"": ""value1"",
						""__access__"":[""2343234aswefwae"", ""waef23rf2w38fh""]
					}
				},
				""errors"": { 
					""key3"": { 
						""code"": 404, 
						""message"": ""Not Found"" 
					}
				} 
			}";

		[Test()]
		public void CanDeserializeCMObjectResponseWithACL()
		{
			using (Stream data = new MemoryStream(Encoding.UTF8.GetBytes(cmObjectResponseWithACL)))
			{
				CMObjectFetchResponse<CMOBjectMock> response =
					new CMObjectFetchResponse<CMOBjectMock>(new CMResponse(HttpStatusCode.Accepted, data));

				Assert.AreEqual(response.Success["key2"].AccessListIDs.Length, 2);
				Assert.AreEqual(response.Success["key2"].AccessListIDs[0], "2343234aswefwae");
				Assert.AreEqual(response.Success["key2"].AccessListIDs[1], "waef23rf2w38fh");
				Assert.AreEqual(response.Success["key1"].AccessListIDs, null);
				Assert.AreEqual(response.Status, HttpStatusCode.Accepted);
				Assert.AreEqual(response.HasErrors, true);
				Assert.AreEqual(response.Errors.Count, 1);
			};
		}
	}
}
