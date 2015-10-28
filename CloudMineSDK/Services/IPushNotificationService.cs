using CloudMineSDK.Model.PushNotifications;
using System.Threading.Tasks;
using CloudmineSDK.Model;

namespace CloudMineSDK.Services
{
	public interface IPushNotificationService
	{
		Task<CMResponse> SendNotification(CMPushNotification pushNotification);

		Task<CMResponse> CreateChannel(CMPushNotificationChannel puchChannel);
		Task<CMResponse> UpdateChannel(CMPushNotificationChannel puchChannel);
		Task<CMResponse> DeleteChannel(string channelName);

		Task<CMResponse> BulkAddChannelSubscribers(string channelName, CMPushUser[] usersToAdd);
		Task<CMResponse> BulkAddChannelDeviceIDs(string channelName, string[] deviceIDs);
		Task<CMResponse> BulkRemoveChannelSubscribers(string channelName, string[] userIdsToRemove);
		Task<CMResponse> BulkRemoveChannelDeviceIDs(string channelName, string[] deviceIdsToRemove);
		Task<CMResponse> SubscribeToChannel(string channelName, CMUser userToAdd);
		Task<CMResponse> UnsubscribeToChannel(string channelName, CMUser userToRemove);

		Task<CMResponse> RegisterAndroidDevicePushNotifications(CMUser user, object deviceToken);
		Task<CMResponse> UnRegisterAndroidDevicePushNotifications(CMUser user);
		Task<CMResponse> RegisterWPDevicePushNotifications(CMUser user, object deviceToken);
		Task<CMResponse> UnRegisterWPDevicePushNotifications(CMUser user);
		Task<CMResponse> RegisterIOSDevicePushNotifications(CMUser user, object deviceToken);
		Task<CMResponse> UnRegisterIOSDevicePushNotifications(CMUser user);
		string StripIOSDeviceToken(string deviceID);
	}	
}
