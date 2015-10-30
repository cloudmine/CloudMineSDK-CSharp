using CloudmineSDK.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NetSDKPrivate.Model.Responses;
using CloudMineSDK.Model.Responses;

namespace CloudMineSDK.Services
{
	public interface IUserService
	{
		Task<CMResponse> DeleteUser(CMUser user);
		Task<CMUserResponse> Create(CMUser user, CMRequestOptions opts);

		// Theses are the not yet implemented interface methods. Haven't decided the proper return shape yet
		void CurrentUserProfile();
		void UpdateUserProfile();
		void ListUsers();
		void SearchUsers();

		Task<CMObjectResponse> DeleteAllUserObjects(CMUser user, CMRequestOptions opts);
		Task<CMObjectResponse> DeleteUserObject(string key, CMUser user, CMRequestOptions opts);
		Task<CMObjectResponse> DeleteUserObject<T>(T data, CMUser user, CMRequestOptions opts) where T : CMObject;
		Task<CMObjectResponse> DeleteUserObjects(string[] keys, CMUser user, CMRequestOptions opts);
		Task<CMObjectResponse> DeleteUserObjects<T>(System.Collections.Generic.List<T> data, CMUser user, CMRequestOptions opts) where T : CMObject;

		Task<CMObjectFetchResponse<CMObject>> GetUserObject(CMUser user, string key, CMRequestOptions opts);
		Task<CMObjectFetchResponse<T>> GetUserObject<T>(CMUser user, string key, CMRequestOptions opts) where T : CMObject;
		Task<CMObjectFetchResponse<CMObject>> GetUserObjects(CMUser user, List<string> keys, CMRequestOptions opts);
		Task<CMObjectFetchResponse<CMObject>> GetUserObjects(CMUser user, string[] keys, CMRequestOptions opts);
		Task<CMObjectFetchResponse<T>> GetUserObjects<T>(CMUser user, List<string> keys, CMRequestOptions opts) where T : CMObject;
		Task<CMObjectFetchResponse<T>> GetUserObjects<T>(CMUser user, string[] keys, CMRequestOptions opts) where T : CMObject;

		Task<CMUserResponse> Login(CMUser user, CMRequestOptions options = null);
		Task<CMUserResponse> Login<T>(CMUser<T> user, CMRequestOptions options = null) where T : CMUserProfile;
		Task<CMLogoutResponse> Logoff(CMUser user, CMRequestOptions options);

		Task<CMResponse> ChangePassword (CMUser user, string newPassword);
		Task<CMResponse> ResetPassword(string token, string newPassword);
		Task<CMResponse> ResetPasswordRequest(string email);

		Task<CMObjectSearchResponse> SearchUserObjects(string query, CMUser user,CMRequestOptions opts);
		Task<CMObjectSearchResponse<T>> SearchUserObjects<T>(string query, CMUser user, CMRequestOptions opts) where T : CMObject;

		Task<CMObjectResponse> SetUserObject(object data, CMUser user, CMRequestOptions opts);
		Task<CMObjectResponse> SetUserObject(object value, CMUser user, CMRequestOptions opts = null, string key = null, string type = null);
		Task<CMObjectResponse> SetUserObject<T>(T data, CMUser user, CMRequestOptions opts) where T : CMObject;

		Task<CMObjectResponse> UpdateUserObject(object data, CMUser user, CMRequestOptions opts);
		Task<CMObjectResponse> UpdateUserObject(string key, object value, CMUser user, CMRequestOptions opts);
		Task<CMObjectResponse> UpdateUserObject<T>(T data, CMUser user, CMRequestOptions opts) where T : CMObject;

		Task<CMResponse> Upload(string key, System.IO.Stream data, CMUser user, CMRequestOptions opts);
		Task<CMFileResponse> Download(string key, CMUser user, CMRequestOptions opts);
	}
}
