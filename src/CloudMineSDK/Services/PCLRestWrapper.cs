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
		/// <summary>
		/// Request the specified app, action, method, content and options.
		/// </summary>
		/// <param name="app">An instance of application ID and API key.</param>
		/// <param name="action">URL action which the platform should execute ("user/binary/", "user/search")</param>
		/// <param name="method">HTTP method type which the action uses (PUT, POST, GET, DELETE)</param>
		/// <param name="content">Content stream to be sent in the body. Also can be passed through on options</param>
		/// <param name="options">CMRequestOptions which contains the necessary values for query parameters and headers.</param>
		public Task<CMResponse> Request(CMApplication app, string action, HttpMethod method, Stream content, CMRequestOptions options)
        {
			return Request<CMResponse>(app, action, method, content, options);
        }

		/// <summary>
		/// Request the specified app, action, method, content and options.
		/// </summary>
		/// <param name="app">An instance of application ID and API key.</param>
		/// <param name="action">URL action which the platform should execute ("user/binary/", "user/search")</param>
		/// <param name="method">HTTP method type which the action uses (PUT, POST, GET, DELETE)</param>
		/// <param name="content">Content stream to be sent in the body. Also can be passed through on options</param>
		/// <param name="options">CMRequestOptions which contains the necessary values for query parameters and headers.</param>
		/// <typeparam name="T">CMResponse type derivative which wraps the return shape in the task response.</typeparam>
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
				Uri uri = GetCloudmineUri(options.BaseURL ?? "api.cloudmine.me", app.APIVersion, app.ApplicationID, action, query);
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

				if (method == HttpMethod.Post)
				{
					// Login requests post no body
					if (content != null || opts.Data != null) {
						StreamContent contentData = content != null ? new StreamContent (content) : new StreamContent (opts.Data);
						StringContent stringContent = new StringContent (await contentData.ReadAsStringAsync (), System.Text.Encoding.UTF8, "application/json");

						using (HttpResponseMessage responseMsg = await httpClient.PostAsync (uri, stringContent))
							return await GenerateCMResponseObject<T> (responseMsg);
					} else {
						using (HttpResponseMessage responseMsg = await httpClient.PostAsync (uri, new StringContent (string.Empty)))
							return await GenerateCMResponseObject<T> (responseMsg);
					}
				}
				else if (method == HttpMethod.Put)
				{
					StreamContent contentData = content != null ? new StreamContent(content) : new StreamContent(opts.Data);
					StringContent stringContent = new StringContent (await contentData.ReadAsStringAsync (), System.Text.Encoding.UTF8, "application/json");

					using (HttpResponseMessage responseMsg = await httpClient.PutAsync(uri, stringContent))
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

		/// <summary>
		/// Generates the CM response object given the proper CMResponse type.
		/// All responses inherit from CMResponse so if the return type is known
		/// the base objcet contains everything needed to drill down.
		/// </summary>
		/// <returns>The CM response object.</returns>
		/// <param name="response">Response.</param>
		/// <typeparam name="T">CMResponse derivative</typeparam>
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


		/// <summary>
		/// Take a set of request options and extracts the proper query
		/// parameters. Does not deal with the necessary headers used
		/// in a CM REST call.
		/// </summary>
		/// <returns>The cloud mine query.</returns>
		/// <param name="opts">Opts.</param>
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

		private Uri GetCloudmineUri(string baseURL, string version, string applicationID, string action, List<string> query)
        {
            UriBuilder ub = new UriBuilder();
            ub.Scheme = "https";
			ub.Host = baseURL;
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
