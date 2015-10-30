using CloudmineSDK.Model;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;

namespace CloudMineSDK.Model.Responses
{
    public class CMJSONResponse : CMResponse
    {
        public string RawJSON { get; internal set; }

        public JObject RawObject { get; internal set; }

        private const string errorKey = "errors";

        public CMJSONResponse()
            : base()
        {
            RawJSON = string.Empty;
            RawObject = null;
        }

        public CMJSONResponse(CMResponse response)
            : base(response)
        {
            RawJSON = new StreamReader(DataStream).ReadToEnd();
            if (!string.IsNullOrEmpty(RawJSON) && RawJSON.Length > 1) // logout response bodies have one character which causes an empty check to fail
				RawObject = JObject.Parse(RawJSON);
        }

        public CMJSONResponse(HttpStatusCode code, Stream dataStream)
            : base(code, dataStream)
        {
			if (dataStream != null)
			{
				RawJSON = new StreamReader(DataStream).ReadToEnd();
				if (!string.IsNullOrEmpty(RawJSON) && RawJSON.Length > 1) // logout response bodies have one character which causes an empty check to fail
					RawObject = JObject.Parse(RawJSON);
			}
			else
			{
				RawObject = null;
			}
        }

        public override void Initialize(HttpStatusCode code, Stream dataStream)
        {
            base.Initialize(code, dataStream);

			if (dataStream != null)
			{
				RawJSON = new StreamReader(DataStream).ReadToEnd();
				if (!string.IsNullOrEmpty(RawJSON) && RawJSON.Length > 1) // logout response bodies have one character which causes an empty check to fail
					RawObject = JObject.Parse(RawJSON);
			}
			else
			{
				RawObject = null;
			}
        }

        /// <summary>
        /// Extracts the root part if there are any from the response object.
        /// Returns the default value for T if the key is null
        /// </summary>
        /// <param name="key">Reference to the root key to be parsed into the generic type.</param>
        protected virtual T ExtractKey<T>(string key)
        {
            T result;
			if (RawObject != null && RawObject[key] != null)
                result = RawObject[key].ToObject<T>();
            else
                result = default(T);

            return result;
        }

        protected virtual Dictionary<string, object> ExtractErrors()
        {
            Dictionary<string, object> errors;
            if (RawObject != null && RawObject[errorKey] != null)
            {
                errors = new Dictionary<string, object>();

                if (RawObject[errorKey].Type == JTokenType.Array)
                {
                    foreach (string err in RawObject[errorKey].Select(err => (string)err).ToList())
                    {
                        if (!errors.ContainsKey(err))
                            errors.Add(err, err);
                    }
                }
                else
                {
                    errors = RawObject[errorKey].ToObject<Dictionary<string, object>>();
                }
            }
            else
                errors = null;

            return errors;
        }
    }
}
