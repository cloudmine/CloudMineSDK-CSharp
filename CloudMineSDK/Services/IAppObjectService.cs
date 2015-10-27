using System;
using System.Net.Http;
using CloudMineSDK.Scripts.Model.Responses;
using CloudmineSDK.Model;
using System.Collections.Generic;
using System.IO;

namespace CloudMineSDK.Scripts.Services
{
	public interface IAppObjectService
	{
		void DeleteAllObjects(CMRequestOptions opts = null);
		void DeleteObject(string key, CMRequestOptions opts = null);
		void DeleteObject<T>(T data, CMRequestOptions opts = null) where T : CMObject;
		void DeleteObjects(string[] keys, CMRequestOptions opts = null);
		void DeleteObjects<T>(List<T> data, CMRequestOptions opts = null) where T : CMObject;
		void Download(string key, Action<CMFileResponse> responseAction, CMRequestOptions opts = null);
		void GetObject(string key = null, CMRequestOptions opts = null);
		void GetObject<T>(string key = null, CMRequestOptions opts = null) where T : CMObject;
		void GetObjects(List<string> keys, CMRequestOptions opts = null);
		void GetObjects(string[] keys, CMRequestOptions opts = null);
		void GetObjects<T>(CMUser user, List<string> keys, CMRequestOptions opts = null) where T : CMObject;
		void GetObjects<T>(string[] keys, CMRequestOptions opts = null) where T : CMObject;
		void Run(string snippet, HttpMethod method, Dictionary<string, string> parameters = null, CMRequestOptions opts = null);
		void SearchObjects(string query, CMRequestOptions opts = null);
		void SearchObjects<T>(string query, CMRequestOptions opts = null) where T : CMObject;
		void SetObject(object data, CMRequestOptions opts = null);
		void SetObject(object value, CMRequestOptions opts = null, string key = null, string type = null);
		void SetObject<T>(T data, CMRequestOptions opts = null) where T : CMObject;
		void UpdateObject(object data, CMRequestOptions opts = null);
		void UpdateObject(string key, object value, CMRequestOptions opts = null);
		void UpdateObject<T>(T data, CMRequestOptions opts = null) where T : CMObject;
		void Upload(string key, Stream data, CMRequestOptions opts = null);
	}
}
