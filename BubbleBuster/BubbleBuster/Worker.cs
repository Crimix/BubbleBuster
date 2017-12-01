using BubbleBuster.Helper;
using BubbleBuster.Helper.Objects;
using BubbleBuster.Web;
using BubbleBuster.Web.ReturnedObjects;
using BubbleBuster.Web.ReturnedObjects.RateLimit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace BubbleBuster
{
    public class Worker
    {
        public Worker (AuthObj auth, string twitterName) //Executes the task parsed by the ServerTask class
        {
            //Sets the limits such that we do not exceed the limits
            LimitHelper.Instance(auth).SetLimit(new WebHandler(auth).MakeRequest<Limit>(TwitterRequestBuilder.BuildStartupRequest()));
            User user = new WebHandler(auth).MakeRequest<User>(TwitterRequestBuilder.BuildRequest(DataType.user, auth, "screen_name=" + twitterName)); //Used for getting the users political value

            var userTweets = TweetRetriever.Instance.GetTweetsFromUser(user, auth); 
            var friends = FriendsRetriever.Instance.GetFriends(user, auth);
            Log.Info("Following " + friends.Users.Count + "users");

            List<Tweet> filterBubble = TweetRetriever.Instance.GetTweetsFromFriends(friends, auth);
            FileHelper.WriteObjectToFile("multTweets", filterBubble);

            Classifier c = new Classifier();
            double userpol = c.RunNaiveBayes(userTweets);
            double filtherpol = c.RunNaiveBayes(filterBubble);

            Log.Info(userpol + "   " + filtherpol);

            //double[] filterBubbleResults = TweetAnalyzer.Instance.AnalyzeAndDecorateTweetsThreaded(filterBubble);
            //double[] userResults = TweetAnalyzer.Instance.AnalyzeAndDecorateTweetsThreaded(userTweets);
            //double userpol = userResults[0] + userResults[1];
            //double filterPol = filterBubbleResults[0] + filterBubbleResults[1];

            //bool post = new WebHandler(apiKey).DBPostRequest(Constants.DB_SERVER_IP+ "twitter/?", "name =" + user.Name, "twitterID=" + user.Id, "pol_var=" + userpol, "lib_var=" + 0, "fpol_var=" + filterPol, "flib_var=" + 0, "protect=" + Convert.ToInt32(user.IsProtected)));

            //Log.Info("Done!!! " + filterBubble.Count + " success " + post);
        }


    }
}
