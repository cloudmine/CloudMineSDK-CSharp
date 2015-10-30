using CloudmineSDK.Model;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.IO;
using System.Net;

namespace CloudMineSDK.Model.Responses
{
    public class CMObjectFetchResponse<T> : CMJSONResponse where T : CMObject
    {
        public Dictionary<string, T> Success { get; private set; }

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

        public CMObjectFetchResponse()
        {
            
        }

		public CMObjectFetchResponse(HttpStatusCode code, Stream dataStream)
            : base(code, dataStream)
        {
            ExtractKeyAndErrors();
        }

        public CMObjectFetchResponse(CMResponse response)
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
            Success = ExtractKey<Dictionary<string, T>>("success");
            Errors = ExtractErrors();
        }
    }

    public class CMObjectFetchResponse : CMJSONResponse
    {
        public Dictionary<string, JObject> Success { get; private set; }

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

        public CMObjectFetchResponse()
        {
            
        }

		public CMObjectFetchResponse(HttpStatusCode code, Stream dataStream)
            : base(code, dataStream)
        {
            ExtractKeyAndErrors();
        }

        public CMObjectFetchResponse(CMResponse response)
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
            Success = ExtractKey<Dictionary<string, JObject>>("success");
            Errors = ExtractErrors();
        }
    }
}
