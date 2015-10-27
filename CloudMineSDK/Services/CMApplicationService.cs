//using CloudmineSDK.Model;
//using CloudmineSDK.Services;
//using CloudMineSDK.Scripts.Model.Responses;
//using CloudMineSDK.Scripts.Services;
//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Net.Http;
//
//namespace CloudMineSDK.Services
//{
//	public class CMApplicationService : IApplicationService
//	{
//		private CMApplication Application { get; set; }
//		private IRestWrapper APIService { get; set; }
//
//		public CMApplicationService(CMApplication application, IRestWrapper apiService)
//		{
//			Application = application;
//			APIService = apiService;
//		}
//
//		#region Get
//		// Get ==============
//		public void GetObject(Action<CMObjectFetchResponse> responseAction, string key = null, CMRequestOptions opts = null)
//		{
//			if (opts == null) opts = new CMRequestOptions();
//			if (key != null) opts.Query["keys"] = key;
//
//			APIService.Request(this.Application, "text", HttpMethod.Get, null, opts, (req, resp) =>
//			{
//				responseAction(new CMObjectFetchResponse(resp));
//			});
//		}
//
//		public void GetObject<T>(Action<CMObjectFetchResponse<T>> responseAction, string key = null, CMRequestOptions opts = null) where T : CMObject
//		{
//			if (opts == null) opts = new CMRequestOptions();
//			if (key != null) opts.Query["keys"] = key;
//
//			APIService.Request(this.Application, "text", HttpMethod.Get, null, opts, (req, resp) =>
//			{
//				responseAction(new CMObjectFetchResponse<T>(resp));
//			});
//		}
//
//		public void GetObjects(Action<CMObjectFetchResponse> responseAction, List<string> keys, CMRequestOptions opts = null)
//		{
//			GetObjects(responseAction, keys.ToArray(), opts);
//		}
//
//		public void GetObjects<T>(CMUser user, Action<CMObjectFetchResponse<T>> responseAction, List<string> keys, CMRequestOptions opts = null) where T : CMObject
//		{
//			GetObjects<T>(responseAction, keys.ToArray(), opts);
//		}
//
//		public void GetObjects(Action<CMObjectFetchResponse> responseAction, string[] keys, CMRequestOptions opts = null)
//		{
//			if (opts == null) opts = new CMRequestOptions();
//			if (keys.Length > 0) opts.Headers["keys"] = String.Join(",", keys);
//
//			APIService.Request(this.Application, "text", HttpMethod.Get, null, opts, (req, resp) =>
//			{
//				responseAction(new CMObjectFetchResponse(resp));
//			});
//		}
//
//		public void GetObjects<T>(Action<CMObjectFetchResponse<T>> responseAction, string[] keys, CMRequestOptions opts = null) where T : CMObject
//		{
//			if (opts == null) opts = new CMRequestOptions();
//			if (keys.Length > 0) opts.Headers["keys"] = String.Join(",", keys);
//
//			APIService.Request(this.Application, "text", HttpMethod.Get, null, opts, (req, resp) =>
//			{
//				responseAction(new CMObjectFetchResponse<T>(resp));
//			});
//		}
//		#endregion
//
//		#region Set
//		// Set ==============
//		/// <summary>
//		/// Method to create objects under a particular user. If the id exists already, the object will replace the existing object.
//		/// </summary>
//		/// <param name="data">Data being uploaded to the server</param>
//		/// <param name="user">the user for which the object will be created</param>
//		/// <param name="responseAction">delegate handler for the server response</param>
//		/// <param name="opts">request options</param>
//		public void SetObject(object data, Action<CMObjectResponse> responseAction, CMRequestOptions opts = null)
//		{
//			APIService.Request(this.Application, "text", HttpMethod.Put, CMSerializer.ToStream(data), new CMRequestOptions(opts), (req, resp) =>
//			{
//				responseAction(new CMObjectResponse(resp));
//			});
//		}
//
//		/// <summary>
//		/// Method to create objects under a particular user. If the id exists already, the object will replace the existing object.
//		/// </summary>
//		/// <param name="id">the id to classify the object under. defaults to the type of object value being passed in</param>
//		/// <param name="value">the cloudmine object being created</param>
//		/// <param name="user">the user for which this object will be created</param>
//		/// <param name="responseAction">delegate handler for the server repsonse</param>
//		/// <param name="opts">request options</param>
//		public void SetObject(object value, Action<CMObjectResponse> responseAction, CMRequestOptions opts = null, string key = null, string type = null)
//		{
//			Dictionary<string, object> data = new Dictionary<string, object>();
//
//			if (!string.IsNullOrEmpty(key))
//				data[key] = value;
//			if (!string.IsNullOrEmpty(type))
//				data[type] = type;
//
//			APIService.Request(this.Application, "text", HttpMethod.Put, CMSerializer.ToStream(data), new CMRequestOptions(opts), (req, resp) =>
//			{
//				responseAction(new CMObjectResponse(resp));
//			});
//		}
//
//		/// <summary>
//		/// Set executes a PUT request on the object being passed in. PUT requests will create a new object or replace an prior existing object if the
//		/// key (__id__, ID) already exists. 
//		/// </summary>
//		/// <typeparam name="T">Any object which derives from CMObject. CMObject auto declares and creates unique identifiers and the class type</typeparam>
//		/// <param name="data"></param>
//		/// <param name="user"></param>
//		/// <param name="responseAction"></param>
//		/// <param name="opts"></param>
//		public void SetObject<T>(T data, Action<CMObjectResponse> responseAction, CMRequestOptions opts = null) where T : CMObject
//		{
//			Dictionary<string, object> dataDict = new Dictionary<string, object>();
//			// only set the type if not already set and T is not CMobject
//			if (string.IsNullOrEmpty(data.Class) && typeof(T).Name != typeof(CMObject).Name)
//				data.Class = typeof(T).Name;
//
//			dataDict.Add(data.ID, data);
//
//			APIService.Request(this.Application, "text", HttpMethod.Put, CMSerializer.ToStream(data), new CMRequestOptions(opts), (req, resp) =>
//			{
//				responseAction(new CMObjectResponse(resp));
//			});
//		}
//		#endregion
//
//		#region Update
//		// Update ===========
//		public void UpdateObject(object data, Action<CMObjectResponse> responseAction, CMRequestOptions opts = null)
//		{
//			APIService.Request(this.Application, "user/text", HttpMethod.Post, CMSerializer.ToStream(data), new CMRequestOptions(opts), (req, resp) =>
//			{
//				responseAction(new CMObjectResponse(resp));
//			});
//		}
//
//		public void UpdateObject(string key, object value, Action<CMObjectResponse> responseAction, CMRequestOptions opts = null)
//		{
//			Dictionary<string, object> data = new Dictionary<string, object>();
//			data[key] = value;
//
//			APIService.Request(this.Application, "user/text", HttpMethod.Post, CMSerializer.ToStream(data), new CMRequestOptions(opts), (req, resp) =>
//			{
//				responseAction(new CMObjectResponse(resp));
//			});
//		}
//
//		public void UpdateObject<T>(T data, Action<CMObjectResponse> responseAction, CMRequestOptions opts = null) where T : CMObject
//		{
//			APIService.Request(this.Application, "user/text", HttpMethod.Post, CMSerializer.ToStream(data), new CMRequestOptions(opts), (req, resp) =>
//			{
//				responseAction(new CMObjectResponse(resp));
//			});
//		}
//		#endregion
//
//		#region Delete
//		// Delete ============
//		public void DeleteObject(string key, Action<CMObjectResponse> responseAction, CMRequestOptions opts = null)
//		{
//			if (key != null)
//			{
//				if (opts == null) opts = new CMRequestOptions();
//				opts.Query["keys"] = key;
//				APIService.Request(this.Application, "text", HttpMethod.Delete, null, new CMRequestOptions(opts), (req, resp) =>
//				{
//					responseAction(new CMObjectResponse(resp));
//				});
//			}
//		}
//
//		public void DeleteObject<T>(T data, Action<CMObjectResponse> responseAction, CMRequestOptions opts = null) where T : CMObject
//		{
//			if (!string.IsNullOrEmpty(data.ID))
//				DeleteObject(data.ID, responseAction, opts);
//		}
//
//		public void DeleteObjects(string[] keys, Action<CMObjectResponse> responseAction, CMRequestOptions opts = null)
//		{
//			if (keys != null && keys.Length > 0)
//			{
//				if (opts == null) opts = new CMRequestOptions();
//				opts.Query["keys"] = String.Join(",", keys);
//				APIService.Request(this.Application, "text", HttpMethod.Delete, null, new CMRequestOptions(opts), (req, resp) =>
//				{
//					responseAction(new CMObjectResponse(resp));
//				});
//			}
//		}
//
//		public void DeleteObjects<T>(List<T> data, Action<CMObjectResponse> responseAction, CMRequestOptions opts = null) where T : CMObject
//		{
//			if (data.Count > 0)
//				DeleteObjects(data.Select(d => d.ID).ToArray(), responseAction, opts);
//		}
//
//		public void DeleteAllObjects(Action<CMObjectResponse> responseAction, CMRequestOptions opts = null)
//		{
//			if (opts == null) opts = new CMRequestOptions();
//			opts.Query["all"] = true.ToString();
//			APIService.Request(this.Application, "text", HttpMethod.Delete, null, new CMRequestOptions(opts), (req, resp) =>
//			{
//				responseAction(new CMObjectResponse(resp));
//			});
//		}
//		#endregion
//
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
//	}
//}
