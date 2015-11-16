using System;
using CloudmineSDK.Model;
using Newtonsoft.Json;

namespace CloudMineSDKiOSTutorial
{
	[JsonObject(MemberSerialization.OptIn)]
	public class HCP : CMObject
	{
		[JsonProperty("ProviderName", NullValueHandling=NullValueHandling.Ignore)]
		public string ProviderName { get; set; }

		[JsonProperty("ProviderAddress", NullValueHandling=NullValueHandling.Ignore)]
		public string ProviderAddress { get; set; }

		[JsonProperty("ProviderEmployeeCount", NullValueHandling=NullValueHandling.Ignore)]
		public int ProviderEmployeeCount { get; set; }

		public HCP (): base()
		{
		}
	}
}

