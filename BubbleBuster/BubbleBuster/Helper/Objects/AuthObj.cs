using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BubbleBuster.Helper.Objects
{
    public class AuthObj
    {
        public enum AuthType { User, App };

        public AuthObj(string OAuthToken, string OAuthTokenSecret, string Name, string RequesterName)
        {
            UUID = Guid.NewGuid();
            this.OAuthToken = OAuthToken;
            this.OAuthTokenSecret = OAuthTokenSecret;
            this.Name = Name;
            this.RequesterName = RequesterName;
            Type = AuthType.User;
        }

        public AuthObj(string apiKey = Constants.APP_API_CREDS)
        {
            UUID = Guid.NewGuid();
            APIKey = apiKey;
            RequesterName = "FilterBubble_SW709";
            Type = AuthType.App;
        }

        public string APIKey { get; private set; }

        public AuthType Type { get; private set; }

        public Guid UUID { get; private set; }

        public string Name { get; private set; }

        public string RequesterName { get; private set; }

        public string OAuthToken { get; private set; }

        public string OAuthTokenSecret { get; private set; }

        public override bool Equals(object obj)
        {
            if(obj is AuthObj)
            {
                return (obj as AuthObj).UUID == this.UUID;
            }
            else
            {
                return false;
            }
        }

        public override int GetHashCode()
        {
            return UUID.GetHashCode();
        }
    }
}
