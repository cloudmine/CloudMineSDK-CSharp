using System;
using CloudMineSDK.Services;
using NUnit.Framework;
using CloudMineSDKNUnitTests;
using CloudMineSDK.Model.Responses;
using System.Threading.Tasks;
using System.Net;
using CloudmineSDK.Model;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace Testing
{
	public class CMAppObjGetTests
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
		public void GetObjectFromID ()
		{
			HCPMock hcp = new HCPMock () {
				ProviderName = "CloudMine Data Hospital 3",
				ProviderAddress = "1217 Sansom Street | Suite 600, Philadelphia, PA 19107",
				ProviderEmployeeCount = 25
			};

			Task<CMObjectResponse> objResponse = appObjSrvc.SetObject<HCPMock> (hcp);
			objResponse.Wait ();

			Assert.AreEqual (objResponse.Result.Status, HttpStatusCode.OK);
			Assert.That (objResponse.Result.HasErrors, Is.False);
			Assert.That (objResponse.Result.Success.ContainsKey(hcp.ID), Is.True);

			Task<CMObjectFetchResponse<HCPMock>> getResponse = appObjSrvc.GetObject<HCPMock> (hcp.ID);
			getResponse.Wait ();

			Assert.AreEqual (getResponse.Result.Status, HttpStatusCode.OK);
			Assert.That (getResponse.Result.HasErrors, Is.False);
			Assert.That (getResponse.Result.Success.ContainsKey (hcp.ID), Is.True);
			Assert.AreEqual (getResponse.Result.Success [hcp.ID].GetType (), typeof(HCPMock));
		}

		[Test]
		public void GetObjectFromIDs()
		{
			HCPMock hcp1 = new HCPMock () {
				ProviderName = "CloudMine Data Hospital 1",
				ProviderAddress = "1217 Sansom Street | Suite 600, Philadelphia, PA 19107",
				ProviderEmployeeCount = 25
			};

			HCPMock hcp2 = new HCPMock () {
				ProviderName = "CloudMine Data Hospital 2",
				ProviderAddress = "1217 Sansom Street | Suite 600, Philadelphia, PA 19107",
				ProviderEmployeeCount = 25
			};

			// insert the first object
			Task<CMObjectResponse> objResponse = appObjSrvc.SetObject<HCPMock> (hcp1);
			objResponse.Wait ();

			// insert the second object
			objResponse = appObjSrvc.SetObject<HCPMock> (hcp2);
			objResponse.Wait ();

			Task<CMObjectFetchResponse<HCPMock>> getResponse = appObjSrvc.GetObjects<HCPMock> (new string[] { hcp1.ID, hcp2.ID });
			getResponse.Wait ();

			Assert.AreEqual (getResponse.Result.Status, HttpStatusCode.OK);
			Assert.That (getResponse.Result.HasErrors, Is.False);
			Assert.That (getResponse.Result.Success.ContainsKey (hcp1.ID), Is.True);
			Assert.That (getResponse.Result.Success.ContainsKey (hcp2.ID), Is.True);
			Assert.AreEqual (getResponse.Result.Success [hcp1.ID].GetType (), typeof(HCPMock));
			Assert.AreEqual (getResponse.Result.Success [hcp2.ID].GetType (), typeof(HCPMock));
		}

		[Test]
		public void GetObjectFromIDsWithMixedResults()
		{
			HCPMock hcp1 = new HCPMock () {
				ProviderName = "CloudMine Data Hospital 1",
				ProviderAddress = "1217 Sansom Street | Suite 600, Philadelphia, PA 19107",
				ProviderEmployeeCount = 25
			};

			HCPMock hcp2 = new HCPMock () {
				ProviderName = "CloudMine Data Hospital 2",
				ProviderAddress = "1217 Sansom Street | Suite 600, Philadelphia, PA 19107",
				ProviderEmployeeCount = 25
			};

			// insert the first object
			Task<CMObjectResponse> objResponse = appObjSrvc.SetObject<HCPMock> (hcp1);
			objResponse.Wait ();

			// insert the second object
			objResponse = appObjSrvc.SetObject<HCPMock> (hcp2);
			objResponse.Wait ();

			Task<CMObjectFetchResponse<HCPMock>> getResponse = appObjSrvc.GetObjects<HCPMock> (new string[] { hcp1.ID, hcp2.ID, "NotARealID" });
			getResponse.Wait ();

			Assert.AreEqual (getResponse.Result.Status, HttpStatusCode.OK);
			Assert.That (getResponse.Result.HasErrors, Is.True);

			Assert.That (getResponse.Result.Success.ContainsKey (hcp1.ID), Is.True);
			Assert.That (getResponse.Result.Success.ContainsKey (hcp2.ID), Is.True);
			Assert.That (getResponse.Result.Errors.ContainsKey ("NotARealID"), Is.True);

			Assert.AreEqual (getResponse.Result.Success [hcp1.ID].GetType (), typeof(HCPMock));
			Assert.AreEqual (getResponse.Result.Success [hcp2.ID].GetType (), typeof(HCPMock));
			Assert.AreEqual (getResponse.Result.Errors ["NotARealID"].GetType (), typeof(JObject));
		}

		[Test]
		public void GetObjectFromIDsListWithMixedResults()
		{
			HCPMock hcp1 = new HCPMock () {
				ProviderName = "CloudMine Data Hospital 1",
				ProviderAddress = "1217 Sansom Street | Suite 600, Philadelphia, PA 19107",
				ProviderEmployeeCount = 25
			};

			HCPMock hcp2 = new HCPMock () {
				ProviderName = "CloudMine Data Hospital 2",
				ProviderAddress = "1217 Sansom Street | Suite 600, Philadelphia, PA 19107",
				ProviderEmployeeCount = 25
			};

			// insert the first object
			Task<CMObjectResponse> objResponse = appObjSrvc.SetObject<HCPMock> (hcp1);
			objResponse.Wait ();

			// insert the second object
			objResponse = appObjSrvc.SetObject<HCPMock> (hcp2);
			objResponse.Wait ();

			Task<CMObjectFetchResponse<HCPMock>> getResponse = appObjSrvc.GetObjects<HCPMock> (new List<string>() { hcp1.ID, hcp2.ID, "NotARealID" });
			getResponse.Wait ();

			Assert.AreEqual (getResponse.Result.Status, HttpStatusCode.OK);
			Assert.That (getResponse.Result.HasErrors, Is.True);

			Assert.That (getResponse.Result.Success.ContainsKey (hcp1.ID), Is.True);
			Assert.That (getResponse.Result.Success.ContainsKey (hcp2.ID), Is.True);
			Assert.That (getResponse.Result.Errors.ContainsKey ("NotARealID"), Is.True);

			Assert.AreEqual (getResponse.Result.Success [hcp1.ID].GetType (), typeof(HCPMock));
			Assert.AreEqual (getResponse.Result.Success [hcp2.ID].GetType (), typeof(HCPMock));
			Assert.AreEqual (getResponse.Result.Errors ["NotARealID"].GetType (), typeof(JObject));
		}
	}
}

