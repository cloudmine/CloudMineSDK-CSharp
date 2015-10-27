using CloudmineSDK.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace CloudmineSDK.Model
{
	/// <summary>
	/// TODO: Allow endpoint configuration in the application definition.
	/// Gets used by user, object, and api services
	/// </summary>
	public class CMApplication
	{
		private string applicationID;
		private string applicationKey;
		private string applicationName;

		/// <summary>
		/// Denotes the API version to hit. Certain versions of the 
		/// API might have different return objects. 
		/// </summary>
		/// <value>The API version.</value>
		public string APIVersion { get; set; }

		public CMApplication(string appID, string apiKey)
		{
			this.applicationID = appID;
			this.applicationKey = apiKey;
			this.applicationName = string.Empty;

			APIVersion = "v1";
		}

		public CMApplication(string appID, string apiKey, string appName, string appVersion)
		{
			this.applicationID = appID;
			this.applicationKey = apiKey;
			this.applicationName = appName.Replace('/', '_') + "/" + appVersion.Replace('/', '_');

			APIVersion = "v1";
		}

		public string ApplicationName
		{
			get { return this.applicationName; }
			set
			{
				this.applicationName = value;
			}
		}

		public string ApplicationID
		{
			get { return this.applicationID; }
		}

		internal string APIKey
		{
			get { return this.applicationKey; }
		}
	}
}
