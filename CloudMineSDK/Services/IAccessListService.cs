using System.Threading.Tasks;
using CloudmineSDK.Model;
using CloudMineSDK.Scripts.Model;

namespace CloudMineSDK.Scripts.Services
{
	public interface IAccessListService
	{
		Task<CMResponse> CreateAccessList(CMUser user, CMAccessList acl);
		Task<CMResponse> ModifyAccessList(CMUser user, CMAccessList acl);
	}
}
