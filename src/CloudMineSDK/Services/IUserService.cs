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
		Task<CMUserResponse> Create(CMUser user, CMRequestOptions opts = null);

		Task<CMObjectResponse> DeleteAllUserObjects(CMUser user, CMRequestOptions opts = null);
		Task<CMObjectResponse> DeleteUserObject(string key, CMUser user, CMRequestOptions opts = null);
		Task<CMObjectResponse> DeleteUserObjects(string[] keys, CMUser user, CMRequestOptions opts = null);

		Task<CMObjectFetchResponse<T>> GetUserObject<T>(CMUser user, string key, CMRequestOptions opts = null) where T : CMObject;
		Task<CMObjectFetchResponse<T>> GetUserObjects<T>(CMUser user, List<string> keys, CMRequestOptions opts = null) where T : CMObject;
		Task<CMObjectFetchResponse<T>> GetUserObjects<T>(CMUser user, string[] keys, CMRequestOptions opts = null) where T : CMObject;

		Task<CMUserResponse> Login(CMUser user, CMRequestOptions options = null);
		Task<CMUserResponse<T>> Login<T>(CMUser<T> user, CMRequestOptions options = null) where T : CMUserProfile;
		Task<CMLogoutResponse> Logoff(CMUser user, CMRequestOptions options = null);

		Task<CMResponse> ChangePassword (CMUser user, string newPassword);
		Task<CMResponse> ResetPassword(string token, string newPassword);
		Task<CMResponse> ResetPasswordRequest(string email);

		Task<CMObjectSearchResponse<T>> SearchUserObjects<T>(CMUser user, string query, CMRequestOptions opts = null) where T : CMObject;

		Task<CMObjectResponse> SetUserObject<T>(T data, CMUser user, CMRequestOptions opts = null) where T : CMObject;

		Task<CMObjectResponse> UpdateUserObject(string key, object value, CMUser user, CMRequestOptions opts = null);
		Task<CMObjectResponse> UpdateUserObject<T>(T data, CMUser user, CMRequestOptions opts = null) where T : CMObject;

		Task<CMFileResponse> Upload(string key, System.IO.Stream data, CMUser user, CMRequestOptions opts);
		Task<CMFileResponse> Download(string key, CMUser user, CMRequestOptions opts);

		Task<CMResponse> ListUsers (CMRequestOptions opts = null);
		Task<CMResponse> SearchUsers (CMUser user, string query, CMRequestOptions opts = null);
		Task<CMUserResponse<T>> CurrentUserProfile<T> (CMUser<T> user, CMRequestOptions opts = null) where T : CMUserProfile;
		Task<CMResponse> UpdateUserProfile<T> (CMUser<T> user, CMRequestOptions opts = null) where T : CMUserProfile;
		Task<CMResponse> MergeUserProfile<T> (CMUser<T> user, CMRequestOptions opts = null) where T : CMUserProfile;
	}
}
