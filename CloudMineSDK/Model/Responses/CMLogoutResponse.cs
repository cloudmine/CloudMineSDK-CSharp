using CloudmineSDK.Model;
using System.IO;
using System.Net;

namespace CloudMineSDK.Scripts.Model.Responses
{
	/// <summary>
	/// Logout calls don't have a body in the response object so doing the
	/// raw response read and generic object parsing is not needed.
	/// </summary>
	public class CMLogoutResponse : CMResponse
	{
		public CMLogoutResponse()
			: base()
		{

		}

		public CMLogoutResponse(HttpStatusCode code, Stream dataStream)
			: base()
		{
			Status = code;
			ContentLength = dataStream.Length;
			ContentType = string.Empty;
			DataStream = dataStream;
		}
	}
}
