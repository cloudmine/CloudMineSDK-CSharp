using CloudmineSDK.Model;
using CloudmineSDK.Services;
using CloudMineSDK.Scripts.Model.Responses;
using CloudMineSDK.Scripts.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace CloudMineSDK.Services
{
	public class CMAppObjectService : IAppObjectService
	{
		private CMApplication Application { get; set; }
		private IRestWrapper APIService { get; set; }

		public CMAppObjectService(CMApplication application, IRestWrapper apiService)
		{
			Application = application;
			APIService = apiService;
		}

		#region Get
		// Get ==============
		public Task<CMObjectFetchResponse> GetObject(string key = null, CMRequestOptions opts = null)
		{
			if (opts == null) opts = new CMRequestOptions();
			if (key != null) opts.Query["keys"] = key;

			return APIService.Request<CMObjectFetchResponse>(this.Application, "text", HttpMethod.Get, null, opts);
		}

		public Task<CMObjectFetchResponse<T>> GetObject<T>(string key = null, CMRequestOptions opts = null) where T : CMObject
		{
			if (opts == null) opts = new CMRequestOptions();
			if (key != null) opts.Query["keys"] = key;

			return APIService.Request<CMObjectFetchResponse<T>>(this.Application, "text", HttpMethod.Get, null, opts);
		}

		public Task<CMObjectFetchResponse> GetObjects(List<string> keys, CMRequestOptions opts = null)
		{
			return GetObjects(keys.ToArray(), opts);
		}

		public Task<CMObjectFetchResponse<T>> GetObjects<T>(List<string> keys, CMRequestOptions opts = null) where T : CMObject
		{
			return GetObjects<T>(keys.ToArray(), opts);
		}

		public Task<CMObjectFetchResponse> GetObjects(string[] keys, CMRequestOptions opts = null)
		{
			if (opts == null) opts = new CMRequestOptions();
			if (keys.Length > 0) opts.Headers["keys"] = String.Join(",", keys);

			return APIService.Request<CMObjectFetchResponse>(this.Application, "text", HttpMethod.Get, null, opts);
		}

		public Task<CMObjectFetchResponse<T>> GetObjects<T>(string[] keys, CMRequestOptions opts = null) where T : CMObject
		{
			if (opts == null) opts = new CMRequestOptions();
			if (keys.Length > 0) opts.Headers["keys"] = String.Join(",", keys);

			return APIService.Request<CMObjectFetchResponse<T>>(this.Application, "text", HttpMethod.Get, null, opts);
		}
		#endregion

		#region Set
		// Set ==============
		/// <summary>
		/// Method to create objects under a particular user. If the id exists already, the object will replace the existing object.
		/// </summary>
		/// <param name="data">Data being uploaded to the server</param>
		/// <param name="user">the user for which the object will be created</param>
		/// <param name="responseAction">delegate handler for the server response</param>
		/// <param name="opts">request options</param>
		public Task<CMObjectResponse> SetObject(object data, CMRequestOptions opts = null)
		{
			return APIService.Request<CMObjectResponse>(this.Application, "text", HttpMethod.Put, CMSerializer.ToStream(data), new CMRequestOptions(opts));
		}

		/// <summary>
		/// Method to create objects under a particular user. If the id exists already, the object will replace the existing object.
		/// </summary>
		/// <param name="id">the id to classify the object under. defaults to the type of object value being passed in</param>
		/// <param name="value">the cloudmine object being created</param>
		/// <param name="user">the user for which this object will be created</param>
		/// <param name="responseAction">delegate handler for the server repsonse</param>
		/// <param name="opts">request options</param>
		public Task<CMObjectResponse> SetObject(object value, CMRequestOptions opts = null, string key = null, string type = null)
		{
			Dictionary<string, object> data = new Dictionary<string, object>();

			if (!string.IsNullOrEmpty(key))
				data[key] = value;
			if (!string.IsNullOrEmpty(type))
				data[type] = type;

			return APIService.Request<CMObjectResponse>(this.Application, "text", HttpMethod.Put, CMSerializer.ToStream(data), new CMRequestOptions(opts));
		}

		/// <summary>
		/// Set executes a PUT request on the object being passed in. PUT requests will create a new object or replace an prior existing object if the
		/// key (__id__, ID) already exists. 
		/// </summary>
		/// <typeparam name="T">Any object which derives from CMObject. CMObject auto declares and creates unique identifiers and the class type</typeparam>
		/// <param name="data"></param>
		/// <param name="user"></param>
		/// <param name="responseAction"></param>
		/// <param name="opts"></param>
		public Task<CMObjectResponse> SetObject<T>(T data, CMRequestOptions opts = null) where T : CMObject
		{
			Dictionary<string, object> dataDict = new Dictionary<string, object>();
			// only set the type if not already set and T is not CMobject
			if (string.IsNullOrEmpty(data.Class) && typeof(T).Name != typeof(CMObject).Name)
				data.Class = typeof(T).Name;

			dataDict.Add(data.ID, data);

			return APIService.Request<CMObjectResponse>(this.Application, "text", HttpMethod.Put, CMSerializer.ToStream(data), new CMRequestOptions(opts));
		}
		#endregion

		#region Update
		// Update ===========
		public Task<CMObjectResponse> UpdateObject(object data, CMRequestOptions opts = null)
		{
			return APIService.Request<CMObjectResponse>(this.Application, "user/text", HttpMethod.Post, CMSerializer.ToStream(data), new CMRequestOptions(opts));
		}

		public Task<CMObjectResponse> UpdateObject(string key, object value, CMRequestOptions opts = null)
		{
			Dictionary<string, object> data = new Dictionary<string, object>();
			data[key] = value;

			return APIService.Request<CMObjectResponse>(this.Application, "user/text", HttpMethod.Post, CMSerializer.ToStream(data), new CMRequestOptions(opts));
		}

		public Task<CMObjectResponse> UpdateObject<T>(T data, CMRequestOptions opts = null) where T : CMObject
		{
			return APIService.Request<CMObjectResponse>(this.Application, "user/text", HttpMethod.Post, CMSerializer.ToStream(data), new CMRequestOptions(opts));
		}
		#endregion

		#region Delete
		// Delete ============
		public Task<CMObjectResponse> DeleteObject(string key, CMRequestOptions opts = null)
		{
			if (key != null)
			{
				if (opts == null) opts = new CMRequestOptions();
				opts.Query["keys"] = key;
				return APIService.Request<CMObjectResponse>(this.Application, "text", HttpMethod.Delete, null, new CMRequestOptions(opts));
			} else {
				throw new InvalidOperationException("Cannot delete empty data. At least one item must be present to delete.");
			}
		}

		public Task<CMObjectResponse> DeleteObject<T>(T data, CMRequestOptions opts = null) where T : CMObject
		{
			if (!string.IsNullOrEmpty(data.ID))
				return DeleteObject(data.ID, opts);
			else {
				throw new InvalidOperationException("Cannot delete empty data. At least one item must be present to delete.");
			}
		}

		public Task<CMObjectResponse> DeleteObjects(string[] keys, CMRequestOptions opts = null)
		{
			if (keys != null && keys.Length > 0)
			{
				if (opts == null) opts = new CMRequestOptions();
				opts.Query["keys"] = String.Join(",", keys);

				return APIService.Request<CMObjectResponse>(this.Application, "text", HttpMethod.Delete, null, new CMRequestOptions(opts));
			} else {
				throw new InvalidOperationException("Cannot delete empty data. At least one item must be present to delete.");
			}
		}

		public Task<CMObjectResponse> DeleteObjects<T>(List<T> data, CMRequestOptions opts = null) where T : CMObject
		{
			if (data.Count > 0)
				return DeleteObjects(data.Select(d => d.ID).ToArray(), opts);
			else {
				throw new InvalidOperationException("Cannot delete empty data. At least one item must be present to delete.");
			} 
		}

		public Task<CMObjectResponse> DeleteAllObjects(CMRequestOptions opts = null)
		{
			if (opts == null) opts = new CMRequestOptions();
			opts.Query["all"] = true.ToString();

			return APIService.Request<CMObjectResponse>(this.Application, "text", HttpMethod.Delete, null, new CMRequestOptions(opts));
		}
		#endregion

//		#region Search
//		// Search =============
//		public void SearchObjects(string query, Action<CMObjectSearchResponse> responseAction, CMRequestOptions opts = null)
//		{
//			if (opts == null) opts = new CMRequestOptions();
//			opts.Query["q"] = query;
//
//			APIService.Request(this.Application, "search", HttpMethod.Get, null, new CMRequestOptions(opts), (req, resp) =>
//			{
//				responseAction(new CMObjectSearchResponse(resp));
//			});
//		}
//
//		/// <summary>
//		/// Performs a search query based on the string query and ensures the type of the query results
//		/// are of the generic type parameter.
//		/// </summary>
//		/// <typeparam name="T">Becomes the __class__ parameter of the query. Uses Type name as the value</typeparam>
//		/// <param name="query">String query for CloudMine search. Please reference dos at: https://cloudmine.me/docs/api#query_syntax </param>
//		/// <param name="user"></param>
//		/// <param name="responseAction"></param>
//		/// <param name="opts"></param>
//		public void SearchObjects<T>(string query, Action<CMObjectSearchResponse<T>> responseAction, CMRequestOptions opts = null) where T : CMObject
//		{
//			if (opts == null) opts = new CMRequestOptions();
//			opts.Query["q"] = query;
//			// add __class__ of type T name to the query passed in
//
//			APIService.Request(this.Application, "search", HttpMethod.Get, null, new CMRequestOptions(opts), (req, resp) =>
//			{
//				responseAction(new CMObjectSearchResponse<T>(resp));
//			});
//		}
//		#endregion
//
//		#region File
//		// Upload file =========
//		/// <summary>
//		/// Portable class libraries won't work with file path as a parameter so data must be
//		/// streamed through the upload method. 
//		/// </summary>
//		/// <param name="id"></param>
//		/// <param name="data"></param>
//		/// <param name="opts"></param>
//		/// <returns></returns>
//		public void Upload(string key, Stream data, Action<CMResponse> responseAction, CMRequestOptions opts = null)
//		{
//			APIService.Request(this.Application, "binary/" + key, HttpMethod.Put, data, new CMRequestOptions(opts), (req, resp) =>
//			{
//				responseAction(new CMResponse(resp));
//			});
//		}
//
//		// Download file ========
//		public void Download(string key, Action<CMFileResponse> responseAction, CMRequestOptions opts = null)
//		{
//			APIService.Request<CMFileResponse>(this.Application, "binary/" + key, HttpMethod.Get, null, new CMRequestOptions(opts), (req, resp) =>
//			{
//				responseAction(resp);
//			});
//		}
//		#endregion
//
//		#region Snippet
//		// Code snippet =========
//		public void Run(String snippet, Action<CMResponse> responseAction, HttpMethod method, Dictionary<string, string> parameters = null, CMRequestOptions opts = null)
//		{
//			if (opts == null) opts = new CMRequestOptions();
//			foreach (string id in parameters.Keys)
//			{
//				opts.Query[id] = parameters[id];
//			}
//			foreach (string id in opts.SnippetParams.Keys)
//			{
//				opts.Query[id] = opts.SnippetParams[id];
//			}
//
//			opts.Snippet = null;
//			opts.SnippetParams.Clear();
//
//			APIService.Request(Application, "run/" + snippet, method, null, opts, (req, resp) =>
//			{
//				responseAction(new CMResponse(resp));
//			});
//		}
//		#endregion
	}
}
