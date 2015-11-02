using System;
using CloudMineSDK.Services;
using NUnit.Framework;
using CloudmineSDK.Model;
using CloudMineSDKNUnitTests;
using System.Threading.Tasks;
using System.Net;
using CloudMineSDK.Model.Responses;

namespace Testing
{
	public class CMAppObjUpdateTests
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
		public void UpdateCMObject ()
		{
			HCPMock hcp = new HCPMock () {
				ProviderName = "CloudMine Data Hospital",
				ProviderAddress = "1217 Sansom Street | Suite 600, Philadelphia, PA 19107",
				ProviderEmployeeCount = 25
			};

			Task<CMObjectResponse> objResponse = appObjSrvc.SetObject<HCPMock> (hcp);
			objResponse.Wait ();

			Assert.AreEqual (objResponse.Result.Status, HttpStatusCode.OK);
			Assert.That (objResponse.Result.HasErrors, Is.False);
			Assert.That (objResponse.Result.Success.ContainsKey(hcp.ID), Is.True);

			hcp.ProviderName = "CloudMine Data Grave";
			Task<CMObjectResponse> updateResponse = appObjSrvc.UpdateObject<HCPMock> (hcp);
			updateResponse.Wait ();

			Assert.AreEqual (updateResponse.Result.Status, HttpStatusCode.OK);
			Assert.That (updateResponse.Result.HasErrors, Is.False);
			Assert.That (updateResponse.Result.Success.ContainsKey (hcp.ID), Is.True);
		}

		[Test]
		public void UpdateCMObjectByID ()
		{
			HCPMock hcp = new HCPMock () {
				ProviderName = "CloudMine Data Hospital",
				ProviderAddress = "1217 Sansom Street | Suite 600, Philadelphia, PA 19107",
				ProviderEmployeeCount = 25
			};

			Task<CMObjectResponse> objResponse = appObjSrvc.SetObject<HCPMock> (hcp);
			objResponse.Wait ();

			Assert.AreEqual (objResponse.Result.Status, HttpStatusCode.OK);
			Assert.That (objResponse.Result.HasErrors, Is.False);
			Assert.That (objResponse.Result.Success.ContainsKey(hcp.ID), Is.True);

			hcp.ProviderName = "CloudMine Data Grave";
			Task<CMObjectResponse> getResponse = appObjSrvc.UpdateObject (hcp.ID, hcp);
			getResponse.Wait ();

			Assert.AreEqual (getResponse.Result.Status, HttpStatusCode.OK);
			Assert.That (getResponse.Result.HasErrors, Is.False);
			Assert.That (getResponse.Result.Success.ContainsKey (hcp.ID), Is.True);
		}

	}
}

