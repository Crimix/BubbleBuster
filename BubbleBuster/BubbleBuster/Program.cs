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
            /*
            List<Tweet> returned3 = TweetRetriever.Instance.GetTweetsFromFriends(returned);
            FileHelper.WriteObjectToFile("BubbleBuster", "multTweets", returned3);

            Log.Info("Done!!! " + returned3.Count);
            Console.WriteLine("Done!");

            Console.ReadLine();
            */
            /*User a = new User();
            a.Id = 85583894;
            a.IsProtected = false;
            a.Name = "MuslimIQ";
            List<Tweet> tweetList = TweetRetriever.Instance.GetUserTweets(a);
            double[] conclusion = TweetAnalyzer.Instance.AnalyzeAndDecorateTweets(tweetList);
            Console.WriteLine("HashtagBias= " + conclusion[0]);
            Console.WriteLine("MediaBias= " + conclusion[1]);
            Console.WriteLine("TweetCount= " + conclusion[2]);
            Console.WriteLine("Positivity= " + conclusion[3] / conclusion[2]);
            Console.WriteLine("Negativity= " + conclusion[4]/conclusion[2]);
            Console.WriteLine("Conclusion left/right= " + (conclusion[0] + conclusion[1])/conclusion[2]);
            FileHelper.WriteObjectToFile("abc", a.Name, tweetList);
            */

            List<PolUserObj> polusers = new List<PolUserObj>();
            //(MI, ALG)

            //Leftwing Users
            polusers.Add(new PolUserObj(61734492, -1)); //fahrenthold,  (right, left)
            polusers.Add(new PolUserObj(15893354, -1)); //wpjenna       (right, left)
            polusers.Add(new PolUserObj(15689503, -1)); //cbellatoni    (right, left)
            polusers.Add(new PolUserObj(15691197, -1));//Atrios         (right, left)
            polusers.Add(new PolUserObj(14129299, -1));//Nicopitney     (right, left)
            polusers.Add(new PolUserObj(16076032, -1));//ggreenwald     (right, left)
            polusers.Add(new PolUserObj(3586084752, -1));//wonkroom     (banned?)
            polusers.Add(new PolUserObj(27511061, -1));//stevebenen     (right, left)
            polusers.Add(new PolUserObj(14924233, -1));//AlanColmes     (right, left)
            polusers.Add(new PolUserObj(85583894, -1));//MuslimIQ       (right, left)
            polusers.Add(new PolUserObj(93069110, -1));//MaggieNYT      (right, left)
            polusers.Add(new PolUserObj(16303106, -1));//StephenAtHome  (right, left)
            polusers.Add(new PolUserObj(46335511, -1));//TrevorNoah     (right, left)

            //Rightwing Users
            polusers.Add(new PolUserObj(147580943, 1)); //              (right, right)
            polusers.Add(new PolUserObj(18643437, 1));  //              (right, right)
            polusers.Add(new PolUserObj(640893, 1)); //Ewerickson       (right, right)
            polusers.Add(new PolUserObj(4248211, 1)); //mindyflynn      (right, left)
            polusers.Add(new PolUserObj(14197312, 1)); //dmataconis     (right, neutral)
            polusers.Add(new PolUserObj(16068266, 1)); //TPCarney       (right, right)
            polusers.Add(new PolUserObj(16244449, 1)); //jbarro         (left, right)
            polusers.Add(new PolUserObj(14347972, 1)); //heminator      (
            polusers.Add(new PolUserObj(366618441, 1)); //reihansalam   (
            polusers.Add(new PolUserObj(15976697, 1)); //michellemalkin (
            polusers.Add(new PolUserObj(65493023, 1)); //sarahpalinUSA  (
            polusers.Add(new PolUserObj(17454769, 1)); //glennbeck      (
            polusers.Add(new PolUserObj(17980523, 1)); //mitchellvii    (

            Console.WriteLine("Begin Retrieving: " + DateTime.Now);
            AuthObj auth = new AuthObj();
            User user = new WebHandler(auth).TwitterGetRequest<User>(TwitterRequestBuilder.BuildRequest(RequestType.user, auth, "screen_name=" + "realDonaldTrump"));
            var userTweets = TweetRetriever.Instance.GetTweetsFromUser(user, auth);
            var friends = FriendsRetriever.Instance.GetFriends(user, auth);
            Log.Debug("Following " + friends.Users.Count + "users");

            Dictionary<Tweet, string> asf = new Dictionary<Tweet, string>();

            //List<Tweet> ageg = asf.Keys;

            //List<Tweet> filterBubble = TweetRetriever.Instance.GetTweetsFromFriends(friends, auth);
            List<Tweet> filterBubble = TweetRetriever.Instance.GetTweetsFromUser(user, auth);
            Console.WriteLine("Begin Analyzing: " + DateTime.Now);
            AnalysisResultObj output = TweetAnalyzer.Instance.AnalyzeAndDecorateTweets(filterBubble);
            Console.WriteLine(output.GetAlgorithmResult() + " " + output.KeywordBias);
            Console.WriteLine("Done: " + DateTime.Now);
            /*Classifier classifier = new Classifier();

            Console.WriteLine("Naive Bayes:");
            double naive = classifier.RunNaiveBayes(tweetList);
            Console.WriteLine(naive);

            Console.WriteLine("\nAlgorithm:");
            foreach(double aasda in output)
            {
                Console.WriteLine(aasda);
            }

            FileHelper.WriteObjectToFile("test", tweetList);
            */
            //WordWorker.Instance.UpdateWords(polusers, new AuthObj("a"));



            //new TwitterApi(ConsumerKey, ConsumerKeySecret, AccessToken, AccessTokenSecret);

            OAuthHelper a = OAuthHelper.Instance;
            //Console.WriteLine(a.BuildAuthHeader("xvz1evFS4wEEPTGEFPHBog", "370773112-GmHxMAgYyLbNEtIKZeRNFsMKPR9EyMZeS9weJAEb"));
            //Console.WriteLine(a.BuildAuthHeader(OAuthHelper.DataType.POST, "ExampleUser", "xvz1evFS4wEEPTGEFPHBog", "kAcSOqF21Fu85e7zjz7ZN2U4ZRhfV3WpwPAoE3Z7kBw", "370773112-GmHxMAgYyLbNEtIKZeRNFsMKPR9EyMZeS9weJAEb", "LswwdoUaIvS8ltyTt5jkRh4J50vUPVVHtR2YPi5kE"));

            string name = "FilterBubble";
            string accessToken = "14594669-CUFTDY6oMmC6FIu3IwHY65G26cQHmmuc9XhHuQFaL";
            string tokenSecret = "uzV2suIyfoTaHDgbZERCsxBlqjMaFFCfGJ0yNDbra5b5K";
            string baseUrl = "https://api.twitter.com/1.1/application/rate_limit_status.json";

            Dictionary<string, string> extraParams = new Dictionary<string, string>();
            //extraParams.Add("status", "Hello Ladies + Gentlemen, a signed OAuth request!");
            //extraParams.Add("resources", "friends,statuses,application,users");
            //&&
            extraParams.Add("resources", "friends,statuses,application,users");
            //extraParams.Add("cursor", "-1");
            //extraParams.Add("screen_name", "twitterapi");
            //extraParams.Add("count", "5000");

            //List<Tweet> aasdas = FileHelper.ReadObjectFromFile<List<Tweet>>(Constants.PROGRAM_DATA_FILEPATH + "\\");



            // Console.WriteLine(a.BuildAuthHeader(OAuthHelper.DataType.GET, name, accessToken, tokenSecret, baseUrl, extraParams));

            

            /*
            List<string> elist = new List<string>();


            List <string> emotionList = FileHelper.ReadObjectFromFile<List<string>>("emotions.json");

            foreach(string str in emotionList)
            {
                if (!elist.Contains(str))
                {
                    elist.Add(str);
                }
            }

            FileHelper.WriteObjectToFile("newEmotion", elist);
            */

            Console.WriteLine("?");
            Console.ReadLine();
        }

    }
}
