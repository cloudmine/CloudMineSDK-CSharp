using Newtonsoft.Json;

namespace CloudMineSDK.Model.PushNotifications
{
	[JsonObject(MemberSerialization.OptIn)]
	public class CMPushNotification
	{
		[JsonProperty("channel", NullValueHandling = NullValueHandling.Ignore)]
		public string Channel { get; set; }
		[JsonProperty("device_ids", NullValueHandling = NullValueHandling.Ignore)]
		public string[] DeviceIds { get; set; }
		[JsonProperty("users", NullValueHandling = NullValueHandling.Ignore)]
		public CMPushUser[] Users { get; set; }
		[JsonProperty("text", NullValueHandling = NullValueHandling.Ignore)]
		public string Text { get; set; }
		[JsonProperty("payload", NullValueHandling = NullValueHandling.Ignore)]
		public object Payload { get; set; }


		public CMPushNotification()
		{
			Channel = null;
			DeviceIds = null;
			Users = null;
			Text = null;
			Payload = null;
		}
	}
}
