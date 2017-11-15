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
        public TwitterAcc (string twitterApiKey, string twitterName)
        {
            TwitterApiKey = twitterApiKey;
            TwitterName = twitterName;
        }
        public string TwitterApiKey { get; set; }

        public string TwitterName { get; set; }
    }
}
