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

            var returned = Web.WebHandler.MakeRequest<Friends>(RequestBuilder.BuildRequest(DataType.friendsObj, "screen_name=pewdiepie"));
            FileHelper.WriteObjectToFile("BubbleBuster", "friends", returned);
            Console.WriteLine(returned);
            var returned2 = Web.WebHandler.MakeRequest<List<Tweet>>(RequestBuilder.BuildRequest(DataType.tweets, "screen_name=pewdiepie"));
            FileHelper.WriteObjectToFile("BubbleBuster", "tweets", returned2);
            Console.WriteLine(returned2);
            var returned3 = Web.WebHandler.MakeRequest<Limit>(RequestBuilder.BuildRequest(DataType.limit));
            FileHelper.WriteObjectToFile("BubbleBuster", "limits", returned3);
            Console.WriteLine(returned3);


            Console.ReadLine();
        }
    
    }
}
