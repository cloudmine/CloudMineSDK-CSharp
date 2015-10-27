using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CloudMineSDK.Scripts.Model
{
	/// <summary>
	/// Each access list belongs to a single user and may only be accessed by that user.
	/// And access list can be appended to objects created by another user as a way to
	/// share data with a pre-defined list of members such as an administration list.
	/// 
	/// Note: CMAccessList can be subclassed with additional opt-in JSON data. Often
	/// used to add descriptive fields regarding the usage intention od the ACL such
	/// as description: "This is the admin ACL which grants read access on all objects"
	/// </summary>
	public class CMAccessList
	{
		[JsonProperty("__id__")]
		public string ID { get; set; }
		[JsonProperty("__type__")]
		private const string Type = "acl";
		/// <summary>
		/// User IDs which are associated with the permissions granted through this ACL.
		/// </summary>
		[JsonProperty("members")]
		public string[] Members { get; set; }
		[JsonProperty("permissions")]
		public CMAccessListPermissions[] Permissions { get; set; }
		[JsonProperty("segments")]
		public CMAccessListSegments Segments { get; set; }

		public CMAccessList()
		{
			ID = new Guid().ToString().Replace("-", string.Empty);
			Members = new string[] { };
			Permissions = new CMAccessListPermissions[] { CMAccessListPermissions.r };
		}
	}

	/// <summary>
	/// Instead of specifying a list of specific members, access lists may be marked as either 
	/// "public" (open access, no authentication required) or "logged_in" (any authenticated user). 
	/// These fields are specified in the "segments" member on the access list.
	/// </summary>
	public class CMAccessListSegments
	{
		/// <summary>
		/// Denotes open access on a user object. Often used in a read only state so a user object
		/// is visible to the rest of the application.
		/// </summary>
		[JsonProperty("public")]
		public bool Public { get; set; }
		/// <summary>
		/// Denotes that it is required for the user to be logged in to gain access to the access.
		/// appended item.
		/// </summary>
		[JsonProperty("logged_in")]
		public bool LoggedIn { get; set; }

		/// <summary>
		/// Default constructor which sets "public" and "logged_in" to false.
		/// </summary>
		public CMAccessListSegments()
		{
			Public = false;
			LoggedIn = false;
		}
	}

	/// <summary>
	/// Operators of permissions to be associated with Access lists.
	/// c - Create
	/// r - Read
	/// u - Update
	/// d - Delete
	/// </summary>
	public enum CMAccessListPermissions
	{
		c, r, u, d
	}
}
