using CloudmineSDK.Model;
using CloudMineSDK.Scripts.Model.Responses;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Net;

namespace NetSDKPrivate.Scripts.Model.Responses
{
    public class CMUserResponse : CMJSONResponse
    {
        public CMUser<CMUserProfile> CMUser { get; private set; }

		private string[] errors;
        public string[] Errors { 
			get 
			{
				return errors ?? new string[] { };
			}
			private set { errors = value; }
		}

        public override bool HasErrors
        {
            get
            {
                if (Errors == null)
                    return false;
                return this.Errors.Length > 0;
            }
        }

        public CMUserResponse()
        {
            
        }

		public CMUserResponse(HttpStatusCode code, Stream dataStream)
            : base(code, dataStream)
        {
            ExtractUser();
        }

        public CMUserResponse(CMResponse response)
            : base(response)
        {
            ExtractUser();
        }

		public override void Initialize(HttpStatusCode code, Stream dataStream)
        {
            base.Initialize(code, dataStream);
            ExtractUser();
        }

		void ExtractUser()
		{
			Errors = ExtractKey<string[]>("errors");
			if (!HasErrors)
			{
				CMUser = RawObject.ToObject<CMUser<CMUserProfile>>();

				if (RawObject["profile"] != null)
					CMUser.Profile = RawObject["profile"].ToObject<CMUserProfile>();

				// Because user id can come from different places depending on create, search, login
				if (!string.IsNullOrEmpty(CMUser.UserID)
					&& CMUser.Profile != null
					&& string.IsNullOrEmpty(CMUser.Profile.UserID))
					CMUser.Profile.UserID = CMUser.UserID;
				else if (CMUser.Profile != null
					&& !string.IsNullOrEmpty(CMUser.Profile.UserID)
					&& string.IsNullOrEmpty(CMUser.UserID))
					CMUser.UserID = CMUser.Profile.UserID;
			}
			else
			{
				CMUser = null;
			}
		}
    }

    public class CMUserResponse<T> : CMJSONResponse where T: CMUserProfile
    {
        public CMUser<T> CMUser { get; private set; }

		private string[] errors;
		public string[] Errors
		{
			get
			{
				return errors ?? new string[] { };
			}
			private set { errors = value; }
		}

        public override bool HasErrors
        {
            get
            {
                if (Errors == null)
                    return false;
                return this.Errors.Length > 0;
            }
        }

        public CMUserResponse()
        {
        }

		public CMUserResponse(HttpStatusCode code, Stream dataStream)
            : base(code, dataStream)
        {
            ExtractUser();
        }

        public CMUserResponse(CMResponse response)
            : base(response)
        {
            ExtractUser();
        }

		public override void Initialize(HttpStatusCode code, Stream dataStream)
        {
            base.Initialize(code, dataStream);
            ExtractUser();
        }


        void ExtractUser()
        {
            Errors = ExtractKey<string[]>("errors");
            if (!HasErrors)
            {
                CMUser = RawObject.ToObject<CMUser<T>>();

				if (RawObject["profile"] != null)
					CMUser.Profile = RawObject["profile"].ToObject<T>();

				// Because user id can come from different places depending on create, search, login
				if (!string.IsNullOrEmpty(CMUser.UserID)
					&& CMUser.Profile != null
					&& string.IsNullOrEmpty(CMUser.Profile.UserID))
					CMUser.Profile.UserID = CMUser.UserID;
				else if (CMUser.Profile != null
					&& !string.IsNullOrEmpty(CMUser.Profile.UserID)
					&& string.IsNullOrEmpty(CMUser.UserID))
					CMUser.UserID = CMUser.Profile.UserID;
            }
            else
            {
                CMUser = null;
            }
        }
    }
}
