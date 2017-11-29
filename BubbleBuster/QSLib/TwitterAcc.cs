using BubbleBuster.Helper.Objects;
using System;
using System.Collections.Generic;
using System.Text;

namespace QSLib
{
    public class TwitterAcc //Class of all information needed about a Twitter account
    {
        public TwitterAcc()
        {
        }

        public TwitterAcc (string Token, string Name, string RequesterName, string Secret)
        {
            this.Token = Token;
            this.Name = Name;
            this.Secret = Secret;
        }
        
        //Returns true if all the needed values for the auth obj is there
        //Then it assigns the authobj the values
        //So if this method returns true, the out has some value, else it is null
        public bool GetAuthObj(out AuthObj auth)
        {
            if(string.IsNullOrWhiteSpace(Token) || string.IsNullOrWhiteSpace(Secret))
            {
                auth = null;
                return false;
            }
            else
            {
                auth = new AuthObj(Token, Secret, Name, RequesterName);
                return true;
            }

        }

        public string RequesterName { get; set; }

        public string Token { get; set; }

        public string Name { get; set; }

        public string Secret { get; set; }
    }
}
