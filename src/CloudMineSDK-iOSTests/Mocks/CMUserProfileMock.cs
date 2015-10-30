using CloudmineSDK.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSDKTests.Mocks
{
	[JsonObject(MemberSerialization.OptIn)]
	public class CMUserProfileMock : CMUserProfile
	{
		[JsonProperty("favorite_cafe")]
		public string FavoriteCafe { get; set; }

		public CMUserProfileMock()
		{

		}
	}
}
