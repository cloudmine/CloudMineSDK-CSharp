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

		
		[SetUp]
		public void Setup ()
		{
			CMApplication app = new CMApplication (appID, apiKey);
			IRestWrapper api = new PCLRestWrapper ();
			IAppObjectService appObjSrvc = new CMAppObjectService (app, api);
		}

		[Test]
		public void Pass ()
		{
			Console.WriteLine ("test1");
			Assert.True (true);
		}

		[Test]
		public void Fail ()
		{
			Assert.False (true);
		}

		[Test]
		[Ignore ("another time")]
		public void Ignore ()
		{
			Assert.True (false);
		}

		[Test]
		public void Inconclusive ()
		{
			Assert.Inconclusive ("Inconclusive");
		}
	}
}

