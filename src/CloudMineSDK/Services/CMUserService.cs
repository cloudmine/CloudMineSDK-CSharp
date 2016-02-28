using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using CloudmineSDK.Model;
using CloudMineSDK.Model.Responses;
using CloudMineSDK.Services;
using NetSDKPrivate.Model.Responses;
using Newtonsoft.Json;

namespace CloudMineSDK.Services
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

		/// <summary>
		/// Create the specified user. User can also have a custom user profile
		/// definition or a custom profile can be set at a later time with the update
		/// user profile function. This function creates the user but doesn't log the
		/// user in automatically. 
		/// </summary>
		/// <param name="user">Instance of the user to be created</param>
		/// <param name="opts">Optional Request parameters for things like post execution snippet params.</param>
		public Task<CMUserResponse> Create(CMUser user, CMRequestOptions opts = null)
		{
			if (opts == null)
				opts = new CMRequestOptions(user);
			else
				opts = new CMRequestOptions(opts, user);
			
			string reqData = JsonConvert.SerializeObject(user);
			byte[] byteArray = Encoding.UTF8.GetBytes(reqData);
			MemoryStream stream = new MemoryStream(byteArray);
			opts.Data = stream;

			Task<CMUserResponse> createUser = APIService.Request<CMUserResponse>(this.Application, "account/create", HttpMethod.Post, stream, opts);

			createUser.ContinueWith(result => {
				if (result.IsCompleted)
				{
					// sets the user id of the user object passed in
					if (!result.Result.HasErrors)
						user.UserID = result.Result.CMUser.UserID;
				}
			});

			return createUser;
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
		/// <param name="user">User to be logged in. Requires credentials on the user object which will override
		/// credentials passed in on the options parameter if not null.</param>
		/// <param name="options">Optional Request parameters for things like post execution snippet params.</param>
		public Task<CMUserResponse> Login(CMUser user, CMRequestOptions options = null)
		{
			if (options == null)
				options = new CMRequestOptions(user);

			options.Credentials = new CMCredentials(user.Credentials.Username, user.Credentials.Email, user.Credentials.Password);

			var login = APIService.Request<CMUserResponse> (this.Application, "account/login", HttpMethod.Post, null, options);

			login.ContinueWith(result => {
					// sets the user id of the user object passed in
					if (!result.Result.HasErrors) {
						user.Session = result.Result.CMUser.Session;
						user.SessionExpires = result.Result.CMUser.SessionExpires;
					}
				}, TaskContinuationOptions.OnlyOnRanToCompletion)
				.Wait(); // ensures the continue with happens before user defined things

			return login;
		}

		/// <summary>
		/// Login method which allows the specification of the profile object type. T must derive from the CMUserProfile base object. 
		/// </summary>
		/// <typeparam name="T">Type of the CMUserProfile obect.</typeparam>
		/// <param name="user">User object to be logged in. Requires credentials on the user object which will override
		/// credentials passed in on the options parameter if not null.</param>
		/// <param name="options">Any additional options like snippet execution parameters.</param>
		public Task<CMUserResponse<T>> Login<T>(CMUser<T> user, CMRequestOptions options = null) where T : CMUserProfile
		{
			if (options == null)
				options = new CMRequestOptions(user);

			options.Credentials = new CMCredentials(user.Credentials.Username, user.Credentials.Email, user.Credentials.Password);

			var login = APIService.Request<CMUserResponse<T>>(this.Application, "account/login", HttpMethod.Post, null, options);

			login.ContinueWith(result => {
					// sets the user id of the user object passed in
					if (!result.Result.HasErrors) {
						user.Session = result.Result.CMUser.Session;
						user.SessionExpires = result.Result.CMUser.SessionExpires;
					}
				}, TaskContinuationOptions.OnlyOnRanToCompletion)
				.Wait(); // ensures the continue with happens before user defined things

			return login;
		}

		/// <summary>
		/// Logoff the specified user and options. Leverages the credentials or session token
		/// available on the user object to invalidate user login. Will also set the current
		/// session token and sesssion expiration to empty on a successful request.
		/// </summary>
		/// <param name="user">User.</param>
		/// <param name="options">Any additional options like snippet execution parameters.</param>
		public Task<CMLogoutResponse> Logoff(CMUser user, CMRequestOptions options = null)
		{
			if (options == null)
				options = new CMRequestOptions(null, user);
			
			var logout = APIService.Request<CMLogoutResponse>(this.Application, "account/logout", HttpMethod.Post, null, options);

			// TODO: Stuff in the await which invalidates the session on the user param obj
			logout.ContinueWith(result => {
				if (result.IsCompleted)
				{
					// erase the session token
					user.Session = string.Empty;
					user.SessionExpires = DateTime.MinValue;
				}
			}).Wait(); // ensures the continue with happens before user defined things
			
			return logout;
		}

		/// <summary>
		/// You may submit requests to change user passwords through the API. 
		/// Use this method instead of password reset if the user knows their current password and simply wishes to change it. 
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

		/// <summary>
		/// You can use this endpoint to reset user passwords. Use this if the user has 
		/// forgotten their password and needs to securely reset it. This requires two API 
		/// requests: one to request a password reset for the user (which sends them a 
		/// password reset email), and another to fulfill that request and submit the new password.
		/// </summary>
		/// <returns>The password request.</returns>
		/// <param name="email">Email address of the user.</param>
		public Task<CMResponse> ResetPasswordRequest(string email)
		{
			Dictionary<string, string> data = new Dictionary<string, string>();
			data["email"] = email;

			return APIService.Request(this.Application, "account/password/reset", HttpMethod.Post, CMSerializer.ToStream(data), null);
		}

		/// <summary>
		/// Resets the password. This request is used to fulfill the password reset once a reset token has been created.
		/// Custom password reset form can be created on the CloudMine dashboard to deep link the token to the app.
		/// Otherwise a default email form is presented to the user.
		/// </summary>
		/// <returns>The password.</returns>
		/// <param name="token">The token sent in the email which is used to set the new password.</param>
		/// <param name="newPassword">New password.</param>
		public Task<CMResponse> ResetPassword(string token, string newPassword)
		{
			Dictionary<string, string> data = new Dictionary<string, string>();
			data["password"] = newPassword;

			return APIService.Request(this.Application, "account/password/reset/" + token, HttpMethod.Post, CMSerializer.ToStream(data), null);
		}

		#endregion Session Management

		#region Get

		// Get ==============
		/// <summary>
		/// Gets an object of type CMObject by (key,__id__) and returns that Type
		/// automatically parsed into the proper type in the Success field of the results.
		/// </summary>
		/// <returns>The user object.</returns>
		/// <param name="user">User with session to use in the request.</param>
		/// <param name="key">key,__id__,ID</param>
		/// <param name="opts">Optional Request parameters for things like post execution snippet params.</param>
		/// <typeparam name="T">a Type which derives from CMObject. Used to parse results</typeparam>
		public Task<CMObjectFetchResponse<T>> GetUserObject<T>(CMUser user, string key, CMRequestOptions opts = null) where T : CMObject
		{
			if (opts == null) opts = new CMRequestOptions(user);
			if (key != null) opts.Query["keys"] = key;

			return APIService.Request<CMObjectFetchResponse<T>>(this.Application, "user/text", HttpMethod.Get, null, opts);
		}

		/// <summary>
		///  Gets objects of type CMObject by (key,__id__) and returns that Type
		/// </summary>
		/// <returns>The user objects.</returns>
		/// <param name="user">User with session to use in the request.</param>
		/// <param name="keys">Collection of key,__id__,ID</param>
		/// <param name="opts">Optional Request parameters for things like post execution snippet params.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public Task<CMObjectFetchResponse<T>> GetUserObjects<T>(CMUser user, List<string> keys, CMRequestOptions opts = null) where T : CMObject
		{
			return GetUserObjects<T>(user, keys.ToArray(), opts);
		}

		/// <summary>
		/// Gets the user objects.
		/// </summary>
		/// <returns>The user objects.</returns>
		/// <param name="user">User with session to use in the request.</param>
		/// <param name="keys">Collection of key,__id__,ID</param>
		/// <param name="opts">Optional Request parameters for things like post execution snippet params.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public Task<CMObjectFetchResponse<T>> GetUserObjects<T>(CMUser user, string[] keys, CMRequestOptions opts = null) where T : CMObject
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
		/// <summary>
		/// Set executes a PUT request on the object being passed in. PUT requests will create a new object or replace an prior existing object if the
		/// key (__id__, ID) already exists.
		/// </summary>
		/// <typeparam name="T">Any object which derives from CMObject. CMObject auto declares and creates unique identifiers and the class type</typeparam>
		/// <param name="data">CMObject to be created at the server</param>
		/// <param name="user">the user for which this object will be created</param>
		/// <param name="opts">Optional Request parameters for things like post execution snippet params.</param>
		public Task<CMObjectResponse> SetUserObject<T>(CMUser user, T data, CMRequestOptions opts = null) where T : CMObject
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
		/// <summary>
		/// Updates the user object. The values posted in this request are merged with existing values on the server.
		/// If the key you are creating already exists, isn't a simple value (such as a string or number), 
		/// and the new value you send for it also isn't a simple value, its contents will be merged with 
		/// the data you send. Otherwise the contents will be replaced. If the key does not exist, the 
		/// entry will be created.
		/// </summary>
		/// <returns>The user object.</returns>
		/// <param name="key">Key, __id__ where the data will be indexed</param>
		/// <param name="value">CMObject to be uploaded</param>
		/// <param name="user">User which contains session where the data will reside.</param>
		/// <param name="opts">Optional Request parameters for things like post execution snippet params..</param>
		public Task<CMObjectResponse> UpdateUserObject(CMUser user, string key, object value, CMRequestOptions opts = null)
		{
			Dictionary<string, object> dataDict = new Dictionary<string, object>();
			dataDict[key] = value;

			return APIService.Request<CMObjectResponse>(this.Application, "user/text", HttpMethod.Post, CMSerializer.ToStream(dataDict), new CMRequestOptions(opts, user));
		}

		/// <summary>
		/// Updates the user object. The values posted in this request are merged with existing values on the server.
		/// If the key you are creating already exists, isn't a simple value (such as a string or number), 
		/// and the new value you send for it also isn't a simple value, its contents will be merged with 
		/// the data you send. Otherwise the contents will be replaced. If the key does not exist, the 
		/// entry will be created.
		/// </summary>
		/// <returns>The user object.</returns>
		/// <param name="data">CMObject to be uploaded</param>
		/// <param name="user">User which contains session where the data will reside.</param>
		/// <param name="opts">Optional Request parameters for things like post execution snippet params.</param>
		/// <typeparam name="T">Objects must derive from the CMObject class which ensures proper configuration.</typeparam>
		public Task<CMObjectResponse> UpdateUserObject<T>(CMUser user, T data, CMRequestOptions opts = null) where T : CMObject
		{
			Dictionary<string, T> dataDict = new Dictionary<string, T>();
			dataDict[data.ID] = data;

			return APIService.Request<CMObjectResponse>(this.Application, "user/text", HttpMethod.Post, CMSerializer.ToStream(dataDict), new CMRequestOptions(opts, user));
		}

		#endregion Update

		#region Delete

		// Delete ============
		/// <summary>
		/// Deletes the user object. Deletes object that match specified key. If no key is specified, no action will be taken unless 
		/// the all=true parameter is specified in the URL. If that is specified, all data will be deleted. 
		/// The purpose of this extra parameter is to avoid accidental total data deletion.
		/// </summary>
		/// <returns>The user object.</returns>
		/// <param name="key">Key for the user object to be deleted. Also known as ID, __id__</param>
		/// <param name="user">User.</param>
		/// <param name="opts">Optional Request parameters for things like post execution snippet params.</param>
		public Task<CMObjectResponse> DeleteUserObject(CMUser user, string key,CMRequestOptions opts = null)
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

		/// <summary>
		/// Deletes the user objects. Deletes objects that match specified keys. If no keys are specified, no action will be taken unless 
		/// the all=true parameter is specified in the URL. If that is specified, all data will be deleted. 
		/// The purpose of this extra parameter is to avoid accidental total data deletion.
		/// </summary>
		/// <returns>The user objects.</returns>
		/// <param name="keys">Keys for the user objects to be deleted. Also known as ID, __id__</param>
		/// <param name="user">User.</param>
		/// <param name="opts">Optional Request parameters for things like post execution snippet params.</param>
		public Task<CMObjectResponse> DeleteUserObjects(CMUser user, string[] keys, CMRequestOptions opts = null)
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

		/// <summary>
		/// Deletes all user objects. Deletes objects that match specified keys. If no keys are specified, no action will be taken unless 
		/// the all=true parameter is specified in the URL. If that is specified, all data will be deleted. 
		/// The purpose of this extra parameter is to avoid accidental total data deletion.
		/// </summary>
		/// <returns>The all user objects.</returns>
		/// <param name="user">User.</param>
		/// <param name="opts">Optional Request parameters for things like post execution snippet params.</param>
		public Task<CMObjectResponse> DeleteAllUserObjects(CMUser user, CMRequestOptions opts = null)
		{
			if (opts == null)
				opts = new CMRequestOptions ();
			
			opts.Query["all"] = true.ToString();
			return APIService.Request<CMObjectResponse>(this.Application, "user/text", HttpMethod.Delete, null, new CMRequestOptions(opts, user));
		}

		#endregion Delete

		#region Search
		// Search =============
		/// <summary>
		/// Performs a search query based on the string query and ensures the type of the query results
		/// are of the generic type parameter.
		/// </summary>
		/// <typeparam name="T">Becomes the __class__ parameter of the query. Uses Type name as the value</typeparam>
		/// <param name="query">String query for CloudMine search. Please reference dos at: https://cloudmine.io/docs/api#query_syntax </param>
		/// <param name="user"></param>
		/// <param name="opts"></param>
		public Task<CMObjectSearchResponse<T>> SearchUserObjects<T>(CMUser user, string query, CMRequestOptions opts = null) where T : CMObject
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
		public Task<CMFileResponse> Upload(CMUser user, string key, Stream data, CMRequestOptions opts)
		{
			if (opts == null) {
				opts = new CMRequestOptions ();
				opts.ContentType = "application/octet-stream";
			}

			if (!string.IsNullOrEmpty(user.Session))
			{
				opts.Headers["X-CloudMine-SessionToken"] = user.Session;
				opts.SnippetParams.Add("session_token", user.Session ?? string.Empty);
				opts.SnippetParams.Add("user_id", user.UserID ?? string.Empty);
			}

			return APIService.Request<CMFileResponse>(this.Application, "user/binary/" + key, HttpMethod.Put, data, new CMRequestOptions(opts, user));
		}

		// Download file ========
		public Task<CMFileResponse> Download(CMUser user, string key, CMRequestOptions opts)
		{
			if (opts == null) {
				opts = new CMRequestOptions ();
				opts.ContentType = "application/octet-stream";
			}
			
			if (!string.IsNullOrEmpty(user.Session))
			{
				opts.Headers["X-CloudMine-SessionToken"] = user.Session;
				opts.SnippetParams.Add("session_token", user.Session ?? string.Empty);
				opts.SnippetParams.Add("user_id", user.UserID ?? string.Empty);
			}

			return APIService.Request<CMFileResponse>(this.Application, "user/binary/" + key, HttpMethod.Get, null, new CMRequestOptions(opts, user));
		}

		#endregion File

		public Task<CMResponse> ListUsers(CMRequestOptions opts = null)
		{
			if (opts == null)
				opts = new CMRequestOptions();
			
			return APIService.Request<CMResponse>(this.Application, "appid/account/", HttpMethod.Get, null, opts);
		}


		/// <summary>
		/// Searchs the user profiles of an application given a CloudMine search query string.
		/// </summary>
		/// <returns>The users.</returns>
		/// <param name="user">User.</param>
		/// <param name="query">Query.</param>
		/// <param name="opts">Any custom options for the request such as snippet execution on upload completion.</param>
		public Task<CMResponse> SearchUsers(CMUser user, string query, CMRequestOptions opts = null)
		{
			if (opts == null)
				opts = new CMRequestOptions();

			if (string.IsNullOrEmpty(opts.Query["q"]))
				opts.Query["q"] = query;

			return APIService.Request<CMResponse>(this.Application, "account/search/", HttpMethod.Get, null, new CMRequestOptions(opts, user));
		}

		/// <summary>
		/// Returns the current user profile. Good to use when you have a cached session token 
		/// but not the latest instance of the profile object.
		/// </summary>
		/// <returns>The user profile.</returns>
		/// <param name="user">Instance of CMUser with a valid session token</param>
		/// <param name="opts">Any custom options for the request such as snippet execution on upload completion.</param>
		/// <typeparam name="T">CMUserprofile type derivative</typeparam>
		public Task<CMUserResponse<T>> CurrentUserProfile<T>(CMUser<T> user, CMRequestOptions opts = null) where T : CMUserProfile
		{
			if (opts == null)
				opts = new CMRequestOptions();
			
			if (!string.IsNullOrEmpty(user.Session))
			{
				opts.Headers["X-CloudMine-SessionToken"] = user.Session;
				opts.SnippetParams.Add("session_token", user.Session ?? string.Empty);
				opts.SnippetParams.Add("user_id", user.UserID ?? string.Empty);
			}

			return APIService.Request<CMUserResponse<T>>(this.Application, "account/mine/", HttpMethod.Get, null, new CMRequestOptions(opts, user));
		}

		/// <summary>
		/// Replaces the user profile via a PUT call.
		/// </summary>
		/// <returns>The user profile.</returns>
		/// <param name="user">User containg the profile to be replaced with valid session token.</param>
		/// <param name="opts">Any custom options for the request such as snippet execution on upload completion.</param>
		/// <typeparam name="T">CMUserprofile type derivative</typeparam>
		public Task<CMResponse> UpdateUserProfile<T>(CMUser<T> user, CMRequestOptions opts = null) where T : CMUserProfile
		{
			if (opts == null)
				opts = new CMRequestOptions();

			if (!string.IsNullOrEmpty(user.Session))
			{
				opts.Headers["X-CloudMine-SessionToken"] = user.Session;
				opts.SnippetParams.Add("session_token", user.Session ?? string.Empty);
				opts.SnippetParams.Add("user_id", user.UserID ?? string.Empty);
			}
				
			return APIService.Request<CMResponse>(this.Application, "account/", HttpMethod.Put, CMSerializer.ToStream(user.Profile), new CMRequestOptions(opts, user));
		}

		/// <summary>
		/// Merges the user profile via a POST call.
		/// </summary>
		/// <returns>The user profile.</returns>
		/// <param name="user">User containg the profile to be replaced with valid session token.</param>
		/// <param name="opts">Any custom options for the request such as snippet execution on upload completion.</param>
		/// <typeparam name="T">CMUserprofile type derivative</typeparam>
		public Task<CMResponse> MergeUserProfile<T>(CMUser<T> user, CMRequestOptions opts = null) where T : CMUserProfile
		{
			if (opts == null)
				opts = new CMRequestOptions();

			if (!string.IsNullOrEmpty(user.Session))
			{
				opts.Headers["X-CloudMine-SessionToken"] = user.Session;
				opts.SnippetParams.Add("session_token", user.Session ?? string.Empty);
				opts.SnippetParams.Add("user_id", user.UserID ?? string.Empty);
			}

			return APIService.Request<CMResponse>(this.Application, "account/", HttpMethod.Post, CMSerializer.ToStream(user.Profile), new CMRequestOptions(opts, user));
		}
	}
}