using CloudmineSDK.Model;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace CloudmineSDK.Services
{
    public interface IRestWrapper
    {
		Task<T> GenerateCMResponseObject<T>(HttpResponseMessage response) where T : CMResponse, new();

        List<string> GetCloudMineQuery(CMRequestOptions opts);

		Task<CMResponse> Request(CMApplication app, string action, HttpMethod method, System.IO.Stream content, CMRequestOptions options);

		Task<T> Request<T>(CMApplication app, string action, HttpMethod method, System.IO.Stream content, CMRequestOptions options) where T: CMResponse, new();
    }
}
