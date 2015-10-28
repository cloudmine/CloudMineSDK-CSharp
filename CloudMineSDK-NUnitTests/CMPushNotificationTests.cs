using System;
using NUnit.Framework;
using CloudMineSDK.Services;
using CloudmineSDK.Model;
using CloudmineSDK.Services;

namespace NetSDKTests
{
	[TestFixture()]
	public class CMPushNotificationTests
	{
		const string deviceToken = "<740f4707 bebcf74f 9b7c25d4 8e335894 5f6aa01d a5ddb387 462c7eaf 61bb78ad>";

		IPushNotificationService pushService { get; set; }

		[Test()]
		public void CanStripDeviceToken ()
		{
			pushService = new CMPushNotificationService (null, null);

			string expectedToken = "740f4707bebcf74f9b7c25d48e3358945f6aa01da5ddb387462c7eaf61bb78ad";
			string strippedToken = pushService.StripIOSDeviceToken (deviceToken);

			Assert.AreNotEqual (deviceToken, expectedToken);
			Assert.AreEqual(strippedToken, expectedToken);
		}
	}
}