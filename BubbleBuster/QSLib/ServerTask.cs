using System;
using System.Collections.Generic;
using System.Text;
using BubbleBuster;
using System.Threading.Tasks;
using BubbleBuster.Helper.Objects;

namespace QSLib
{
    public class ServerTask // Class that parses the data from its input to the worker when run.
    {
        private AuthObj twitterApiKey;
        private string twitterName;

        public ServerTask (TwitterAcc twitterRequest)
        {
            twitterRequest.GetAuthObj(out twitterApiKey);
            twitterName = twitterRequest.Name;
        }

        /// <summary>
        /// Executes the task
        /// </summary>
        public void Run ()
        {
            new Worker(twitterApiKey, twitterName);
        }
    }
}
