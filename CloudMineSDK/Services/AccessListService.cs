using CloudmineSDK.Model;
using CloudmineSDK.Services;
using CloudMineSDK.Scripts.Model;
using System.Threading.Tasks;
using System.Net.Http;

namespace CloudMineSDK.Scripts.Services
{
	/// <summary>
	/// New access lists can be constructed by making a PUT or POST request to the access list endpoint.
	/// </summary>
	public class AccessListService: IAccessListService
	{
		private CMApplication Application { get; set; }
		private IRestWrapper APIService { get; set; }

		public AccessListService(CMApplication application, IRestWrapper apiService)
		{
			Application = application;
			APIService = apiService;
		}
					
		public Task<CMResponse> CreateAccessList(CMUser user, CMAccessList acl)
		{
			return APIService.Request (this.Application, "user/access", HttpMethod.Post, CMSerializer.ToStream (acl), new CMRequestOptions (null, user));
		}

		public Task<CMResponse> ModifyAccessList(CMUser user, CMAccessList acl)
		{
			return APIService.Request (this.Application, "user/access", HttpMethod.Put, CMSerializer.ToStream (acl), new CMRequestOptions (null, user));
		}
	}
}
