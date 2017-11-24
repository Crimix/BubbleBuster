using BubbleBuster.Helper;
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
        public Worker (string twitterApiKey, string twitterName) //Executes the task parsed by the ServerTask class
        {
            string username = twitterName;
            string apiKey = twitterApiKey;

            //Sets the limits such that we do not exceed the limits
            LimitHelper.Instance(apiKey).SetLimit(new WebHandler().MakeRequest<Limit>(RequestBuilder.BuildStartupRequest()));
            User user = new WebHandler(apiKey).MakeRequest<User>(RequestBuilder.BuildRequest(DataType.user, apiKey, "screen_name=" + username));

            var userTweets = TweetRetriever.Instance.GetTweetsFromUser(user.Id, apiKey);
            var friends = FriendsRetriever.Instance.GetFriends(username,apiKey);
            Log.Info("Following " + friends.Users.Count + "users");

            List<Tweet> filterBubble = TweetRetriever.Instance.GetTweetsFromFriends(friends,apiKey);
            FileHelper.WriteObjectToFile("multTweets", filterBubble);


            double[] filterBubbleResults = TweetAnalyzer.Instance.AnalyzeAndDecorateTweets(filterBubble);
            double[] userResults = TweetAnalyzer.Instance.AnalyzeAndDecorateTweets(userTweets);
            double userpol = userResults[0] + userResults[1];
            double filterPol = filterBubbleResults[0] + filterBubbleResults[1];


            new WebHandler(apiKey).DBPostRequest(RequestBuilder.BuildRequest(DataType.database, apiKey, "name=" + user.Name, "twitterID=" + user.Id, "pol_var=" + userpol, "lib_var=" + 0, "fpol_var=" + filterPol, "flib_var=" + 0, "protect=" + Convert.ToInt32(user.IsProtected)));

            Log.Info("Done!!! " + filterBubble.Count);
        }
    }
}
