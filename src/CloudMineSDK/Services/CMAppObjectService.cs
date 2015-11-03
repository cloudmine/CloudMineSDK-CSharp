using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using CloudmineSDK.Model;
using CloudMineSDK.Model.Responses;
using CloudMineSDK.Services;
using CloudmineSDK.Services;
using Newtonsoft.Json;
using System.Text;

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
		/// <summary>
		/// Gets an object of type CMObject by (key,__id__) and returns that Type
		/// automatically parsed into the proper type in the Success field of the results.
		/// </summary>
		/// <returns>The object.</returns>
		/// <param name="key">key,__id__,ID</param>
		/// <param name="opts">Optional Request parameters for things like post execution snippet params.</param>
		/// <typeparam name="T">a Type which derives from CMObject. Used to parse results</typeparam>
		public Task<CMObjectFetchResponse<T>> GetObject<T>(string key = null, CMRequestOptions opts = null) where T : CMObject
		{
			if (opts == null) opts = new CMRequestOptions();
			if (key != null) opts.Query["keys"] = key;

			return APIService.Request<CMObjectFetchResponse<T>>(this.Application, "text", HttpMethod.Get, null, opts);
		}

		/// <summary>
		/// Gets objects of type CMObject by (key,__id__) and returns that Type
		/// </summary>
		/// <returns>The objects in Success and Errors dictionaries.</returns>
		/// <param name="keys">key,__id__,ID</param>
		/// <param name="opts">Optional Request parameters for things like post execution snippet params.</param>
		/// <typeparam name="T">a Type which derives from CMObject. Used to parse result</typeparam>
		public Task<CMObjectFetchResponse<T>> GetObjects<T>(List<string> keys, CMRequestOptions opts = null) where T : CMObject
		{
			return GetObjects<T>(keys.ToArray(), opts);
		}

		// <summary>
		/// Gets objects of type CMObject by (key,__id__) and returns that Type
		/// </summary>
		/// <returns>The objects in Success and Errors dictionaries.</returns>
		/// <param name="keys">key,__id__,ID</param>
		/// <param name="opts">Optional Request parameters for things like post execution snippet params.</param>
		/// <typeparam name="T">a Type which derives from CMObject. Used to parse result</typeparam>
		public Task<CMObjectFetchResponse<T>> GetObjects<T>(string[] keys, CMRequestOptions opts = null) where T : CMObject
		{
			if (opts == null) opts = new CMRequestOptions();
			if (keys.Length > 0) opts.Query["keys"] = String.Join(",", keys);

			return APIService.Request<CMObjectFetchResponse<T>>(this.Application, "text", HttpMethod.Get, null, opts);
		}
		#endregion

		#region Set

		/// <summary>
		/// Method to create objects under a particular user. If the id exists already, the object will replace the existing object.
		/// </summary>
		/// <param name="key">the id to classify the object under. defaults to the type of object value being passed in</param>
		/// <param name="value">the cloudmine object being created</param>
		/// <param name="opts">Optional Request parameters for things like post execution snippet params.</param>
		public async Task<CMObjectResponse> SetObject(object value, CMRequestOptions opts = null, string key = null, string type = null)
		{
			Dictionary<string, object> data = new Dictionary<string, object>();

			if (!string.IsNullOrEmpty(key))
				data[key] = value;
			if (!string.IsNullOrEmpty(type))
				data[type] = type;

			return await APIService.Request<CMObjectResponse>(this.Application, "text", HttpMethod.Put, CMSerializer.ToStream(data), new CMRequestOptions(opts));
		}

		/// <summary>
		/// Set executes a PUT request on the object being passed in. PUT requests will create a new object or replace an prior existing object if the
		/// key (__id__, ID) already exists. 
		/// </summary>
		/// <typeparam name="T">Any object which derives from CMObject. CMObject auto declares and creates unique identifiers and the class type</typeparam>
		/// <param name="data">CMObject to create.</param>
		/// <param name="opts">Optional Request parameters for things like post execution snippet params.</param>
		public async Task<CMObjectResponse> SetObject<T>(T data, CMRequestOptions opts = null) where T : CMObject
		{
			Dictionary<string, object> dataDict = new Dictionary<string, object>();
			// only set the type if not already set and T is not CMobject
			if (string.IsNullOrEmpty(data.Class) && typeof(T).Name != typeof(CMObject).Name)
				data.Class = typeof(T).Name;

			dataDict.Add (data.ID, data);

			Stream stream = CMSerializer.ToStream (dataDict);

			return await APIService.Request<CMObjectResponse>(this.Application, "text", HttpMethod.Put, stream, new CMRequestOptions(opts));
		}
		#endregion

		#region Update
		// Update ===========
		/// <summary>
		/// The values posted in this request are merged with existing values on the server.
		/// If the key you are creating already exists, isn't a simple value (such as a string or number), 
		/// and the new value you send for it also isn't a simple value, its contents will be merged with 
		/// the data you send. Otherwise the contents will be replaced. If the key does not exist, the 
		/// entry will be created.
		/// </summary>
		/// <returns>CMObjectResponse</returns>
		/// <param name="key">Key value to be updated.</param>
		/// <param name="value">Object value to be serialized.</param>
		/// <param name="opts">Optional Request parameters for things like post execution snippet params.</param>
		public Task<CMObjectResponse> UpdateObject(string key, object value, CMRequestOptions opts = null)
		{
			Dictionary<string, object> data = new Dictionary<string, object>();
			data[key] = value;

			return APIService.Request<CMObjectResponse>(this.Application, "text", HttpMethod.Post, CMSerializer.ToStream(data), new CMRequestOptions(opts));
		}

		/// <summary>
		/// The values posted in this request are merged with existing values on the server.
		/// If the key you are creating already exists, isn't a simple value (such as a string or number), 
		/// and the new value you send for it also isn't a simple value, its contents will be merged with 
		/// the data you send. Otherwise the contents will be replaced. If the key does not exist, the 
		/// entry will be created.
		/// </summary>
		/// <returns>The object.</returns>
		/// <param name="data">CMObject value to be serialized.</param>
		/// <param name="opts">Optional Request parameters for things like post execution snippet params.</param>
		/// <typeparam name="T">Objects must derive from the CMObject class which ensures proper configuration.</typeparam>
		public Task<CMObjectResponse> UpdateObject<T>(T data, CMRequestOptions opts = null) where T : CMObject
		{
			Dictionary<string, T> dataDict = new Dictionary<string, T>();
			dataDict[data.ID] = data;

			return APIService.Request<CMObjectResponse>(this.Application, "text", HttpMethod.Post, CMSerializer.ToStream(dataDict), new CMRequestOptions(opts));
		}
		#endregion

		#region Delete
		// Delete ============
		/// <summary>
		/// Deletes objects that match specified keys. If no keys are specified, no action will be taken unless 
		/// the all=true parameter is specified in the URL. If that is specified, all data will be deleted. 
		/// The purpose of this extra parameter is to avoid accidental total data deletion.
		/// </summary>
		/// <returns>CMObjectResponse</returns>
		/// <param name="key">Individual key to the object being deleted</param>
		/// <param name="opts">Optional Request parameters for things like post execution snippet params.</param>
		public Task<CMObjectResponse> DeleteObject(string key, CMRequestOptions opts = null)
		{
			if (key != null)
			{
				if (opts == null) opts = new CMRequestOptions();
					opts.Query["keys"] = key;
				return APIService.Request<CMObjectResponse>(this.Application, "data", HttpMethod.Delete, null, new CMRequestOptions(opts));
			} else {
				throw new InvalidOperationException("Cannot delete empty data. At least one item must be present to delete.");
			}
		}

		/// <summary>
		/// Deletes objects that match specified keys. If no keys are specified, no action will be taken unless 
		/// the all=true parameter is specified in the URL. If that is specified, all data will be deleted. 
		/// The purpose of this extra parameter is to avoid accidental total data deletion.
		/// </summary>
		/// <returns>CMObjectResponse</returns>
		/// <param name="keys">Keys to the objects being deleted</param>
		/// <param name="opts">Optional Request parameters for things like post execution snippet params.</param>
		public Task<CMObjectResponse> DeleteObjects(string[] keys, CMRequestOptions opts = null)
		{
			if (keys != null && keys.Length > 0)
			{
				if (opts == null) opts = new CMRequestOptions();
				opts.Query["keys"] = String.Join(",", keys);

				return APIService.Request<CMObjectResponse>(this.Application, "data", HttpMethod.Delete, null, new CMRequestOptions(opts));
			} else {
				throw new InvalidOperationException("Cannot delete empty data. At least one item must be present to delete.");
			}
		}

		/// <summary>
		/// Deletes objects that match specified keys. If no keys are specified, no action will be taken unless 
		/// the all=true parameter is specified in the URL. If that is specified, all data will be deleted. 
		/// The purpose of this extra parameter is to avoid accidental total data deletion.
		/// </summary>
		/// <returns>CMObjectResponse</returns>
		/// <param name="opts">Optional Request parameters for things like post execution snippet params.</param>
		public Task<CMObjectResponse> DeleteAllObjects(CMRequestOptions opts = null)
		{
			if (opts == null) opts = new CMRequestOptions();
			opts.Query["all"] = true.ToString();

			return APIService.Request<CMObjectResponse>(this.Application, "data", HttpMethod.Delete, null, new CMRequestOptions(opts));
		}
		#endregion

		#region Search
		// Search =============

		/// <summary>
		/// Objects can be fetched via a search query. For example, this allows you to fetch all objects where 
		/// a field has a specific value. The full specification for the query language is specified below.
		/// This query returns all objects where the key field is equal to the string "value".
		/// </summary>
		/// <typeparam name="T">Becomes the __class__ parameter of the query. Uses Type name as the value</typeparam>
		/// <param name="query">String query for CloudMine search. Please reference dos at: https://cloudmine.me/docs/api#query_syntax </param>
		/// <param name="opts">Optional Request parameters for things like post execution snippet params.</param>
		public Task<CMObjectSearchResponse<T>> SearchObjects<T>(string query, CMRequestOptions opts = null) where T : CMObject
		{
			if (opts == null) opts = new CMRequestOptions();
			opts.Query["q"] = query;
			// add __class__ of type T name to the query passed in

			return APIService.Request<CMObjectSearchResponse<T>>(this.Application, "search", HttpMethod.Get, null, new CMRequestOptions(opts));
		}
		#endregion

		#region File
		// Upload file =========
		/// <summary>
		/// Portable class libraries won't work with file path as a parameter so data must be
		/// streamed through the upload method. 
		/// </summary>
		/// <param name="key">Unique identifier for subsequent retrieval, update, and delete.</param>
		/// <param name="data">Stream from a binary or other stream source which should be stored as a file.</param>
		/// <param name="opts">Optional Request parameters for things like post execution snippet params.</param>
		/// <returns></returns>
		public Task<CMFileResponse> Upload(string key, Stream data, CMRequestOptions opts = null)
		{
			if (opts == null) {
				opts = new CMRequestOptions ();
				opts.ContentType = "application/octet-stream";
			}
			return APIService.Request<CMFileResponse>(this.Application, "binary/" + key, HttpMethod.Put, data, new CMRequestOptions(opts));
		}

		// Download file ========
		/// <summary>
		/// Download s specified file specified key and opts.
		/// </summary>
		/// <param name="key">Unique identifier for subsequent retrieval, update, and delete.</param>
		/// <param name="opts">Optional Request parameters for things like post execution snippet params</param>
		public Task<CMFileResponse> Download(string key, CMRequestOptions opts = null)
		{
			if (opts == null) {
				opts = new CMRequestOptions ();
				opts.ContentType = "application/octet-stream";
			}
			return APIService.Request<CMFileResponse>(this.Application, "binary/" + key, HttpMethod.Get, null, new CMRequestOptions(opts));
		}
		#endregion

		#region Snippet
		// Code snippet =========
		public Task<CMResponse> Run(String snippet, HttpMethod method, Dictionary<string, string> parameters = null, CMRequestOptions opts = null)
		{
			if (opts == null) opts = new CMRequestOptions();
			foreach (string id in parameters.Keys)
			{
				opts.Query[id] = parameters[id];
			}
			foreach (string id in opts.SnippetParams.Keys)
			{
				opts.Query[id] = opts.SnippetParams[id];
			}

			opts.Snippet = null;
			opts.SnippetParams.Clear();

			return APIService.Request(Application, "run/" + snippet, method, null, opts);
		}
		#endregion
	}
}
