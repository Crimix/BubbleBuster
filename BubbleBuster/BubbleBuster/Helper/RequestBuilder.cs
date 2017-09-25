using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BubbleBuster
{
    public enum DataType { friendsId, friendsObj, tweets }; //Expand based on what data is needed

    public class RequestBuilder
    {
        string baseUrl = "https://api.twitter.com/1.1/";
        
        public string BuildRequest(DataType returnType, string userName)
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
                default:
                    break;
            }

            returnString += ("screen_name=" + userName);

            return returnString;
        }

        public string BuildRequest(DataType returnType, string userName, string cursor)
        {
            string returnString = BuildRequest(returnType, userName);
            returnString += "&cursor=" + cursor;
            return returnString;
        }
    }
}
