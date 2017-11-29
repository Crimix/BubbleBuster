using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BubbleBuster.Helper.Objects
{
    public class AuthObj
    {
        public AuthObj(string OAuthToken, string OAuthTokenSecret, string Name, string RequesterName)
        {
            UUID = Guid.NewGuid();
            this.OAuthToken = OAuthToken;
            this.OAuthTokenSecret = OAuthTokenSecret;
            this.Name = Name;
            this.RequesterName = RequesterName;
        }

        public Guid UUID { get; private set; }

        public String Name { get; private set; }

        public String RequesterName { get; private set; }

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
