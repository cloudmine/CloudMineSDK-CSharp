using CloudmineSDK.Model;
using CloudMineSDK.Services;
using CloudMineSDK.Model.PushNotifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Net.Http;
using System.Threading.Tasks;

namespace CloudMineSDK.Services
{
	public class CMPushNotificationService : IPushNotificationService
	{
		private CMApplication Application { get; set; }
		private IRestWrapper APIService { get; set; }

		public CMPushNotificationService(CMApplication application, IRestWrapper apiService)
		{
			Application = application;
			APIService = apiService;
		}

		public Task<CMResponse> SendNotification(CMPushNotification pushNotification)
		{
			return APIService.Request(Application, "push", HttpMethod.Post, CMSerializer.ToStream(pushNotification), new CMRequestOptions());
		}

		public Task<CMResponse> CreateChannel(CMPushNotificationChannel puchChannel)
		{
			return APIService.Request(Application, "push/channel", HttpMethod.Post, CMSerializer.ToStream(puchChannel), new CMRequestOptions());
		}

		public Task<CMResponse> UpdateChannel(CMPushNotificationChannel puchChannel)
		{
			return APIService.Request(Application, "push/channel", HttpMethod.Post, CMSerializer.ToStream(puchChannel), new CMRequestOptions());
		}

		public Task<CMResponse> DeleteChannel(string channelName)
		{
			return APIService.Request(Application, string.Format("delete/channel/{0}" + channelName), HttpMethod.Delete, null, new CMRequestOptions());
		}

		public Task<CMResponse> BulkAddChannelSubscribers(string channelName, CMPushUser[] usersToAdd)
		{
			return APIService.Request(Application, string.Format("push/channel/{0}/users", channelName), HttpMethod.Post, CMSerializer.ToStream(usersToAdd), new CMRequestOptions());
		}

		public Task<CMResponse> BulkAddChannelDeviceIDs(string channelName, string[] deviceIDs)
		{
			return APIService.Request(Application, string.Format("push/channel/{0}/device_ids", channelName), HttpMethod.Post, CMSerializer.ToStream(deviceIDs), new CMRequestOptions());
		}

		public Task<CMResponse> BulkRemoveChannelSubscribers(string channelName, string[] userIdsToRemove)
		{
			var opts = new CMRequestOptions();
			opts.Parameters.Add("ids", userIdsToRemove.ToString());

			return APIService.Request(Application, string.Format("push/channel/{0}/user_ids", channelName), HttpMethod.Delete, null, opts);
		}

		public Task<CMResponse> BulkRemoveChannelDeviceIDs(string channelName, string[] deviceIdsToRemove)
		{
			var opts = new CMRequestOptions();
			opts.Parameters.Add("ids", deviceIdsToRemove.ToString());

			return APIService.Request(Application, string.Format("push/channel/{0}/device_ids", channelName), HttpMethod.Delete, null, opts);
		}

		public Task<CMResponse> SubscribeToChannel(string channelName, CMUser userToAdd)
		{
			var opts = new CMRequestOptions(userToAdd);

			Dictionary<string, object> dataDict = new Dictionary<string, object>();
			dataDict.Add("user", true);

			return APIService.Request(Application, string.Format("push/channel/{0}/subscribe", channelName), HttpMethod.Post, CMSerializer.ToStream(dataDict), opts);
		}

		public Task<CMResponse> UnsubscribeToChannel(string channelName, CMUser userToRemove)
		{
			var opts = new CMRequestOptions(userToRemove);
			
			Dictionary<string, object> dataDict = new Dictionary<string, object>();
			dataDict.Add("user", true);
			
			return APIService.Request(Application, string.Format("push/channel/{0}/unsubscribe", channelName), HttpMethod.Post, CMSerializer.ToStream(dataDict), opts);
		}

		/// <summary>
		/// Registers the android device push notifications.
		/// </summary>
		/// <returns>The android device push notifications.</returns>
		/// <param name="user">User.</param>
		/// <param name="uniqueDeviceId">Android.OS.Build.Serial</param>
		/// <param name="gcmToken">Device token from Google GCM registration.</param>
		public Task<CMResponse> RegisterAndroidDevicePushNotifications(CMUser user, string uniqueDeviceId, object gcmToken)
		{
			CMRequestOptions options = new CMRequestOptions(null, user);

			options.Headers.Add ("device_type", "android");
			options.Headers.Add ("HTTP_X_CLOUDMINE_UT", uniqueDeviceId);
			options.Headers.Add ("X-CloudMine-Agent", "Android");

			Dictionary<string, object> dataDict = new Dictionary<string, object>();
			dataDict.Add("token", gcmToken);
			dataDict.Add("device_type", "android");
			dataDict.Add("device_id", uniqueDeviceId);

			return APIService.Request(Application, "device", HttpMethod.Post, CMSerializer.ToStream(dataDict), options);
		}

		/// <summary>
		/// unregister android device push notifications.
		/// </summary>
		/// <returns>The register android device push notifications.</returns>
		/// <param name="user">User with valid session.</param>
		/// <param name="uniqueDeviceId">Android.OS.Build.Serial</param>
		public Task<CMResponse> UnRegisterAndroidDevicePushNotifications(CMUser user, string uniqueDeviceId)
		{
			CMRequestOptions options = new CMRequestOptions(null, user);

			options.Headers.Add ("device_type", "android");
			options.Headers.Add ("HTTP_X_CLOUDMINE_UT", uniqueDeviceId);
			options.Headers.Add ("X-CloudMine-Agent", "Android");

			return APIService.Request(Application, "device", HttpMethod.Delete, null, options);
		}

		/// <summary>
		/// Registers the WP device push notifications.
		/// </summary>
		/// <returns>The WP device push notifications.</returns>
		/// <param name="user">User with valid session.</param>
		/// <param name="uniqueDeviceId">Windows.Phone.System.Analytics.HostInformation.PublisherHostId</param>
		/// <param name="wpToken">Device token.</param>
		public Task<CMResponse> RegisterWPDevicePushNotifications(CMUser user, string uniqueDeviceId, object wpToken)
		{
			CMRequestOptions options = new CMRequestOptions(null, user);

			options.Headers.Add ("device_type", "wp");
			options.Headers.Add ("HTTP_X_CLOUDMINE_UT", uniqueDeviceId);
			options.Headers.Add ("X-CloudMine-Agent", "wp");

			Dictionary<string, object> dataDict = new Dictionary<string, object>();
			dataDict.Add("token", wpToken);
			dataDict.Add("device_type", "wp");
			dataDict.Add("device_id", uniqueDeviceId);

			throw new NotImplementedException();
		}

		/// <summary>
		/// Unregister WP device push notifications.
		/// </summary>
		/// <returns>The register WP device push notifications.</returns>
		/// <param name="user">User with valid session.</param>
		/// <param name="uniqueDeviceId">Windows.Phone.System.Analytics.HostInformation.PublisherHostId</param>
		public Task<CMResponse> UnRegisterWPDevicePushNotifications(CMUser user, string uniqueDeviceId)
		{
			CMRequestOptions options = new CMRequestOptions(null, user);

			options.Headers.Add ("device_type", "wp");
			options.Headers.Add ("HTTP_X_CLOUDMINE_UT", uniqueDeviceId);
			options.Headers.Add ("X-CloudMine-Agent", "wp");

			throw new NotImplementedException();
		}

		/// <summary>
		/// Strips the device ID from device token callback value for the method
		/// didRegisterForRemoteNotificationsWithDeviceToken. Requires the Apple device 
		/// identification string contained in the callback and an actively logged in 
		/// CMUser to register with CloudMine.
		/// </summary>
		/// <param name="user">User with valid session</param>
		/// /// <param name="uniqueDeviceId">UIKit.UIDevice.CurrentDevice.IdentifierForVendor.AsString()</param>
		/// <param name="apnsToken">The token object returned in didRegisterForRemoteNotificationsWithDeviceToken</param>
		public Task<CMResponse> RegisterIOSDevicePushNotifications(CMUser user, string uniqueDeviceId, object apnsToken)
		{
			CMRequestOptions options = new CMRequestOptions(null, user);

			options.Headers.Add ("device_type", "ios");
			options.Headers.Add ("HTTP_X_CLOUDMINE_UT", uniqueDeviceId);
			options.Headers.Add ("X-CloudMine-Agent", "iOS");

			string deviceTokenString = StripIOSDeviceToken(apnsToken.ToString());

			Dictionary<string, string> dataDict = new Dictionary<string, string>();
			dataDict.Add("token", deviceTokenString);
			dataDict.Add("device_type", "ios");
			dataDict.Add("device_id", uniqueDeviceId);

			return APIService.Request(Application, "device", HttpMethod.Post, CMSerializer.ToStream(dataDict), options);
		}

		/// <summary>
		/// Allows for unregistering a valid logged in CMUser from push notifications.
		/// </summary>
		/// <param name="user">Valid logged in CMUser</param>
		/// <param name="uniqueDeviceId">UIKit.UIDevice.CurrentDevice.IdentifierForVendor.AsString()</param>
		public Task<CMResponse> UnRegisterIOSDevicePushNotifications(CMUser user, string uniqueDeviceId)
		{
			CMRequestOptions options = new CMRequestOptions(null, user);

			options.Headers.Add ("device_type", "ios");
			options.Headers.Add ("HTTP_X_CLOUDMINE_UT", uniqueDeviceId);
			options.Headers.Add ("X-CloudMine-Agent", "iOS");

			return APIService.Request(Application, "device", HttpMethod.Delete, null, options);
		}

		/// <summary>
		/// Strips the device token of special characters
		/// </summary>
		/// <param name="deviceID"></param>
		public string StripIOSDeviceToken(string deviceID)
		{
			// Instantiate the regular expression object.
			Regex regex = new Regex("(<|\\s|>)", RegexOptions.IgnoreCase);
			string strippedDeviceToken = regex.Replace(deviceID, string.Empty);
			return strippedDeviceToken;
		}
	}
}
