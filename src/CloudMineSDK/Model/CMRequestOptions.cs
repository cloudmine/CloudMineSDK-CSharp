using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security;
using System.Text;
using System.Text.RegularExpressions;

namespace CloudmineSDK.Model
{
    public class CMRequestOptions
    {
        private int limit;
        private bool count;
        private int skip;
        private string snippet;
        private string contentType;
        private bool resultOnly;
        private Dictionary<string, string> query;
        private Dictionary<string, string> headers;
        private Dictionary<string, string> parameters;
        const string DEFAULT_TYPE = "application/json";

		/// <summary>
		/// Denotes the root URL of the CloudMine API endpoints. Examples
		/// include but not limited to: {"api.cloudmine.me", "secure.cloudmine.me", 
		/// "testing.cloudmine.me"}
		/// </summary>
		public string BaseURL = "api.cloudmine.me";

        public CMRequestOptions()
        {
            this.headers = new Dictionary<string, string>();
            this.parameters = new Dictionary<string, string>();
            this.query = new Dictionary<string, string>();
            this.skip = -1;
            this.limit = -1;
            this.resultOnly = false;
            this.snippet = null;
            this.count = false;
            this.contentType = DEFAULT_TYPE;
        }

        public CMRequestOptions(CMRequestOptions opts)
            : this()
        {
            this.Merge(opts);
        }

        public CMRequestOptions(CMUser user)
            : this()
        {
            if (user != null && !string.IsNullOrEmpty(user.Session))
                this.Headers["X-CloudMine-SessionToken"] = user.Session;
        }


        public CMRequestOptions(CMRequestOptions opts, CMUser user)
            : this()
        {
            this.Merge(opts);

            if (user != null && !string.IsNullOrEmpty(user.Session))
                this.Headers["X-CloudMine-SessionToken"] = user.Session;
        }

        /// <summary>
        /// Merges the request shape in place instead of returning a new object.
        /// </summary>
        /// <param name="opts"></param>
        /// <returns></returns>
        public CMRequestOptions Merge(CMRequestOptions opts)
        {
            if (opts != null)
            {
                this.headers = this.headers.Concat(opts.headers).ToDictionary(e => e.Key, e => e.Value);
                this.parameters = this.parameters.Concat(opts.parameters).ToDictionary(e => e.Key, e => e.Value);
				this.query = this.query.Concat(opts.query).ToDictionary(e => e.Key, e => e.Value);
                this.count = opts.count;

                if (opts.Data != null)
                    this.Data = opts.Data;
                if (opts.Credentials != null)
                    this.Credentials = opts.Credentials;
                if (opts.limit > -1)
                    this.limit = opts.limit;
                if (opts.skip > -1)
                    this.skip = opts.skip;
                if (opts.snippet != null)
                    this.snippet = opts.snippet;
                if (!string.IsNullOrEmpty(opts.ContentType))
                    this.contentType = opts.contentType;
            }
            return this;
        }

        public string ContentType
        {
            get { return this.contentType; }
            set
            {
                this.contentType = (Regex.Match(value, ".+/.+", RegexOptions.IgnoreCase).Success ? value.ToLower() : DEFAULT_TYPE);
            }
        }

        public Dictionary<string, string> Headers
        {
            get { return this.headers; }
        }

        public bool CountResults
        {
            get { return this.count; }
            set { this.count = value; }
        }

        public int LimitResults
        {
            get { return this.limit; }
            set { this.limit = value; }
        }

        public Dictionary<string, string> Query
        {
            get { return this.query; }
        }

		public Dictionary<string, string> Parameters
		{
			get { return this.parameters; }
			set { this.parameters = value; }
		}

        public int SkipResults
        {
            get { return this.skip; }
            set { this.skip = Math.Max(0, value); }
        }

        public string Snippet
        {
            get { return this.snippet; }
            set { this.snippet = value != "" ? value : null; }
        }

        public Dictionary<string, string> SnippetParams
        {
            get { return this.parameters; }
        }

        public bool SnippetResultOnly
        {
            get { return this.resultOnly; }
            set { this.resultOnly = value; }
        }

        public void SetCredentials(CMCredentials credentials)
        {
            Credentials = credentials;
        }

        internal CMCredentials Credentials { get; set; }

        internal Stream Data { get; set; }
    }
}
