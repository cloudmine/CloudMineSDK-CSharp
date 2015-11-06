using System;
using System.IO;
using System.Net;
using System.Text;
using CloudmineSDK.Model;
using CloudMineSDK.Model.Responses;
using CloudMineSDK.Services;
using NetSDKTests.Mocks;
using NUnit.Framework;

namespace NetSDKTests
{
	/// <summary>
	/// TODO: Deserialization is started but still need to do serialization
	/// </summary>

	[TestFixture()]
	public class CMDeserializeObjectResponseTests
	{
		private const string cmObjectCreateResponse =
			"{ \"success\": { \"key1\": \"updated\", \"key2\": \"created\", \"key3\": \"created\" }, \"errors\": {} }";

		[Test()]
		public void CanDeserializeEmptyErrors()
		{
			using(Stream data = new MemoryStream(Encoding.UTF8.GetBytes(cmObjectCreateResponse)))
			{
				CMObjectResponse response = new CMObjectResponse(new CMResponse(HttpStatusCode.Accepted, data));

				Assert.AreEqual(response.Status, HttpStatusCode.Accepted);
				Assert.AreEqual(response.Errors.Count, 0);
			};
		}

		[Test()]
		public void CanDeserializeCMObjectCreateResponse()
		{
			using (Stream data = new MemoryStream(Encoding.UTF8.GetBytes(cmObjectCreateResponse)))
			{
				CMObjectResponse response = new CMObjectResponse(new CMResponse(HttpStatusCode.Accepted, data));

				Assert.AreEqual(response.Status, HttpStatusCode.Accepted);
				Assert.AreEqual(response.Errors.Count, 0);
			};
		}

		private const string cmObjectDeleteResponse =
			"{ \"success\": { \"key1\": \"deleted\", \"key2\": \"deleted\", \"key3\": \"deleted\" }, \"errors\": { \"key4\": \"not found\", \"key5\": \"permission denied\" } }";
		
		/// <summary>
		/// Checks that a returned stream from a list of objects to be deleted can read and deserialize both success and error messages
		/// </summary>
		[Test()]
		public void CanDeserializeCMObjectDeleteResponse()
		{
			using (Stream data = new MemoryStream(Encoding.UTF8.GetBytes(cmObjectDeleteResponse)))
			{
				CMObjectResponse response = new CMObjectResponse(new CMResponse(HttpStatusCode.Accepted, data));

				Assert.AreEqual(response.Status, HttpStatusCode.Accepted);
				Assert.AreEqual(response.HasErrors, true);
				Assert.AreEqual(response.Errors.Count, 2);
			};
		}

		private const string cmObjectFetchResponse =
			@"{
				""success"": {
					""key1"": {
						""field"":""value"",
						""field1"":""value1"",
						""field2"":""value2""
					}, 
					""key2"": { 
						""__id__"":""key1"",
						""__class__"":""mock"", 
						""field1"": ""value1"" 
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
		public void CanDeserializeCMObjectFetchResponse()
		{
			using (Stream data = new MemoryStream(Encoding.UTF8.GetBytes(cmObjectFetchResponse)))
			{
				CMObjectFetchResponse<CMOBjectMock> response = 
					new CMObjectFetchResponse<CMOBjectMock>(new CMResponse(HttpStatusCode.Accepted, data));

				Assert.AreEqual(response.Status, HttpStatusCode.Accepted);
				Assert.AreEqual(response.HasErrors, true);
				Assert.AreEqual(response.Errors.Count, 1);
			};
		}

		private const string cmObjectErrorArrayResponse = 
			@"{
				""errors"": [
					""Unauthorized""
				]
			}";

		/// <summary>
		/// Sometimes when a user object operation happens there is a user like response
		/// where errors are in an array rather than the typical error respose dictionary.
		/// Errors like this should be morphed into a dictionary so the expectation is 
		/// the same as other object and file responses.
		/// </summary>
		[Test()]
		public void CanDeserializeCMObjectUnauthorizedResponse()
		{
			using (Stream data = new MemoryStream(Encoding.UTF8.GetBytes(cmObjectErrorArrayResponse)))
			{
				CMObjectFetchResponse<CMOBjectMock> response =
					new CMObjectFetchResponse<CMOBjectMock>(new CMResponse(HttpStatusCode.Unauthorized, data));

				Assert.AreEqual(response.Status, HttpStatusCode.Unauthorized);
				Assert.AreEqual(response.HasErrors, true);
				Assert.AreEqual(response.Errors.Count, 1);
				Assert.AreEqual(response.Errors["Unauthorized"], "Unauthorized");
			};
		}


		private const string cmObjectSearchResponse = "";
		//[Test()]
		//public void CanDeserializeCMObjectSearchResponse()
		//{
		//}


		private const string cmObjectUpdateResponse = "";
		//[Test()]
		//public void CanDeserializeCMObjectUpdateResponse()
		//{
		//}

		private const string cmObjectLocationResponse = "";
		//[Test()]
		//public void CanDeserializeCMObjectLocationResponse()
		//{
		//}
	}
}
