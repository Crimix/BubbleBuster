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

namespace BubbleBuster
{
    class Program
    {
        static void Main(string[] args)
        {
            RequestBuilder rb = new RequestBuilder();

            var returned = Web.WebHandler.MakeRequest<Friends>(rb.BuildRequest(DataType.friendsObj, "pewdiepie"));
            FileHelper.WriteObjectToFile("BubbleBuster", "friends", returned);
            Console.WriteLine(returned);
            var returned2 = Web.WebHandler.MakeRequest<List<Tweet>>(rb.BuildRequest(DataType.tweets, "pewdiepie"));
            FileHelper.WriteObjectToFile("BubbleBuster", "tweets", returned2);
            Console.WriteLine(returned2);

            Console.ReadLine();
        }
    
    }
}
