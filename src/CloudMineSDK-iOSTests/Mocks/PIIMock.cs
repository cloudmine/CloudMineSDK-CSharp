using System;
using Newtonsoft.Json;
using CloudmineSDK.Model;

namespace CloudMineSDKNUnit
{
	[JsonObject(MemberSerialization.OptIn)]
	public class PIIMock: CMObject
	{
		[JsonProperty("FirstName", NullValueHandling=NullValueHandling.Ignore)]
		public string FirstName { get; set; }

		[JsonProperty("LastName", NullValueHandling=NullValueHandling.Ignore)]
		public string LastName { get; set; }

		[JsonProperty("DoB", NullValueHandling=NullValueHandling.Ignore)]
		public DateTime DateOfBirth { get; set; }

		[JsonProperty("SSN", NullValueHandling=NullValueHandling.Ignore)]
		public string SocialSecurityNumber { get; set; }

		public PIIMock (): base()
		{
		}
	}
}

