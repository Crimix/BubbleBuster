using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using BubbleBuster;
using BubbleBuster.Web.ReturnedObjects;
using BubbleBuster.Helper;
using BubbleBuster.Web.ReturnedObjects.RateLimit;
using BubbleBuster.Web;

namespace BubbleBuster
{
    public class Program
    {
        static void Main(string[] args)
        {
            FileHelper.GenerateDirectoryStructure();

            List<Tweet> tweetList = FileHelper.ReadObjectFromFile<List<Tweet>>("BubbleBuster", "multTweets-DonaldTrump");
            WordChecker.Instance.CheckTweetsForHyperlinks(tweetList);
            string username = "katyperry";
            //string username = "TestBot_SW709";

            WordChecker.Instance.CheckTweetForWords(new Tweet());
           LimitHelper.Instance.SetLimit(new WebHandler().MakeRequest<Limit>(RequestBuilder.BuildStartupRequest()));

            var returned = FriendsRetriever.Instance.GetFriends(username);
            Console.WriteLine(returned.Users.Count);

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

            List<Tweet> returned3 = TweetRetriever.Instance.GetTweetsFromFriends(returned);
            FileHelper.WriteObjectToFile("BubbleBuster", "multTweets", returned3);

            Console.WriteLine("Done!!! " + returned3.Count);

            Console.ReadLine();
        }

    }
}
