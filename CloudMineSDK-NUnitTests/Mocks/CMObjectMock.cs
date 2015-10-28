using CloudmineSDK.Model;
using Newtonsoft.Json;

namespace NetSDKTests.Mocks
{
	[JsonObject(MemberSerialization.OptIn)]
	public class CMOBjectMock: CMObject
	{
		private string fieldtwo;

		[JsonProperty("field", NullValueHandling=NullValueHandling.Ignore)]
		public string field { get; set; }

		[JsonProperty("field1", NullValueHandling = NullValueHandling.Ignore)]
		public string field1 { get; set; }

		[JsonProperty("field2", NullValueHandling = NullValueHandling.Ignore)]
		public string Field2 { 
			get { return fieldtwo; }
			set
			{
				ApplyPropertyChange<CMOBjectMock,
					string>(ref fieldtwo, o => o.fieldtwo, value);
			}
		}

		public CMOBjectMock()
		{
		}
	}
}
