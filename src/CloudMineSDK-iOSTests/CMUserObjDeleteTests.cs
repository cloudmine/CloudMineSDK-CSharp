using System;
using System.Net;
using System.Threading.Tasks;
using CloudmineSDK.Model;
using CloudMineSDK.Model.Responses;
using CloudMineSDK.Services;
using CloudMineSDKNUnitTests;
using CloudMineSDKNUnitTests.Mocks;
using NetSDKPrivate.Model.Responses;
using NUnit.Framework;

namespace CloudMineSDKNUnit
{
	[TestFixture()]
	public class CMUserObjDeleteTests
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
		public void DeleteCMUserObject ()
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

			Task<CMObjectResponse> objResponse = userService.SetUserObject<PIIMock> (pii, user);
			objResponse.Wait ();

			Assert.AreEqual (objResponse.Result.Status, HttpStatusCode.OK);
			Assert.That (objResponse.Result.HasErrors, Is.False);
			Assert.That (objResponse.Result.Success.ContainsKey(pii.ID), Is.True);

			Task<CMObjectResponse> deleteResponse = userService.DeleteUserObject(pii.ID, user);
			deleteResponse.Wait ();

			Assert.AreEqual (deleteResponse.Result.Status, HttpStatusCode.OK);
			Assert.That (deleteResponse.Result.HasErrors, Is.False);
			Assert.That (deleteResponse.Result.Success.ContainsKey (pii.ID), Is.True);
		}
	}
}

