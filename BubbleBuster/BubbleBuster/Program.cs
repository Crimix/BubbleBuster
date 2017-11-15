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

            List<PolUserObj> polusers = new List<PolUserObj>();
            polusers.Add(new PolUserObj(61734492, -1)); //fahrenthold, leftwing
            polusers.Add(new PolUserObj(15893354, -1)); //wpjenna
            polusers.Add(new PolUserObj(147580943, 1)); //
            polusers.Add(new PolUserObj(18643437, 1));  //

            WordWorker.Instance.UpdateWords(polusers);


            Console.WriteLine("?");
            Console.ReadLine();
        }

    }
}
