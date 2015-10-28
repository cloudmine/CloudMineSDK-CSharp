using CloudmineSDK.Model;
using CloudmineSDK.Services;
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

		public CMPushNotificationService(IRestWrapper apiService, CMApplication application)
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

		public Task<CMResponse> RegisterAndroidDevicePushNotifications(CMUser user, object deviceToken)
		{
			throw new NotImplementedException();
		}

		public Task<CMResponse> UnRegisterAndroidDevicePushNotifications(CMUser user)
		{
			throw new NotImplementedException();
		}

		public Task<CMResponse> RegisterWPDevicePushNotifications(CMUser user, object deviceToken)
		{
			throw new NotImplementedException();
		}
		
		public Task<CMResponse> UnRegisterWPDevicePushNotifications(CMUser user)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Strips the device ID from device token callback value for the method
		/// didRegisterForRemoteNotificationsWithDeviceToken. Requires the Apple device 
		/// identification string contained in the callback and an actively logged in 
		/// CMUser to register with CloudMine.
		/// </summary>
		/// <param name="user">Valid logged in CMUser</param>
		/// <param name="deviceToken">The token objcet returned in didRegisterForRemoteNotificationsWithDeviceToken</param>
		/// <param name="registerResponseAction">Callback Action for handling the response</param>
		public Task<CMResponse> RegisterIOSDevicePushNotifications(CMUser user, object deviceToken)
		{
			string deviceTokenString = StripIOSDeviceToken(deviceToken.ToString());

			Dictionary<string, object> dataDict = new Dictionary<string, object>();
			dataDict.Add("token", deviceTokenString);

			return APIService.Request(Application, "device/", HttpMethod.Post, CMSerializer.ToStream(dataDict), new CMRequestOptions(user));
		}

		/// <summary>
		/// Allows for unregistering a valid logged in CMUser from push notifications.
		/// </summary>
		/// <param name="user">Valid logged in CMUser</param>
		/// <param name="unRegisterResponseAction">Callback Action for handling the response</param>
		public Task<CMResponse> UnRegisterIOSDevicePushNotifications(CMUser user)
		{
			return APIService.Request(Application, "device/", HttpMethod.Delete, null, new CMRequestOptions(user));
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
