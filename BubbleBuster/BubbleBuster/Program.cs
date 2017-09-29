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

namespace BubbleBuster
{
    public class Program
    {
        static void Main(string[] args)
        {
            WordChecker.Instance.checkTweetForWords(new Tweet());
            LimitHelper.Instance.SetLimit(Web.WebHandler.MakeRequest<Limit>(RequestBuilder.BuildStartupRequest()));

            var returned = FriendsRetriever.Instance.getFriends("realDonaldTrump");

            List<Tweet> returned3 = TweetRetriever.Instance.getTweets(returned);
            FileHelper.WriteObjectToFile("BubbleBuster", "multTweets", returned3);

            Console.WriteLine("Done!!! " + returned3.Count);
            Console.ReadLine();
        }

    }
}
