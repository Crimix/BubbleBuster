using System;
using System.Collections.Generic;
using System.Text;
using BubbleBuster;
using System.Threading.Tasks;

namespace QSLib
{
    class ServerTask
    {
        string twitterApiKey;
        string twitterName;
        public ServerTask (TwitterAcc twitterRequest)
        {
            twitterApiKey = twitterRequest.TwitterApiKey;
            twitterName = twitterRequest.TwitterName;
        }

        public void Run ()
        {
            BubbleBuster.Worker(twitterApiKey, twitterName);
        }
    }
}
