using System;
using CloudMineSDK.Services;
using NUnit.Framework;
using CloudmineSDK.Model;
using CloudmineSDK.Services;
using System.Threading.Tasks;
using NetSDKPrivate.Model.Responses;
using System.Net;
using CloudMineSDK.Model.Responses;
using CloudMineSDKNUnitTests.Mocks;

namespace CloudMineSDKNUnit
{
	public class CMUserAuthTests
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
		public void CanCreateUser()
		{			
			Task<CMUserResponse> userResponse = userService.Create (user);
			userResponse.Wait ();

			// for this test the conflict means the user exists. poor unit testing
			// example but mostly here to illustrate usage of the create function
			Assert.AreEqual (userResponse.Result.Status, HttpStatusCode.Conflict);
			Assert.That (userResponse.Result.HasErrors, Is.True);
		}

		[Test]
		public void CanLoginAndLogoutUser()
		{
			Task<CMUserResponse<CMUserProfileMock>> loginResponse = userService.Login<CMUserProfileMock> (user);
			loginResponse.Wait ();

			Assert.AreEqual (loginResponse.Result.Status, HttpStatusCode.OK);
			Assert.That (loginResponse.Result.HasErrors, Is.False);
			Assert.AreEqual (loginResponse.Result.CMUser.Profile.GetType (), typeof(CMUserProfileMock)); 
			Assert.AreEqual (loginResponse.Result.CMUser.Profile.FavoriteCafe, "CloudMine Coffee to Go");
			Assert.AreEqual (loginResponse.Result.CMUser.Session, user.Session); // ensure session set

			Task<CMLogoutResponse> logoutResponse = userService.Logoff (user);
			logoutResponse.Wait ();

			Assert.AreEqual (logoutResponse.Result.Status, HttpStatusCode.OK);
			Assert.That (logoutResponse.Result.HasErrors, Is.False);
			Assert.AreEqual (user.Session, string.Empty);
			Assert.AreEqual (user.SessionExpires, DateTime.MinValue);
		}
	}
}

