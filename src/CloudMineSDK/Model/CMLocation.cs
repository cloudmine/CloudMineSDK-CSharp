using System;
using Newtonsoft.Json;

namespace CloudMineSDK
{
	[JsonObject(MemberSerialization.OptIn)]
	public class CMLocation
	{
		[JsonProperty("__type__")]
		private const string __type__ = "geopoint";

		[JsonProperty("longitude")]
		public double Longitude { get; set; }

		[JsonProperty("latitude")]
		public double Latitude { get; set; }

		public CMLocation (double lat, double lon)
		{
			Latitude = lat;
			Longitude = lon;
		}
	}
}

