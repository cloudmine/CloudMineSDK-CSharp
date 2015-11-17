using System;
using CloudmineSDK.Model;
using CloudMineSDK.Services;
using NUnit.Framework;
using CloudMineSDKNUnitTests.Mocks;
using NetSDKPrivate.Model.Responses;
using System.Threading.Tasks;
using System.Net;
using CloudMineSDK.Model;

namespace NetSDKTests
{
	[TestFixture()]
	public class CMPushNotificationTests
	{
		private const string appID = "de45fca60db7402ab15159655581e96c";
		private const string apiKey = "856d34ac32344a0780a022a5bd3c22d6";
		const string deviceToken = "<740f4707 bebcf74f 9b7c25d4 8e335894 5f6aa01d a5ddb387 462c7eaf 61bb78ad>";

		IPushNotificationService pushService { get; set; }
		IUserService userService { get; set; }
		CMUser<CMUserProfileMock> user = new CMUser<CMUserProfileMock> (
			"test2", 
			"test2@cloudmine.me", 
			"testpass", 
			new CMUserProfileMock () {
				FavoriteCafe = "CloudMine Coffee to Go"
			});

		[SetUp()]
		public void SetUp () {
			CMApplication app = new CMApplication (appID, apiKey);
			IRestWrapper api = new PCLRestWrapper ();
			pushService = new CMPushNotificationService (app, api);
		}

		[Test()]
		public void CanStripDeviceToken ()
		{
			string expectedToken = "740f4707bebcf74f9b7c25d48e3358945f6aa01da5ddb387462c7eaf61bb78ad";
			string strippedToken = pushService.StripIOSDeviceToken (deviceToken);

			Assert.AreNotEqual (deviceToken, expectedToken);
			Assert.AreEqual(strippedToken, expectedToken);
		}

		[Test()]
		public void CanPublishDeviceToken_iOS ()
		{
			string token = "740f4707bebcf74f9b7c25d48e3358945f6aa01da5ddb387462c7eaf61bb78ad";

			Task<CMUserResponse<CMUserProfileMock>> loginResponse = userService.Login<CMUserProfileMock> (user);
			loginResponse.Wait ();

			Assert.AreEqual (loginResponse.Result.Status, HttpStatusCode.OK);
			Assert.That (loginResponse.Result.HasErrors, Is.False);
			Assert.AreEqual (loginResponse.Result.CMUser.Profile.GetType (), typeof(CMUserProfileMock)); 
			Assert.AreEqual (loginResponse.Result.CMUser.Profile.FavoriteCafe, "CloudMine Coffee to Go");
			Assert.AreEqual (loginResponse.Result.CMUser.Session, user.Session); // ensure session set

			// register a token with the server
			Task<CMResponse> pushRegisterResponse = pushService.RegisterIOSDevicePushNotifications (user, token);
			pushRegisterResponse.Wait ();

			Assert.AreEqual (loginResponse.Result.Status, HttpStatusCode.OK);
		}

		[Test()]
		public void CanPublishDeviceToken_Android ()
		{
			string token = "740f4707bebcf74f9b7c25d48e3358945f6aa01da5ddb387462c7eaf61bb78ad";

			Task<CMUserResponse<CMUserProfileMock>> loginResponse = userService.Login<CMUserProfileMock> (user);
			loginResponse.Wait ();

			Assert.AreEqual (loginResponse.Result.Status, HttpStatusCode.OK);
			Assert.That (loginResponse.Result.HasErrors, Is.False);
			Assert.AreEqual (loginResponse.Result.CMUser.Profile.GetType (), typeof(CMUserProfileMock)); 
			Assert.AreEqual (loginResponse.Result.CMUser.Profile.FavoriteCafe, "CloudMine Coffee to Go");
			Assert.AreEqual (loginResponse.Result.CMUser.Session, user.Session); // ensure session set

			// register a token with the server
			Task<CMResponse> pushRegisterResponse = pushService.RegisterAndroidDevicePushNotifications (user, token);
			pushRegisterResponse.Wait ();

			Assert.AreEqual (loginResponse.Result.Status, HttpStatusCode.OK);
		}
	}
}