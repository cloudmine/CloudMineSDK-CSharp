using System;
using System.Net.Http;

namespace CloudMineSDK.Scripts.Services
{
	public interface IApplicationService
	{
		void DeleteAllObjects(Action<CloudMineSDK.Scripts.Model.Responses.CMObjectResponse> responseAction, CloudmineSDK.Model.CMRequestOptions opts = null);
		void DeleteObject(string key, Action<CloudMineSDK.Scripts.Model.Responses.CMObjectResponse> responseAction, CloudmineSDK.Model.CMRequestOptions opts = null);
		void DeleteObject<T>(T data, Action<CloudMineSDK.Scripts.Model.Responses.CMObjectResponse> responseAction, CloudmineSDK.Model.CMRequestOptions opts = null) where T : CloudmineSDK.Model.CMObject;
		void DeleteObjects(string[] keys, Action<CloudMineSDK.Scripts.Model.Responses.CMObjectResponse> responseAction, CloudmineSDK.Model.CMRequestOptions opts = null);
		void DeleteObjects<T>(System.Collections.Generic.List<T> data, Action<CloudMineSDK.Scripts.Model.Responses.CMObjectResponse> responseAction, CloudmineSDK.Model.CMRequestOptions opts = null) where T : CloudmineSDK.Model.CMObject;
		void Download(string key, Action<CloudMineSDK.Scripts.Model.Responses.CMFileResponse> responseAction, CloudmineSDK.Model.CMRequestOptions opts = null);
		void GetObject(Action<CloudMineSDK.Scripts.Model.Responses.CMObjectFetchResponse> responseAction, string key = null, CloudmineSDK.Model.CMRequestOptions opts = null);
		void GetObject<T>(Action<CloudMineSDK.Scripts.Model.Responses.CMObjectFetchResponse<T>> responseAction, string key = null, CloudmineSDK.Model.CMRequestOptions opts = null) where T : CloudmineSDK.Model.CMObject;
		void GetObjects(Action<CloudMineSDK.Scripts.Model.Responses.CMObjectFetchResponse> responseAction, System.Collections.Generic.List<string> keys, CloudmineSDK.Model.CMRequestOptions opts = null);
		void GetObjects(Action<CloudMineSDK.Scripts.Model.Responses.CMObjectFetchResponse> responseAction, string[] keys, CloudmineSDK.Model.CMRequestOptions opts = null);
		void GetObjects<T>(CloudmineSDK.Model.CMUser user, Action<CloudMineSDK.Scripts.Model.Responses.CMObjectFetchResponse<T>> responseAction, System.Collections.Generic.List<string> keys, CloudmineSDK.Model.CMRequestOptions opts = null) where T : CloudmineSDK.Model.CMObject;
		void GetObjects<T>(Action<CloudMineSDK.Scripts.Model.Responses.CMObjectFetchResponse<T>> responseAction, string[] keys, CloudmineSDK.Model.CMRequestOptions opts = null) where T : CloudmineSDK.Model.CMObject;
		void Run(string snippet, Action<CloudmineSDK.Model.CMResponse> responseAction, HttpMethod method, System.Collections.Generic.Dictionary<string, string> parameters = null, CloudmineSDK.Model.CMRequestOptions opts = null);
		void SearchObjects(string query, Action<CloudMineSDK.Scripts.Model.Responses.CMObjectSearchResponse> responseAction, CloudmineSDK.Model.CMRequestOptions opts = null);
		void SearchObjects<T>(string query, Action<CloudMineSDK.Scripts.Model.Responses.CMObjectSearchResponse<T>> responseAction, CloudmineSDK.Model.CMRequestOptions opts = null) where T : CloudmineSDK.Model.CMObject;
		void SetObject(object data, Action<CloudMineSDK.Scripts.Model.Responses.CMObjectResponse> responseAction, CloudmineSDK.Model.CMRequestOptions opts = null);
		void SetObject(object value, Action<CloudMineSDK.Scripts.Model.Responses.CMObjectResponse> responseAction, CloudmineSDK.Model.CMRequestOptions opts = null, string key = null, string type = null);
		void SetObject<T>(T data, Action<CloudMineSDK.Scripts.Model.Responses.CMObjectResponse> responseAction, CloudmineSDK.Model.CMRequestOptions opts = null) where T : CloudmineSDK.Model.CMObject;
		void UpdateObject(object data, Action<CloudMineSDK.Scripts.Model.Responses.CMObjectResponse> responseAction, CloudmineSDK.Model.CMRequestOptions opts = null);
		void UpdateObject(string key, object value, Action<CloudMineSDK.Scripts.Model.Responses.CMObjectResponse> responseAction, CloudmineSDK.Model.CMRequestOptions opts = null);
		void UpdateObject<T>(T data, Action<CloudMineSDK.Scripts.Model.Responses.CMObjectResponse> responseAction, CloudmineSDK.Model.CMRequestOptions opts = null) where T : CloudmineSDK.Model.CMObject;
		void Upload(string key, System.IO.Stream data, Action<CloudmineSDK.Model.CMResponse> responseAction, CloudmineSDK.Model.CMRequestOptions opts = null);
	}
}
