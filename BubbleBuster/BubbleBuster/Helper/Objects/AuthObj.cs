using System;

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
        /// <param name="SearchedForUser">The user's name</param>
        public AuthObj(string OAuthToken, string OAuthTokenSecret, string SearchedForUser, string RequestID, string Name)
        {
            this.Name = Name;
            this.OAuthToken = OAuthToken;
            this.OAuthTokenSecret = OAuthTokenSecret;
            this.SearchedForUser = SearchedForUser;
            this.RequestID = RequestID;
            Type = AuthType.User;
        }

        /// <summary>
        /// Creates an app auth object.
        /// Has Optional parameter.
        /// </summary>
        /// <param name="apiKey">The api key. OPTIONAL</param>
        public AuthObj(string apiKey = Constants.APP_API_CREDS)
        {
            Name = "FilterBubble_SW709";
            APIKey = apiKey;
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
        /// The name of the user who requested the request
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// The name of the user, who should be analysed
        /// </summary>
        public string SearchedForUser { get; private set; }

        /// <summary>
        /// The OAuthToken
        /// </summary>
        public string OAuthToken { get; private set; }

        /// <summary>
        /// The OAuthTokenSecret
        /// </summary>
        public string OAuthTokenSecret { get; private set; }

        /// <summary>
        /// The id of the request, assigned by the GUI application
        /// </summary>
        public string RequestID { get; private set; } = "";

        /// <summary>
        /// Overridden, such that it uses the Name to compare objects 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (obj is AuthObj)
            {
                return (obj as AuthObj).Name == Name;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Overridden, such that it uses the hashcode of the Name
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }
    }
}
