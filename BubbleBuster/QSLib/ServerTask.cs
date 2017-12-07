using BubbleBuster;
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
            twitterRequest.GetAuthObj(out twitterAuth);
            twitterName = twitterRequest.Name;
        }

        /// <summary>
        /// Executes the task
        /// </summary>
        public void Run()
        {
            new Worker(twitterAuth, twitterName);
        }
    }
}
