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
using BubbleBuster.Web.ReturnedObjects.RateLimit;
using BubbleBuster.Web;
using BubbleBuster.GUI;
using BubbleBuster.WordUpdater;

namespace BubbleBuster
{
    public class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            /*
            var application = new Application();
            Window window = new Window();
            UserControl1 user = new UserControl1();
            window.Content = user;
            application.Run(window);
            FileHelper.GenerateDirectoryStructure();
            foreach (string line in FileHelper.GetAnalysisWords().Keys)
            {
                Log.Info(line);
            }
            string username = "realDonaldTrump";
            //string username = "TestBot_SW709";

            LimitHelper.Instance.SetLimit(new WebHandler().MakeRequest<Limit>(RequestBuilder.BuildStartupRequest()));

            var returned = FriendsRetriever.Instance.GetFriends(username);
            Log.Info("Following " + returned.Users.Count);

            /*List<Tweet> returned2 = TweetRetriever.Instance.GetTweetsFromUser(909688209080242176);
            string temp = "";
            /foreach (var item in returned2)
            {
                foreach (var url in item.Entities.Urls)
                {
                    temp += url.ExpandedUrl + Environment.NewLine;
                }
            }
            FileHelper.WriteStringToFile("BubbleBuster", "url", temp);

            Console.WriteLine("Done!!! " + returned2.Count);
            */
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

            /*List<PolUserObj> polusers = new List<PolUserObj>();
            polusers.Add(new PolUserObj(61734492, -1)); //fahrenthold, leftwing
            polusers.Add(new PolUserObj(15893354, -1)); //wpjenna
            polusers.Add(new PolUserObj(15689503, -1)); //cbellatoni
            polusers.Add(new PolUserObj(15691197, -1));//Atrios
            polusers.Add(new PolUserObj(14129299, -1));//Nicopitney
            polusers.Add(new PolUserObj(16076032, -1));//ggreenwald
            polusers.Add(new PolUserObj(3586084752, -1));//wonkroom
            polusers.Add(new PolUserObj(27511061, -1));//stevebenen
            polusers.Add(new PolUserObj(14924233, -1));//AlanColmes
            polusers.Add(new PolUserObj(85583894, -1));//MuslimIQ
            polusers.Add(new PolUserObj(93069110, -1));//MaggieNYT
            polusers.Add(new PolUserObj(16303106, -1));//StephenAtHome
            polusers.Add(new PolUserObj(46335511, -1));//TrevorNoah

            polusers.Add(new PolUserObj(147580943, 1)); //
            polusers.Add(new PolUserObj(18643437, 1));  //
            polusers.Add(new PolUserObj(640893, 1)); //Ewerickson
            polusers.Add(new PolUserObj(4248211, 1)); //mindyflynn
            polusers.Add(new PolUserObj(14197312, 1)); //dmataconis
            polusers.Add(new PolUserObj(16068266, 1)); //TPCarney
            polusers.Add(new PolUserObj(16244449, 1)); //jbarro
            polusers.Add(new PolUserObj(14347972, 1)); //heminator
            polusers.Add(new PolUserObj(366618441, 1)); //reihansalam
            polusers.Add(new PolUserObj(15976697, 1)); //michellemalkin
            polusers.Add(new PolUserObj(65493023, 1)); //sarahpalinUSA
            polusers.Add(new PolUserObj(17454769, 1)); //glennbeck
            polusers.Add(new PolUserObj(17980523, 1)); //mitchellvii
            WordWorker.Instance.UpdateWords(polusers);*/

            Console.WriteLine("?");
            Console.ReadLine();
        }

    }
}
