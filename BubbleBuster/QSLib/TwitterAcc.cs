﻿using BubbleBuster.Helper.Objects;

namespace QSLib
{
    public class TwitterAcc //Class of all information needed about a Twitter account
    {
        //Just such that an object can be made without the parameters
        public TwitterAcc()
        {
        }

        public TwitterAcc(string Token, string Name, string Secret, string RequestID, string RequesterName)
        {
            this.Token = Token;
            this.Name = Name;
            this.Secret = Secret;
            this.RequestID = RequestID;
            this.RequesterName = RequesterName;
        }


        /// <summary>
        /// Returns true if all the needed values for the auth obj is there
        /// Then it assigns the authobj the values
        /// So if this method returns true, the out has some value, else it is null
        /// </summary>
        /// <param name="auth">The auth object</param>
        /// <returns>True if the auth variable was assigned</returns>
        public bool GetAuthObj(out AuthObj auth)
        {
            if (string.IsNullOrWhiteSpace(Token) || string.IsNullOrWhiteSpace(Secret) || string.IsNullOrWhiteSpace(Name) || string.IsNullOrWhiteSpace(RequesterName))
            {
                auth = null;
                return false;
            }
            else
            {
                auth = new AuthObj(Token, Secret, Name, RequestID, RequesterName);
                return true;
            }
        }

        //Propertise for the different values
        public string RequestID { get; set; }

        public string Token { get; set; }

        public string Name { get; set; }

        public string Secret { get; set; }

        public string RequesterName { get; set; }
    }
}
