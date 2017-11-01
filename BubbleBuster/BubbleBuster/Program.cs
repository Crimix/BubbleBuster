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

            //WordChecker.Instance.CheckTweetForWords(new Tweet());
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
            */

            User a = new User();
            a.Id = 15081340;
            a.IsProtected = false;
            a.Name = "gogreen18";
            List<Tweet> b = TweetRetriever.Instance.GetUserTweets(a);
            FileHelper.WriteObjectToFile("abc", "bca", b);

            Console.WriteLine("?");
            Console.ReadLine();
        }

    }
}
