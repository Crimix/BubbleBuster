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
            LimitHelper.Instance.SetLimit(Web.WebHandler.MakeRequest<Limit>(RequestBuilder.BuildStartupRequest()));

            var returned = Web.WebHandler.MakeRequest<Friends>(RequestBuilder.BuildRequest(DataType.friendsObj, "screen_name=pewdiepie"));
            FileHelper.WriteObjectToFile("BubbleBuster", "friends", returned);
            Console.WriteLine(returned);
            var returned2 = Web.WebHandler.MakeRequest<List<Tweet>>(RequestBuilder.BuildRequest(DataType.tweets, "screen_name=realDonaldTrump&count=3200&exclude_replies=true"));
            FileHelper.WriteObjectToFile("BubbleBuster", "tweets", returned2);
            Console.WriteLine(returned2.Count);

            Console.ReadLine();
        }

    }
}
