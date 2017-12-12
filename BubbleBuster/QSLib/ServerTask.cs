using BubbleBuster;
using BubbleBuster.Helper;
using BubbleBuster.Helper.Objects;

namespace QSLib
{
    // Class that parses the data from its input to the worker when run.
    public class ServerTask 
    {
        //The two variables for the task
        private AuthObj twitterAuth;
        private string twitterName;

        public ServerTask(TwitterAcc twitterRequest)
        {
            if(!twitterRequest.GetAuthObj(out twitterAuth))
            {
                Log.Error("Auth failed");
            }
            twitterName = twitterRequest.Name;
        }

        /// <summary>
        /// Executes the task
        /// </summary>
        public void Run()
        {
            if(twitterAuth != null)
            {
                Log.Debug("Task started " + twitterAuth.RequestID);
                new Worker(twitterAuth, twitterName);
            }
        }
    }
}
