using CloudmineSDK.Model;
using System.IO;
using System.Net;

namespace CloudMineSDK.Model.Responses
{
    /// <summary>
    /// CMResponse derivative which stores the data stream for 
    /// file conversion into the choice file type. 
    /// </summary>
    public class CMFileResponse : CMResponse
    {
        public CMFileResponse()
        {

        }

		public CMFileResponse(HttpStatusCode code, Stream dataStream)
            : base(code, dataStream)
        {
        }

     

        public override bool HasErrors
        {
			get { return (Status == HttpStatusCode.OK || Status == HttpStatusCode.Created) == false; }
        }
    }
}
