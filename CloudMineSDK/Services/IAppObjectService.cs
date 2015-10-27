using System;
using System.Net.Http;
using CloudMineSDK.Scripts.Model.Responses;
using CloudmineSDK.Model;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace CloudMineSDK.Scripts.Services
{
	public interface IAppObjectService
	{
		Task<CMObjectResponse> DeleteAllObjects(CMRequestOptions opts = null);
		Task<CMObjectResponse> DeleteObject(string key, CMRequestOptions opts = null);
		Task<CMObjectResponse> DeleteObject<T>(T data, CMRequestOptions opts = null) where T : CMObject;
		Task<CMObjectResponse> DeleteObjects(string[] keys, CMRequestOptions opts = null);
		Task<CMObjectResponse> DeleteObjects<T>(List<T> data, CMRequestOptions opts = null) where T : CMObject;

		void Download(string key, CMRequestOptions opts = null);

		Task<CMObjectFetchResponse> GetObject(string key = null, CMRequestOptions opts = null);
		Task<CMObjectFetchResponse<T>> GetObject<T>(string key = null, CMRequestOptions opts = null) where T : CMObject;
		Task<CMObjectFetchResponse> GetObjects(List<string> keys, CMRequestOptions opts = null);
		Task<CMObjectFetchResponse> GetObjects(string[] keys, CMRequestOptions opts = null);
		Task<CMObjectFetchResponse<T>> GetObjects<T>(string[] keys, CMRequestOptions opts = null) where T : CMObject;

		void Run(string snippet, HttpMethod method, Dictionary<string, string> parameters = null, CMRequestOptions opts = null);

		Task<CMObjectSearchResponse> SearchObjects(string query, CMRequestOptions opts = null);
		Task<CMObjectSearchResponse<T>> SearchObjects<T>(string query, CMRequestOptions opts = null) where T : CMObject;

		Task<CMObjectResponse> SetObject(object data, CMRequestOptions opts = null);
		Task<CMObjectResponse> SetObject(object value, CMRequestOptions opts = null, string key = null, string type = null);
		Task<CMObjectResponse> SetObject<T>(T data, CMRequestOptions opts = null) where T : CMObject;

		Task<CMObjectResponse> UpdateObject(object data, CMRequestOptions opts = null);
		Task<CMObjectResponse> UpdateObject(string key, object value, CMRequestOptions opts = null);
		Task<CMObjectResponse> UpdateObject<T>(T data, CMRequestOptions opts = null) where T : CMObject;

		void Upload(string key, Stream data, CMRequestOptions opts = null);
	}
}
