using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BubbleBuster.Helper.Objects
{
    public class AuthObj
    {
        /// <summary>
        /// To tell which type of auth it is
        /// </summary>
        public enum AuthType { User, App };

        /// <summary>
        /// Creates an user auth object.
        /// </summary>
        /// <param name="OAuthToken">The user's OAuthToken</param>
        /// <param name="OAuthTokenSecret">The user's OAuthTokenSecret</param>
        /// <param name="Name">The user's name</param>
        public AuthObj(string OAuthToken, string OAuthTokenSecret, string Name)
        {
            UID = Guid.NewGuid();
            this.OAuthToken = OAuthToken;
            this.OAuthTokenSecret = OAuthTokenSecret;
            this.Name = Name;
            Type = AuthType.User;
        }

        /// <summary>
        /// Creates an app auth object.
        /// Has Optional parameter.
        /// </summary>
        /// <param name="apiKey">The api key. OPTIONAL</param>
        public AuthObj(string apiKey = Constants.APP_API_CREDS)
        {
            UID = Guid.NewGuid();
            APIKey = apiKey;
            Name = "FilterBubble_SW709";
            Type = AuthType.App;
        }

        /// <summary>
        /// The API key, only used when AuthType = App
        /// </summary>
        public string APIKey { get; private set; }

        /// <summary>
        /// The auth type
        /// </summary>
        public AuthType Type { get; private set; }

        /// <summary>
        /// The unique id of this auth object
        /// </summary>
        public Guid UID { get; private set; }

        /// <summary>
        /// The name of the user
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// The OAuthToken
        /// </summary>
        public string OAuthToken { get; private set; }

        /// <summary>
        /// The OAuthTokenSecret
        /// </summary>
        public string OAuthTokenSecret { get; private set; }

        /// <summary>
        /// Overridden, such that it uses the UID to compare objects 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if(obj is AuthObj)
            {
                return (obj as AuthObj).UID == this.UID;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Overridden, such that it uses the hashcode of the UID
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return UID.GetHashCode();
        }
    }
}
