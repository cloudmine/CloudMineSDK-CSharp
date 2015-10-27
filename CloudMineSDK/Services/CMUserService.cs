using CloudmineSDK.Model;
using CloudMineSDK.Scripts.Model.Responses;
using CloudMineSDK.Scripts.Services;
using NetSDKPrivate.Scripts.Model.Responses;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Net.Http;
using System.Net;
using System.Threading.Tasks;

namespace CloudmineSDK.Services
{
	public class CMUserService : IUserService
	{
		private CMApplication Application { get; set; }

		private IRestWrapper APIService { get; set; }

		public CMUserService(CMApplication application, IRestWrapper apiService)
		{
			Application = application;
			APIService = apiService;
		}

		// should be able to call a snippet on any api call
		public Task<CMUserResponse> Create(CMUser user, CMRequestOptions opts)
		{
			// should merge and override passed in options so snippet can be run
			CMRequestOptions request = new CMRequestOptions(opts, user);
			string reqData = JsonConvert.SerializeObject(user);
			byte[] byteArray = Encoding.UTF8.GetBytes(reqData);
			MemoryStream stream = new MemoryStream(byteArray);
			request.Data = stream;

			return APIService.Request<CMUserResponse>(this.Application, "account/create", HttpMethod.Post, stream, request);
		}

		public Task<CMResponse> DeleteUser(CMUser user)
		{
			CMRequestOptions opts = new CMRequestOptions();
			opts.SetCredentials(user.Credentials);

			return APIService.Request(this.Application, "account", HttpMethod.Delete, null, opts);
		}

		#region Session Management

		/// <summary>
		/// Default login method. Uses the CMUserProfofile base object for profile de/serialization
		/// </summary>
		/// <param name="user"></param>
		/// <param name="options"></param>
		public Task<CMUserResponse> Login(CMUser user, CMRequestOptions options = null)
		{
			if (options == null)
				options = new CMRequestOptions(user);

			options.Credentials = new CMCredentials(user.Credentials.Username, user.Credentials.Email, user.Credentials.Password);

			Task<CMUserResponse> act = APIService.Request<CMUserResponse>(this.Application, "account/login", HttpMethod.Post, null, options);
			return act;
		}

		/// <summary>
		/// Login method which allows the specification of the profile object type. T must derive from the CMUserProfile base object. 
		/// </summary>
		/// <typeparam name="T">Type of the CMUserProfile obect.</typeparam>
		/// <param name="user">User object to be logged in.</param>
		/// <param name="options">Any additional options like snippet execution parameters.</param>
		public Task<CMUserResponse> Login<T>(CMUser<T> user, CMRequestOptions options = null) where T : CMUserProfile
		{
			if (options == null)
				options = new CMRequestOptions(user);

			options.Credentials = new CMCredentials(user.Credentials.Username, user.Credentials.Email, user.Credentials.Password);

			return APIService.Request<CMUserResponse>(this.Application, "account/login", HttpMethod.Post, null, options);
		}

		public Task<CMLogoutResponse> Logoff(CMUser user, CMRequestOptions options = null)
		{
			if (options == null)
				options = new CMRequestOptions(null, user);
			
			var loginTask = APIService.Request<CMLogoutResponse>(this.Application, "account/logout", HttpMethod.Post, null, options);
			return loginTask;

			//loginTask.ContinueWith(result => {
			//	if (result.IsCompleted)
			//	{
			//		// erase the session token
			//		user.Session = string.Empty;
			//		user.SessionExpires = DateTime.MinValue;
			//	}
			//});
			//
			return loginTask;
		}

		/// <summary>
		/// Allows for setting a new password for a provided user.
		/// </summary>
		/// <param name="user">Original user with current credentials prior to change</param>
		/// <param name="newPassword"></param>
		public Task<CMResponse> ChangePassword(CMUser user, string newPassword)
		{
			Dictionary<string, string> data = new Dictionary<string, string>();
			data["password"] = newPassword;

			CMRequestOptions opts = new CMRequestOptions();
			opts.SetCredentials(user.Credentials);

			return APIService.Request(this.Application, "account/password/change", HttpMethod.Post, CMSerializer.ToStream(data), opts);
		}

		public Task<CMResponse> ResetPasswordRequest(string email)
		{
			Dictionary<string, string> data = new Dictionary<string, string>();
			data["email"] = email;

			return APIService.Request(this.Application, "account/password/reset", HttpMethod.Post, CMSerializer.ToStream(data), null);
		}

		public Task<CMResponse> ResetPassword(string token, string newPassword)
		{
			Dictionary<string, string> data = new Dictionary<string, string>();
			data["password"] = newPassword;

			return APIService.Request(this.Application, "account/password/reset/" + token, HttpMethod.Post, CMSerializer.ToStream(data), null);
		}

		#endregion Session Management

		#region Get

		// Get ==============
		public Task<CMObjectFetchResponse<CMObject>> GetUserObject(CMUser user, string key, CMRequestOptions opts)
		{
			if (opts == null) opts = new CMRequestOptions(user);
			if (key != null) opts.Query["keys"] = key;

			return APIService.Request<CMObjectFetchResponse<CMObject>>(this.Application, "user/text", HttpMethod.Get, null, opts);
		}

		public Task<CMObjectFetchResponse<T>> GetUserObject<T>(CMUser user, string key, CMRequestOptions opts) where T : CMObject
		{
			if (opts == null) opts = new CMRequestOptions(user);
			if (key != null) opts.Query["keys"] = key; ;

			return APIService.Request<CMObjectFetchResponse<T>>(this.Application, "user/text", HttpMethod.Get, null, opts);
		}

		public Task<CMObjectFetchResponse<CMObject>> GetUserObjects(CMUser user, List<string> keys, CMRequestOptions opts)
		{
			return GetUserObjects<CMObject>(user, keys.ToArray(), opts);
		}

		public Task<CMObjectFetchResponse<T>> GetUserObjects<T>(CMUser user, List<string> keys, CMRequestOptions opts) where T : CMObject
		{
			return GetUserObjects<T>(user, keys.ToArray(), opts);
		}

		public Task<CMObjectFetchResponse<CMObject>> GetUserObjects(CMUser user, string[] keys, CMRequestOptions opts)
		{
			if (keys.Length > 0)
			{
				if (opts == null) opts = new CMRequestOptions(user);
				opts.Headers["keys"] = String.Join(",", keys);
			}

			return APIService.Request<CMObjectFetchResponse<CMObject>>(this.Application, "user/text", HttpMethod.Get, null, opts);
		}

		public Task<CMObjectFetchResponse<T>> GetUserObjects<T>(CMUser user, string[] keys, CMRequestOptions opts) where T : CMObject
		{
			if (keys.Length > 0)
			{
				if (opts == null)
					opts = new CMRequestOptions(user);
				opts.Headers["keys"] = String.Join(",", keys);
			}

			return APIService.Request<CMObjectFetchResponse<T>>(this.Application, "user/text", HttpMethod.Get, null, opts);
		}

		#endregion Get

		#region Set

		// Set ==============
		/// <summary>
		/// Method to create objects under a particular user. If the id exists already, the object will replace the existing object.
		/// </summary>
		/// <param name="data">Data being uploaded to the server</param>
		/// <param name="user">the user for which the object will be created</param>
		/// <param name="opts">request options</param>
		public Task<CMObjectResponse> SetUserObject(object data, CMUser user, CMRequestOptions opts)
		{
			return APIService.Request<CMObjectResponse>(this.Application, "user/text", HttpMethod.Put, CMSerializer.ToStream(data), new CMRequestOptions(opts, user));
		}

		/// <summary>
		/// Method to create objects under a particular user. If the id exists already, the object will replace the existing object.
		/// </summary>
		/// <param name="id">the id to classify the object under. defaults to the type of object value being passed in</param>
		/// <param name="value">the cloudmine object being created</param>
		/// <param name="user">the user for which this object will be created</param>
		/// <param name="responseAction">delegate handler for the server repsonse</param>
		/// <param name="opts">request options</param>
		public Task<CMObjectResponse> SetUserObject(object value, CMUser user, CMRequestOptions opts = null, string key = null, string type = null)
		{
			Dictionary<string, object> data = new Dictionary<string, object>();

			if (!string.IsNullOrEmpty(key))
				data[key] = value;
			if (!string.IsNullOrEmpty(type))
				data[type] = type;

			return APIService.Request<CMObjectResponse>(this.Application, "user/text", HttpMethod.Put, CMSerializer.ToStream(data), new CMRequestOptions(opts, user));
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
		public Task<CMObjectResponse> SetUserObject<T>(T data, CMUser user, CMRequestOptions opts) where T : CMObject
		{
			Dictionary<string, object> dataDict = new Dictionary<string, object>();
			// only set the type if not already set and T is not CMobject
			if (string.IsNullOrEmpty(data.Class) && typeof(T).Name != typeof(CMObject).Name)
				data.Class = typeof(T).Name;

			dataDict.Add(data.ID, data);

			return APIService.Request<CMObjectResponse>(this.Application, "user/text", HttpMethod.Put, CMSerializer.ToStream(dataDict), new CMRequestOptions(opts, user));
		}

		#endregion Set

		#region Update

		// Update ===========
		public Task<CMObjectResponse> UpdateUserObject(object data, CMUser user, CMRequestOptions opts)
		{
			return APIService.Request<CMObjectResponse>(this.Application, "user/text", HttpMethod.Post, CMSerializer.ToStream(data), new CMRequestOptions(opts, user));
		}

		public Task<CMObjectResponse> UpdateUserObject(string key, object value, CMUser user, CMRequestOptions opts)
		{
			Dictionary<string, object> data = new Dictionary<string, object>();
			data[key] = value;

			return APIService.Request<CMObjectResponse>(this.Application, "user/text", HttpMethod.Post, CMSerializer.ToStream(data), new CMRequestOptions(opts, user));
		}

		public Task<CMObjectResponse> UpdateUserObject<T>(T data, CMUser user, CMRequestOptions opts) where T : CMObject
		{
			return APIService.Request<CMObjectResponse>(this.Application, "user/text", HttpMethod.Post, CMSerializer.ToStream(data), new CMRequestOptions(opts, user));
		}

		#endregion Update

		#region Delete

		// Delete ============
		public Task<CMObjectResponse> DeleteUserObject(string key, CMUser user, CMRequestOptions opts)
		{
			if (key != null)
			{
				if (opts == null)
					opts = new CMRequestOptions();
				opts.Query["keys"] = key;
				return APIService.Request<CMObjectResponse>(this.Application, "user/text", HttpMethod.Delete, null, new CMRequestOptions(opts, user));
			} else {
				throw new InvalidOperationException("Cannot delete empty data. At least one item must be present to delete.");
			}
		}

		public Task<CMObjectResponse> DeleteUserObject<T>(T data, CMUser user, CMRequestOptions opts) where T : CMObject
		{
			if (!string.IsNullOrEmpty(data.ID))
				return DeleteUserObject(data.ID, user, opts);
			else {
				throw new InvalidOperationException("Cannot delete empty data. At least one item must be present to delete.");
			}
		}

		public Task<CMObjectResponse> DeleteUserObjects(string[] keys, CMUser user, CMRequestOptions opts)
		{
			if (keys != null && keys.Length > 0)
			{
				if (opts == null)
					opts = new CMRequestOptions();
				opts.Query["keys"] = String.Join(",", keys);
				return APIService.Request<CMObjectResponse>(this.Application, "user/text", HttpMethod.Delete, null, new CMRequestOptions(opts, user));
			} else {
				throw new InvalidOperationException("Cannot delete empty data. At least one item must be present to delete.");
			}
		}

		public Task<CMObjectResponse> DeleteUserObjects<T>(List<T> data, CMUser user, CMRequestOptions opts) where T : CMObject
		{
			if (data != null && data.Count > 0)
				return DeleteUserObjects(data.Select(d => d.ID).ToArray(), user, opts);
			else {
				throw new InvalidOperationException("Cannot delete empty data. At least one item must be present to delete.");
			}
		}

		public Task<CMObjectResponse> DeleteAllUserObjects(CMUser user, CMRequestOptions opts)
		{
			if (opts == null)
				opts = new CMRequestOptions();
			opts.Query["all"] = true.ToString();
			return APIService.Request<CMObjectResponse>(this.Application, "user/text", HttpMethod.Delete, null, new CMRequestOptions(opts, user));
		}

		#endregion Delete

		#region Search

		// Search =============
		public Task<CMObjectSearchResponse> SearchUserObjects(string query, CMUser user, CMRequestOptions opts)
		{
			if (opts == null)
				opts = new CMRequestOptions();
			opts.Query["q"] = query;

			return APIService.Request<CMObjectSearchResponse>(this.Application, "user/search", HttpMethod.Get, null, new CMRequestOptions(opts, user));
		}

		/// <summary>
		/// Performs a search query based on the string query and ensures the type of the query results
		/// are of the generic type parameter.
		/// </summary>
		/// <typeparam name="T">Becomes the __class__ parameter of the query. Uses Type name as the value</typeparam>
		/// <param name="query">String query for CloudMine search. Please reference dos at: https://cloudmine.me/docs/api#query_syntax </param>
		/// <param name="user"></param>
		/// <param name="opts"></param>
		public Task<CMObjectSearchResponse<T>> SearchUserObjects<T>(string query, CMUser user, CMRequestOptions opts) where T : CMObject
		{
			if (opts == null)
				opts = new CMRequestOptions();
			opts.Query["q"] = query;
			// add __class__ of type T name to the query passed in

			return APIService.Request<CMObjectSearchResponse<T>>(this.Application, "user/search", HttpMethod.Get, null, new CMRequestOptions(opts, user));
		}

		#endregion Search

		#region File

		/// <summary>
		///
		/// </summary>
		/// <param name="key">Intended unique key for file reference.</param>
		/// <param name="data">Stream of the binary to be uploaded.</param>
		/// <param name="user">User file uploads requires a user with a valid sessions token.</param>
		/// <param name="opts">Any custom options for the request such as snippet execution on upload completion.</param>
		public Task<CMResponse> Upload(string key, Stream data, CMUser user, CMRequestOptions opts)
		{
			if (opts == null)
				opts = new CMRequestOptions();
			if (!string.IsNullOrEmpty(user.Session))
			{
				opts.Headers["X-CloudMine-SessionToken"] = user.Session;
				opts.SnippetParams.Add("session_token", user.Session ?? string.Empty);
				opts.SnippetParams.Add("user_id", user.UserID ?? string.Empty);
			}

			return APIService.Request(this.Application, "user/binary/" + key, HttpMethod.Put, data, new CMRequestOptions(opts, user));
		}

		// Download file ========
		public Task<CMFileResponse> Download(string key, CMUser user, CMRequestOptions opts)
		{
			if (opts == null)
				opts = new CMRequestOptions();
			
			if (!string.IsNullOrEmpty(user.Session))
			{
				opts.Headers["X-CloudMine-SessionToken"] = user.Session;
				opts.SnippetParams.Add("session_token", user.Session ?? string.Empty);
				opts.SnippetParams.Add("user_id", user.UserID ?? string.Empty);
			}

			return APIService.Request<CMFileResponse>(this.Application, "user/binary/" + key, HttpMethod.Get, null, new CMRequestOptions(opts, user));
		}

		#endregion File

		/*
		 * Here is the TODO for the services not yet implemented but not impeding completion based on 
		 * other methods which can be leveraged to achieve the same results
		 */
		public void ListUsers()
		{
			throw new NotImplementedException();
		}

		public void SearchUsers()
		{
			throw new NotImplementedException();
		}

		public void CurrentUserProfile()
		{
			throw new NotImplementedException();
		}

		public void UpdateUserProfile()
		{
			throw new NotImplementedException();
		}
	}
}