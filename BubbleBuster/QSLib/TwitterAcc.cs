using System;
using System.Collections.Generic;
using System.Text;

namespace QSLib
{
    public class TwitterAcc
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
