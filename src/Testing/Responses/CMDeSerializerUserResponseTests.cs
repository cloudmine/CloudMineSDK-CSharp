using System;
using NUnit.Framework;
using CloudMineSDK.Services;
using CloudmineSDK.Model;
using CloudmineSDK.Services;
using System.IO;
using NetSDKPrivate.Model.Responses;
using System.Text;
using System.Net;
using CloudMineSDKNUnitTests.Mocks;

namespace NetSDKTests
{
	/// <summary>
	/// TODO: Deserialization is started but still need to do serialization
	/// </summary>

	[TestFixture()]
	public class CMDeserializerUserResponseTests
	{
		private const string cmUserSuccessResponse = "{ \"__id__\": \"8bc73e529d6c418da480de7efa0971d9\", \"__type__\": \"user\",\"name\": \"Example User\",\"location\": { \"__type__\": \"geopoint\", \"longitude\": 45.5, \"latitude\": -70.2 }}";
		private const string cmUserAlreadyExistsErrorResponse = "{ \"errors\": [\"There is already an account associated with this email address\"]}";
		private const string cmUserInvalidEmailErrorResponse = "{ \"errors\": [\"Email format isn't valid\"]}";
		private const string cmUserMultipleErrorResponse = "{ \"errors\": [ \"The email for this user must be set.\", \"The password for this user must be set.\"]}";

		[Test()]
		public void CanDeserializeUserCreateResponse()
		{
			using (Stream data = new MemoryStream(Encoding.UTF8.GetBytes(cmUserSuccessResponse)))
			{
				CMUserResponse response = new CMUserResponse(new CMResponse(HttpStatusCode.Accepted, data));

				Assert.AreEqual(response.Status, HttpStatusCode.Accepted);
				Assert.AreEqual(response.Errors.Length, 0);
				// TODO: add checks for the user object to ensure it parsed
				// TODO: add checks to ensure profile parsed
			};
		}

		[Test()]
		public void CanDeserializeUserExistsErrorResponse()
		{
			using (Stream data = new MemoryStream(Encoding.UTF8.GetBytes(cmUserAlreadyExistsErrorResponse)))
			{
				CMUserResponse response = new CMUserResponse(new CMResponse(HttpStatusCode.Accepted, data));

				Assert.AreEqual(response.Status, HttpStatusCode.Accepted);
				Assert.AreEqual(response.Errors.Length, 1);
			};
		}

		[Test()]
		public void CanDeserializeUserLoginResponse()
		{
			string userLoginResponse = 
				@"{
					""session_token"":""44c31131ecac41cf92f49b28b84ebac4"",
					""expires"":""Tue, 13 Mar 2012 20:03:45 GMT"",
					""profile"": {
						""__id__"": ""0063C25CB88BB74AE6799BBFD5E4D205"",
						""favorite_cafe"": ""La Colombe Torrefaction""
					}
				}";

			using (Stream data = new MemoryStream(Encoding.UTF8.GetBytes(userLoginResponse)))
			{
				CMUserResponse<CMUserProfileMock> response = new CMUserResponse<CMUserProfileMock>(new CMResponse(HttpStatusCode.Accepted, data));

				Assert.AreEqual(response.Status, HttpStatusCode.Accepted);
				Assert.AreEqual(response.Errors.Length, 0);
				Assert.AreEqual(response.CMUser.Profile.FavoriteCafe, "La Colombe Torrefaction");
			};
		}
	}
}
