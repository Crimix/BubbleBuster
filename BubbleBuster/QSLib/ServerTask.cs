using System;
using System.Collections.Generic;
using System.Text;
using BubbleBuster;
using System.Threading.Tasks;
using BubbleBuster.Helper.Objects;

namespace QSLib
{
    class ServerTask // Class that parses the data from its input to the worker when run.
    {
        AuthObj twitterApiKey;
        string twitterName;
        public ServerTask (TwitterAcc twitterRequest)
        {
            twitterRequest.GetAuthObj(out twitterApiKey);
            twitterName = twitterRequest.Name;
        }

        public void Run ()
        {
            new Worker(twitterApiKey, twitterName);
        }
    }
}
