using System;
using NUnit.Framework;
using CloudmineSDK.Model;
using CloudMineSDK.Services;
using CloudmineSDK.Services;
using System.Threading.Tasks;
using CloudMineSDK.Model.Responses;
using System.Net;
using System.Dynamic;

namespace CloudMineSDKiOSTests
{
	[TestFixture]
	public class CMApplicationObjectTests
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
		public void SetObjectFromDataObject ()
		{
			dynamic hcp = new ExpandoObject ();

			hcp.__id__ = Guid.NewGuid().ToString();
			hcp.ProviderName = "CloudMine Data Hospital 2";
			hcp.ProviderAddress = "1217 Sansom Street | Suite 600, Philadelphia, PA 19107";
			hcp.ProviderEmployeeCount = 25;

			Task<CMObjectResponse> objResponse = appObjSrvc.SetObject (hcp);
			objResponse.Wait ();

			Assert.AreEqual (objResponse.Result.Status, HttpStatusCode.OK);
			Assert.That (objResponse.Result.HasErrors, Is.False);
			Assert.That (objResponse.Result.Success.ContainsKey(hcp.ID), Is.True);
		}

		[Test]
		public void SetObjectFromKeyAndDataObject ()
		{
			dynamic hcp = new ExpandoObject ();

			hcp.__id__ = Guid.NewGuid().ToString();
			hcp.ProviderName = "CloudMine Data Hospital 2";
			hcp.ProviderAddress = "1217 Sansom Street | Suite 600, Philadelphia, PA 19107";
			hcp.ProviderEmployeeCount = 25;

			Task<CMObjectResponse> objResponse = appObjSrvc.SetObject (hcp, null, "12345");
			objResponse.Wait ();

			Assert.AreEqual (objResponse.Result.Status, HttpStatusCode.OK);
			Assert.That (objResponse.Result.HasErrors, Is.False);
			Assert.That (objResponse.Result.Success.ContainsKey(hcp.ID), Is.True);
		}

		[Test]
		public void SetObjectFromCMObject ()
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
		}
	}
}

