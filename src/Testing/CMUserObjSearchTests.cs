using System;
using CloudMineSDK.Services;
using System.Threading.Tasks;
using CloudMineSDKNUnitTests;
using CloudMineSDK.Model.Responses;
using NUnit.Framework;
using CloudmineSDK.Model;
using CloudMineSDKNUnitTests.Mocks;
using NetSDKPrivate.Model.Responses;
using System.Net;

namespace CloudMineSDKNUnit
{
	public class CMUserObjSearchTests
	{
		private const string appID = "de45fca60db7402ab15159655581e96c";
		private const string apiKey = "856d34ac32344a0780a022a5bd3c22d6";
		IUserService userService { get; set; }

		CMUser<CMUserProfileMock> user = new CMUser<CMUserProfileMock> (
			"test2", 
			"test2@cloudmine.me", 
			"testpass", 
			new CMUserProfileMock () {
				FavoriteCafe = "CloudMine Coffee to Go"
			});

		[SetUp]
		public void Setup ()
		{
			CMApplication app = new CMApplication (appID, apiKey);
			IRestWrapper api = new PCLRestWrapper ();
			userService = new CMUserService (app, api);
		}

		[Test]
		public void SearchObjectByClassAndCount ()
		{
			Task<CMUserResponse<CMUserProfileMock>> loginResponse = userService.Login<CMUserProfileMock> (user);
			loginResponse.Wait ();

			// ensure the login was successful
			Assert.AreEqual (loginResponse.Result.Status, HttpStatusCode.OK);
			Assert.That (loginResponse.Result.HasErrors, Is.False);
			Assert.AreEqual (loginResponse.Result.CMUser.Session, user.Session); // ensure session set

			// lets create a user object
			PIIMock pii = new PIIMock () {
				FirstName = "Jane",
				LastName = "Doe",
				SocialSecurityNumber = "333-22-4444"
			};

			Task<CMObjectResponse> objResponse = userService.SetUserObject<PIIMock> (user, pii);
			objResponse.Wait ();

			Assert.AreEqual (objResponse.Result.Status, HttpStatusCode.OK);
			Assert.That (objResponse.Result.HasErrors, Is.False);
			Assert.That (objResponse.Result.Success.ContainsKey(pii.ID), Is.True);

			// get by id
			Task<CMObjectFetchResponse<PIIMock>> getResponse = userService.GetUserObject<PIIMock> (user, pii.ID);
			getResponse.Wait ();

			Assert.AreEqual (getResponse.Result.Status, HttpStatusCode.OK);

			CMRequestOptions opts = new CMRequestOptions () {
				Parameters = new System.Collections.Generic.Dictionary<string, string> () {
					{ "shared", "true" },
					{ "shared_only", "false" }
				}
			};

			// search
			Task<CMObjectSearchResponse<PIIMock>> searchResponse = userService.SearchUserObjects<PIIMock> (user, @"[__class__=""PIIMock"", FirstName=""Jane""]", opts);
			searchResponse.Wait ();

			Assert.AreEqual (searchResponse.Result.Status, HttpStatusCode.OK);
			Assert.That (searchResponse.Result.HasErrors, Is.False);
			Assert.That (searchResponse.Result.Success.ContainsKey (pii.ID), Is.True);
			Assert.AreEqual (searchResponse.Result.Success [pii.ID].GetType (), typeof(PIIMock));
		}
	}
}

