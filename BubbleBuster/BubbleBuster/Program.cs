using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using BubbleBuster;
using BubbleBuster.Web.ReturnedObjects;
using BubbleBuster.Helper;
using BubbleBuster.Helper.Objects;
using BubbleBuster.Web.ReturnedObjects.RateLimit;
using BubbleBuster.Web;
using BubbleBuster.WordUpdater;

namespace BubbleBuster
{
    public class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            List<PolUserObj> polusers = new List<PolUserObj>();
            //(MI, ALG)

            //Leftwing Users
            /*polusers.Add(new PolUserObj(61734492, -1)); //fahrenthold,  (right, left)
            polusers.Add(new PolUserObj(15893354, -1)); //wpjenna       (right, left)
            polusers.Add(new PolUserObj(15689503, -1)); //cbellantoni    (right, left)
            polusers.Add(new PolUserObj(15691197, -1));//Atrios         (right, left)
            polusers.Add(new PolUserObj(14129299, -1));//Nicopitney     (right, left)
            polusers.Add(new PolUserObj(16076032, -1));//ggreenwald     (right, left)
            polusers.Add(new PolUserObj(27511061, -1));//stevebenen     (right, left)
            polusers.Add(new PolUserObj(14924233, -1));//AlanColmes     (right, left)
            polusers.Add(new PolUserObj(85583894, -1));//MuslimIQ       (right, left)
            polusers.Add(new PolUserObj(93069110, -1));//MaggieNYT      (right, left)
            polusers.Add(new PolUserObj(16303106, -1));//StephenAtHome  (right, left)
            polusers.Add(new PolUserObj(46335511, -1));//TrevorNoah     (right, left)

            //Rightwing Users
            polusers.Add(new PolUserObj(147580943, 1)); //Gavin_McInnes (right, right)
            polusers.Add(new PolUserObj(18643437, 1));  //PrisonPlanet  (right, right)
            polusers.Add(new PolUserObj(640893, 1)); //Ewerickson       (right, right)
            polusers.Add(new PolUserObj(4248211, 1)); //mindyfinn      (right, left)
            polusers.Add(new PolUserObj(14197312, 1)); //dmataconis     (right, neutral)
            polusers.Add(new PolUserObj(16068266, 1)); //TPCarney       (right, right)
            polusers.Add(new PolUserObj(16244449, 1)); //jbarro         (left, right)
            polusers.Add(new PolUserObj(14347972, 1)); //heminator      (
            polusers.Add(new PolUserObj(366618441, 1)); //reihansalam   (
            polusers.Add(new PolUserObj(15976697, 1)); //michellemalkin (
            polusers.Add(new PolUserObj(65493023, 1)); //sarahpalinUSA  (
            polusers.Add(new PolUserObj(17454769, 1)); //glennbeck      (
            polusers.Add(new PolUserObj(17980523, 1)); //mitchellvii    (

            string screenName = "mitchellvii";
            string leaning = "Right";

            Console.WriteLine("Begin Retrieving: " + DateTime.Now);
            AuthObj auth = new AuthObj();
            User user = new WebHandler(auth).TwitterGetRequest<User>(TwitterRequestBuilder.BuildRequest(RequestType.user, auth, "screen_name=" + screenName));

            //var userTweets = TweetRetriever.Instance.GetTweetsFromUser(user, auth);
            //var friends = FriendsRetriever.Instance.GetFriends(user, auth);
            //Log.Info("Following " + friends.Users.Count + "users");
            //Dictionary<Tweet, string> asf = new Dictionary<Tweet, string>();
            //List<Tweet> filterBubble = TweetRetriever.Instance.GetTweetsFromFriends(friends, auth);
            
            List<Tweet> filterBubble = TweetRetriever.Instance.GetTweetsFromUser(user, auth);
            FileHelper.WriteObjectToFile("TestData/" + screenName + "-" + leaning, filterBubble);
            */

            string screenName = "mitchellvii";
            string leaning = "Right";

            List<Tweet> filterBubble = FileHelper.ReadObjectFromFile<List<Tweet>>("TestData/" + screenName + "-" + leaning + ".json");

            AnalysisResultObj output = TweetAnalyzer.Instance.AnalyzeAndDecorateTweets(filterBubble);
            Console.WriteLine("Result: " + output.GetAlgorithmResult() + " KeywordBias: " + output.KeywordBias + "Sentiment: " + output.GetSentiment());
            
            Console.WriteLine("?");
            Console.ReadLine();
        }

    }
}
