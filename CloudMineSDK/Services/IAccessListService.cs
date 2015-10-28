using System.Threading.Tasks;
using CloudmineSDK.Model;
using CloudMineSDK.Model;

namespace CloudMineSDK.Services
{
	public interface IAccessListService
	{
		Task<CMResponse> CreateAccessList(CMUser user, CMAccessList acl);
		Task<CMResponse> ModifyAccessList(CMUser user, CMAccessList acl);
	}
}
