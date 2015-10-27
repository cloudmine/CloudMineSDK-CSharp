using Newtonsoft.Json;

namespace CloudMineSDK.Scripts.Model.PushNotifications
{
	[JsonObject(MemberSerialization.OptIn)]
	public class CMPushNotificationChannel
	{
		[JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
		public string Name { get; set; }
		[JsonProperty("users", NullValueHandling = NullValueHandling.Ignore)]
		public CMPushUser[] Users { get; set; }
		[JsonProperty("device_ids", NullValueHandling = NullValueHandling.Ignore)]
		public string[] DeviceIDs { get; set; }

		public CMPushNotificationChannel()
		{
			Name = string.Empty;
			Users = null;
			DeviceIDs = null;
		}
	}
}
