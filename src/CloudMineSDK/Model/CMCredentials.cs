using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;

namespace CloudmineSDK.Model
{
	public class CMCredentials
	{
		[JsonProperty("username", NullValueHandling = NullValueHandling.Ignore)]
		public string Username { get; set; }
		[JsonProperty("email", NullValueHandling = NullValueHandling.Ignore)]
		public string Email { get; set; }
		[JsonProperty("password")]
		public string Password { get; set; }

		public CMCredentials()
		{
			Username = string.Empty;
			Password = string.Empty;
			Email = string.Empty;
		}

		public CMCredentials(string username, string email, string password) {
			this.Username = username;
			this.Password = password;
			this.Email = email;
		}
	}
}
