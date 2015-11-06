using System;
using System.Dynamic;
using System.Net;
using System.Threading.Tasks;
using CloudmineSDK.Model;
using CloudMineSDK.Model.Responses;
using CloudMineSDK.Services;
using CloudMineSDKNUnitTests;
using NUnit.Framework;

namespace CloudMineSDKNUnit
{
	public class CMAppObjSearchTests
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
		public void SearchObjectByClassAndCount ()
		{
			HCPMock hcp = new HCPMock () {
				ProviderName = "CloudMine Data Hospital",
				ProviderAddress = "1217 Sansom Street | Suite 600, Philadelphia, PA 19107",
				ProviderEmployeeCount = 200
			};

			Task<CMObjectResponse> objResponse = appObjSrvc.SetObject<HCPMock> (hcp);
			objResponse.Wait ();

			Assert.AreEqual (objResponse.Result.Status, HttpStatusCode.OK);
			Assert.That (objResponse.Result.HasErrors, Is.False);
			Assert.That (objResponse.Result.Success.ContainsKey(hcp.ID), Is.True);

			Task<CMObjectSearchResponse<HCPMock>> searchResponse = appObjSrvc.SearchObjects<HCPMock> (@"[__class__=""HCPMock"", ProviderEmployeeCount=200]");
			searchResponse.Wait ();

			Assert.AreEqual (searchResponse.Result.Status, HttpStatusCode.OK);
			Assert.That (searchResponse.Result.HasErrors, Is.False);
			Assert.That (searchResponse.Result.Success.ContainsKey (hcp.ID), Is.True);
			Assert.AreEqual (searchResponse.Result.Success [hcp.ID].GetType (), typeof(HCPMock));
		}
	}
}

