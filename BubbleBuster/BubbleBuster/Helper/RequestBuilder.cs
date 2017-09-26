using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BubbleBuster
{
    public enum DataType { friendsId, friendsObj, tweets, limit }; //Expand based on what data is needed

    public class RequestBuilder
    {
        private static string baseUrl = "https://api.twitter.com/1.1/";
        
        public static string BuildRequest(DataType returnType, params string[] parameters)
        {
            string returnString = baseUrl;

            switch (returnType)
            {
                case DataType.friendsId:
                    returnString += "friends/ids.json?";
                    break;
                case DataType.friendsObj:
                    returnString += "friends/list.json?";
                    break;
                case DataType.tweets:
                    returnString += "statuses/user_timeline.json?";
                    break;
                case DataType.limit:
                    returnString += "application/rate_limit_status.json?resources=friends,statuses";
                    break;
                default:
                    break;
            }

            foreach (string par in parameters)
            {
                returnString += par+"&";
            }

            returnString = returnString.TrimEnd('&');
            return returnString;
        }
    }
}
