using CloudmineSDK.Model;
using System.Collections.Generic;
using System.IO;
using System.Net;

namespace CloudMineSDK.Model.Responses
{
    public class CMObjectResponse : CMJSONResponse
    {
        public Dictionary<string, string> Success { get; private set; }

        public Dictionary<string, object> Errors { get; private set; }

        public override bool HasErrors
        {
            get
            {
                if (Errors == null)
                    return false;
                return this.Errors.Count > 0;
            }
        }

        public CMObjectResponse()
        {
            
        }

		public CMObjectResponse(HttpStatusCode code, Stream dataStream)
            : base(code, dataStream)
        {
            ExtractKeyAndErrors();
        }

        public CMObjectResponse(CMResponse response)
            : base(response)
        {
            ExtractKeyAndErrors();
        }

		public override void Initialize(HttpStatusCode code, Stream dataStream)
        {
            base.Initialize(code, dataStream);
            ExtractKeyAndErrors();
        }

        void ExtractKeyAndErrors()
        {
            Errors = ExtractErrors();
            Success = ExtractKey<Dictionary<string, string>>("success");
        }
    }
}
