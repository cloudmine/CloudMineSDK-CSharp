using System;
using System.Dynamic;
using System.Net;
using System.Threading.Tasks;
using CloudmineSDK.Model;
using CloudMineSDK.Model.Responses;
using CloudMineSDK.Services;
using NUnit.Framework;

namespace CloudMineSDKNUnitTests
{
	[TestFixture]
	public class CMAppObjDeleteTests
	{
		private const string appID = "de45fca60db7402ab15159655581e96c";
		private const string apiKey = "856d34ac32344a0780a022a5bd3c22d6";
		IAppObjectService appObjSrvc { get; set; }

		[SetUp]
		public void Setup ()
		{
			CMApplication app = new CMApplication (appID, apiKey);
			IRestWrapper api = new PCLRestWrapper ();
			appObjSrvc = new CMAppObjectService (app, api);
		}

		[Test]
		public void SetObjectAndDeleteWithKey ()
		{
			dynamic hcp = new ExpandoObject ();

			hcp.__id__ = Guid.NewGuid().ToString();
			hcp.__class__ = "HCPDynamic";
			hcp.ProviderName = "CloudMine Data Hospital 2";
			hcp.ProviderAddress = "1217 Sansom Street | Suite 600, Philadelphia, PA 19107";
			hcp.ProviderEmployeeCount = 25;

			Task<CMObjectResponse> objResponse = appObjSrvc.SetObject (hcp, null, hcp.__id__);
			objResponse.Wait ();

			Assert.AreEqual (objResponse.Result.Status, HttpStatusCode.OK);
			Assert.That (objResponse.Result.HasErrors, Is.False);
			Assert.That (objResponse.Result.Success.ContainsKey(hcp.__id__), Is.True);

			Task<CMObjectResponse> delResponse = appObjSrvc.DeleteObject (hcp.__id__);
			delResponse.Wait ();

			Assert.AreEqual (delResponse.Result.Status, HttpStatusCode.OK);
			Assert.That (delResponse.Result.HasErrors, Is.False);
			Assert.That (delResponse.Result.Success.ContainsKey(hcp.__id__), Is.True);
			Assert.AreEqual (delResponse.Result.Success [hcp.__id__], "deleted");
		}

		[Test]
		public void SetMultipleObjectsAndDeleteWithKey ()
		{
			dynamic hcp = new ExpandoObject ();

			hcp.__id__ = Guid.NewGuid().ToString();
			hcp.__class__ = "HCPDynamic";
			hcp.ProviderName = "CloudMine Data Hospital 2";
			hcp.ProviderAddress = "1217 Sansom Street | Suite 600, Philadelphia, PA 19107";
			hcp.ProviderEmployeeCount = 25;

			Task<CMObjectResponse> objResponse = appObjSrvc.SetObject (hcp, null, hcp.__id__);
			objResponse.Wait ();

			Assert.AreEqual (objResponse.Result.Status, HttpStatusCode.OK);
			Assert.That (objResponse.Result.HasErrors, Is.False);
			Assert.That (objResponse.Result.Success.ContainsKey(hcp.__id__), Is.True);

			Task<CMObjectResponse> delResponse = appObjSrvc.DeleteObject (hcp.__id__);
			delResponse.Wait ();

			Assert.AreEqual (delResponse.Result.Status, HttpStatusCode.OK);
			Assert.That (delResponse.Result.HasErrors, Is.False);
			Assert.That (delResponse.Result.Success.ContainsKey(hcp.__id__), Is.True);
			Assert.AreEqual (delResponse.Result.Success [hcp.__id__], "deleted");
		}

		[Test]
		public void CanHandleMixedDeleteResults ()
		{
			dynamic hcp = new ExpandoObject ();

			hcp.__id__ = Guid.NewGuid().ToString();
			hcp.__class__ = "HCPDynamic";
			hcp.ProviderName = "CloudMine Data Hospital 2";
			hcp.ProviderAddress = "1217 Sansom Street | Suite 600, Philadelphia, PA 19107";
			hcp.ProviderEmployeeCount = 25;

			Task<CMObjectResponse> objResponse = appObjSrvc.SetObject (hcp, null, hcp.__id__);
			objResponse.Wait ();

			Assert.AreEqual (objResponse.Result.Status, HttpStatusCode.OK);
			Assert.That (objResponse.Result.HasErrors, Is.False);
			Assert.That (objResponse.Result.Success.ContainsKey(hcp.__id__), Is.True);

			// add in an ID that doesn't exist to ensure it comes back from the server as an error and parsed accordingly
			Task<CMObjectResponse> delResponse = appObjSrvc.DeleteObjects (new string[] {hcp.__id__, "NotARealID"});
			delResponse.Wait ();

			Assert.AreEqual (delResponse.Result.Status, HttpStatusCode.OK);
			Assert.That (delResponse.Result.HasErrors, Is.True);
			Assert.That (delResponse.Result.Success.ContainsKey(hcp.__id__), Is.True);
			Assert.AreEqual (delResponse.Result.Success [hcp.__id__], "deleted");
			Assert.AreEqual (delResponse.Result.Errors ["NotARealID"], "key does not exist");
		}

		[Test]
		public void DeleteAll ()
		{
			// Ensure that the API key you are using has the Delete All feature toggled in the dashboard
			Task<CMObjectResponse> delAllResponse = appObjSrvc.DeleteAllObjects ();
			delAllResponse.Wait ();

			Assert.AreEqual (delAllResponse.Result.Status, HttpStatusCode.OK);
		}
	}
}

