using System;
using CloudMineSDK.Model;
using System.Threading.Tasks;
using CloudmineSDK.Model;
using CloudMineSDK.Services;
using NUnit.Framework;
using CloudMineSDKNUnitTests.Mocks;
using CloudMineSDKNUnitTests;
using System.Net;
using NetSDKPrivate.Model.Responses;

namespace CloudMineSDKNUnit
{
	public class CMAccessListServiceTests
	{
		private const string appID = "de45fca60db7402ab15159655581e96c";
		private const string apiKey = "856d34ac32344a0780a022a5bd3c22d6";
		IUserService userService { get; set; }
		CMApplication app { get; set; }
		IRestWrapper api { get; set; }

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
			app = new CMApplication (appID, apiKey);
			api = new PCLRestWrapper ();
			userService = new CMUserService (app, api);
		}

		[Test]
		public void CanCreateAccessList ()
		{
			Task<CMUserResponse<CMUserProfileMock>> loginResponse = userService.Login<CMUserProfileMock> (user);
			loginResponse.Wait ();

			Assert.AreEqual (loginResponse.Result.Status, HttpStatusCode.OK);
			Assert.That (loginResponse.Result.HasErrors, Is.False);
			Assert.AreEqual (loginResponse.Result.CMUser.Profile.GetType (), typeof(CMUserProfileMock)); 
			Assert.AreEqual (loginResponse.Result.CMUser.Profile.FavoriteCafe, "CloudMine Coffee to Go");
			Assert.AreEqual (loginResponse.Result.CMUser.Session, user.Session); // ensure session set

			var acl = new CMAccessList () {
				Permissions = new CMAccessListPermission[] {
					CMAccessListPermission.r, 
					CMAccessListPermission.u
				},
				Members =  new string[] { user.UserID }
			};

			IAccessListService accessService = new AccessListService (app, api);
			Task<CMResponse> createAccessListTask = accessService.CreateAccessList (user, acl);
			createAccessListTask.Wait ();

			Assert.AreEqual (createAccessListTask.Result.Status, HttpStatusCode.OK);


			acl.Permissions = new CMAccessListPermission[] {
				CMAccessListPermission.r
			};

			Task<CMResponse> modifyAccessListTask = accessService.ModifyAccessList (user, acl);
			modifyAccessListTask.Wait ();

			Assert.AreEqual (modifyAccessListTask.Result.Status, HttpStatusCode.OK);


			HCPMock obj = new HCPMock () {
				AccessListIDs = new string[] { acl.ID }
			};
		}
	}
}

