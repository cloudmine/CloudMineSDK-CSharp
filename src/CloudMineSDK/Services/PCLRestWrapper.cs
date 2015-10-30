using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using CloudmineSDK.Model;

namespace CloudMineSDK.Services
{
    public class PCLRestWrapper : IRestWrapper
    {
		public Task<CMResponse> Request(CMApplication app, string action, HttpMethod method, Stream content, CMRequestOptions options)
        {
			return Request<CMResponse>(app, action, method, content, options);
        }

		public async Task<T> Request<T>(CMApplication app, string action, HttpMethod method, Stream content, CMRequestOptions options) where T: CMResponse, new()
        {
			HttpClientHandler clientHandler = new HttpClientHandler()
			{
				AllowAutoRedirect = true
			};

			using (HttpClient httpClient = new HttpClient(clientHandler)) {
				CMRequestOptions opts = options ?? new CMRequestOptions();

				// Set various query options.
				List<string> query = GetCloudMineQuery(opts);
				Uri uri = GetCloudmineUri(app.APIVersion, app.ApplicationID, action, query);
				httpClient.BaseAddress = uri;

				if (opts.Credentials != null)
				{
					if (!string.IsNullOrEmpty(opts.Credentials.Username))
					{
						var authData = string.Format ("{0}:{1}", opts.Credentials.Username, opts.Credentials.Password);
						var authHeaderValue = Convert.ToBase64String (System.Text.Encoding.UTF8.GetBytes (authData));
						httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue ("Basic", authHeaderValue);
					}
				}

				httpClient.DefaultRequestHeaders.Add("X-CloudMine-ApiKey", app.APIKey);
				//httpClient.DefaultRequestHeaders.Add("Content-Type", opts.ContentType);
				httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(opts.ContentType));

				foreach (string headerKey in opts.Headers.Keys)
				{
					httpClient.DefaultRequestHeaders.Add(headerKey, opts.Headers[headerKey]);
				}

				StreamContent contentData = content != null ? new StreamContent(content) : new StreamContent(opts.Data);
				StringContent stringContent = new StringContent (await contentData.ReadAsStringAsync (), System.Text.Encoding.UTF8, "application/json");

				if (method == HttpMethod.Post)
				{
					HttpResponseMessage rsp = await httpClient.PostAsync (uri, stringContent);
					using (HttpResponseMessage responseMsg = await httpClient.PostAsync(uri, stringContent))
						return await GenerateCMResponseObject<T>(responseMsg);
				}
				else if (method == HttpMethod.Put)
				{
					using (HttpResponseMessage responseMsg = await httpClient.PutAsync(uri, new StringContent(await contentData.ReadAsStringAsync(), System.Text.Encoding.UTF8, "application/json")))
						return await GenerateCMResponseObject<T>(responseMsg);
				}
				else if (method == HttpMethod.Get)
				{
					using (HttpResponseMessage responseMsg = await httpClient.GetAsync(uri))
						return await GenerateCMResponseObject<T>(responseMsg);
				}
				else if (method == HttpMethod.Delete)
				{
					using (HttpResponseMessage responseMsg = await httpClient.DeleteAsync(uri))
						return await GenerateCMResponseObject<T>(responseMsg);
				}
				else
					throw new InvalidOperationException(string.Format("{0} is not a CloudMine API supported HttpMethod.", method.Method));				
				
			}
        }

		public async Task<T> GenerateCMResponseObject<T>(HttpResponseMessage response) where T: CMResponse, new()
		{
			using (HttpContent content = response.Content)
			{
				var cloudmineResponse = new T();

				if (response == null) {
					cloudmineResponse.Initialize (response.StatusCode, null);
				} else if (response.Content == null || string.IsNullOrEmpty (response.Content.ToString ())) {
					cloudmineResponse.Initialize (response.StatusCode, null);
				} else {
					Stream responseStream = await content.ReadAsStreamAsync();
					//responseStream.CopyTo(cloudmineResponse.DataStream);

					cloudmineResponse.Initialize (response.StatusCode, responseStream);
				}

				return cloudmineResponse;
			}
		}

        public List<string> GetCloudMineQuery(CMRequestOptions opts)
        {
            List<string> query = new List<string>();

            if (opts.LimitResults > -1)
                opts.Query["limit"] = opts.LimitResults.ToString();
            if (opts.SkipResults > -1)
                opts.Query["skip"] = opts.SkipResults.ToString();
            if (opts.CountResults)
                opts.Query["count"] = true.ToString();
            if (opts.Snippet != null)
            {
                opts.Query["f"] = opts.Snippet;
                if (opts.SnippetResultOnly)
                    opts.Query["result_only"] = "true";
                if (opts.SnippetParams.Count > 0)
                    opts.Query["params"] = CMSerializer.ToString(opts.SnippetParams);
            }

            foreach (string key in opts.Query.Keys)
            {
                query.Add(Uri.EscapeUriString(key) + "=" + Uri.EscapeUriString(opts.Query[key]));
            }

            return query;
        }

        private Uri GetCloudmineUri(string version, string applicationID, string action, List<string> query)
        {
            UriBuilder ub = new UriBuilder();
            ub.Scheme = "https";
            ub.Host = "api.cloudmine.me";
			ub.Path = string.Format ("{0}/app/{1}/{2}", version, applicationID, action);
            ub.Query = String.Join("&", query.ToArray(), 0, query.Count);

            return ub.Uri;
        }
			
        private byte[] StreamToByteArray(Stream stream)
        {
            if (stream is MemoryStream)
            {
                return ((MemoryStream)stream).ToArray();
            }
            else
                return ReadStreamFully(stream);
        }

        private static byte[] ReadStreamFully(Stream input)
        {
            byte[] buffer = new byte[16 * 1024];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }
    }
}
