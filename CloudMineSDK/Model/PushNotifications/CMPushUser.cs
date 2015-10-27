using Newtonsoft.Json;

namespace CloudMineSDK.Scripts.Model.PushNotifications
{
	/// <summary>
	/// This class represents the user object needed for sending push notifications to
	/// either an email or username. Only non-null values will be sent and the first match
	/// on the public properties Email and Username will be used to send the 
	/// notification to the device associated. It is not required to know both values.
	/// If using the device id, it is recommendedto use the DeviceIds string array construct 
	/// in CMPushNotification.
	/// </summary>
	[JsonObject(MemberSerialization.OptIn)]
	public class CMPushUser
	{
		/// <summary>
		/// The email of the user to associate with the device id for a notification.
		/// </summary>
		[JsonProperty("email", NullValueHandling = NullValueHandling.Ignore)]
		public string Email { get; set; }
		/// <summary>
		/// The username of the user to associate with the device id for a notification.
		/// </summary>
		[JsonProperty("username", NullValueHandling = NullValueHandling.Ignore)]
		public string Username { get; set; }
		/// <summary>
		/// The user ID of the user to associate with the device id for a notification.
		/// </summary>
		[JsonProperty("userid", NullValueHandling = NullValueHandling.Ignore)]
		public string UserID { get; set; }


		public CMPushUser()
		{
			Email = null;
			Username = null;
			UserID = null;
		}
	}
}
