using System;
using NUnit.Framework;
using CloudmineSDK.Model;
using CloudMineSDK.Services;
using CloudmineSDK.Services;

namespace CloudMineSDKiOSTests
{
	[TestFixture]
	public class CMApplicationObjectTests
	{
		private const string appID = "";
		private const string apiKey = "";
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
			HCPMock hcp = new HCPMock () {
				ProviderName = "CloudMine Data Hospital",
				ProviderAddress = "1217 Sansom Street | Suite 600, Philadelphia, PA 19107",
				ProviderEmployeeCount = 25
			};

			var result = appObjSrvc.SetObject<HCPMock> (hcp);
			Assert.True (false);
		}

		[Test]
		public void SetObjectFromKeyAndDataObject ()
		{
			Assert.True (false);
		}

		[Test]
		public void SetObjectFromCMObject ()
		{
			Assert.True (false);
		}
	}
}

