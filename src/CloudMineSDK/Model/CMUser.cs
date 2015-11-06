using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CloudmineSDK.Model
{
	[JsonObject(MemberSerialization.OptIn)]
	public class CMUser
	{
		/// <summary>
		/// SDK not parsing the ID field coming back from the server.
		/// </summary>
		[JsonProperty("__id__")]
		public string UserID { get; set; }
		[JsonProperty("__type__")]
		public string Type = "user";
		[JsonProperty("session_token")]
		public string Session { get; set; }
		[JsonProperty("expires")]
		public DateTime SessionExpires { get; set; }
		[JsonProperty("credentials")]
		public CMCredentials Credentials { get; private set; }
		[JsonProperty("profile")]
		public CMUserProfile Profile { get; set; } // should be made abstract

		public CMUser()
		{
			Credentials = new CMCredentials();
			Profile = new CMUserProfile();
		}

		public CMUser(string username, string email, string password)
		{
			SetCredentials(new CMCredentials(username, email, password));
		}

		public CMUser(string username, string password, string email, string session)
			: this(username, email, password)
		{
			if (!string.IsNullOrEmpty(session))
			{
				this.Session = session;
			}
		}

		public void SetCredentials(CMCredentials credentials)
		{
			this.Credentials = credentials;
		}

		public bool LoggedIn
		{
			get { return !string.IsNullOrEmpty(this.Session) && (DateTime.Now < SessionExpires); }
		}
	}


	[JsonObject(MemberSerialization.OptIn)]
	public class CMUser<T>: CMUser where T: CMUserProfile
	{
		[JsonProperty("profile")]
		new public T Profile { get; set; } // should be made abstract

		public CMUser()
		{
		}

		public CMUser(T cmUserProfile)
		{
			Profile = cmUserProfile;
		}

		public CMUser(string username, string email, string password)
		{
			SetCredentials(new CMCredentials(username, email, password));
		}

		public CMUser(string username, string email, string password, T cmUserProfile)
		{
			SetCredentials(new CMCredentials(username, email, password));
			Profile = cmUserProfile;
		}

		public CMUser(string username, string password, string email, string session, T cmUserProfile)
		{
			SetCredentials(new CMCredentials(username, email, password));
			Session = session;
			Profile = cmUserProfile;
		}
	}
}	
