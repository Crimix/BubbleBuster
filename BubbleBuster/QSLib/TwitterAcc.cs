using BubbleBuster.Helper.Objects;
using System;
using System.Collections.Generic;
using System.Text;
using BubbleBuster.Helper;

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

        public bool GetAuthObj(out AuthObj auth)
        {
            if(string.IsNullOrWhiteSpace(Token) || string.IsNullOrWhiteSpace(Secret))
            {
                auth = null;
                return false;
            }
            else
            {
                Log.Info("Token: " + Token + "Secret: " + Secret + "Name: " + Name + "RequesterName: " + RequesterName);   
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
