using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ComponentModel;
using System.Net;

namespace CloudmineSDK.Model
{
    public class CMResponse : EventArgs
    {
		public HttpStatusCode Status { get; internal set; }

        public string ContentType { get; internal set; }

        public long ContentLength { get; internal set; }

        public Stream DataStream { get; internal set; }

        public CMResponse()
        {
			Status = HttpStatusCode.Accepted;
            ContentLength = long.MinValue;
            ContentType = string.Empty;
            DataStream = null;
        }

        public CMResponse(CMResponse response)
        {
            Status = response.Status;
            ContentLength = response.ContentLength;
            ContentType = response.ContentType;
            DataStream = response.DataStream;
        }

		public CMResponse(HttpStatusCode code, Stream dataStream)
        {
            Status = code;
            ContentLength = dataStream.Length;
            ContentType = string.Empty;
            DataStream = dataStream;
        }

        public virtual void Initialize(HttpStatusCode code, Stream dataStream)
        {
            Status = code;
            if (dataStream != null)
            {
                ContentLength = dataStream.Length;
            }
           
            ContentType = string.Empty;
            DataStream = dataStream;
        }

        public virtual bool HasErrors { get { return false; } }

        public bool IsSuccess
        {
            get { return (int)this.Status < 300; }
        }
    }
}
