using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CloudmineSDK.Model
{
	/// <summary>
	/// This is just a sample canned profile. A user profile is an 
	/// optional member of a CloudMine user.
	/// 
	/// This class can be sublassed to include any values which should 
	/// be include on the user profile. NOTE: the user profile is to
	/// be considered publicly accessible so it is not wise to store
	/// PII information on the profile but rather as a user object.
	/// </summary>
	[JsonObject(MemberSerialization.OptIn)]
	public class CMUserProfile
	{
		[JsonProperty("__id__")]
		public string UserID { get; set; }
		[JsonProperty("__type__")]
		public string Type { get { return "user"; } }

		public CMUserProfile()
		{
		}
	}
}