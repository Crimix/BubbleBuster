using System;
using System.Collections.Generic;
using System.Text;
using BubbleBuster;
using System.Threading.Tasks;

namespace QSLib
{
    class ServerTask // Class that parses the data from its input to the worker when run.
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
            new BubbleBuster.Worker(twitterApiKey, twitterName);
        }
    }
}
