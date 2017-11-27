using BubbleBuster.Helper;
using BubbleBuster.Web;
using BubbleBuster.Web.ReturnedObjects.RateLimit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BubbleBuster
{
    public enum DataType { friendsId, friendsObj, tweets, limit, user, database }; //Expand based on what data is needed

    public static class RequestBuilder
    {
        private static string baseUrl = "https://api.twitter.com/1.1/";

        public static string BuildStartupRequest()
        {
            return Build(DataType.limit);
        }
     
        public static string BuildRequest(DataType returnType, string apiKey, params string[] parameters)
        {
            string result = Build(returnType, parameters);

            if(CheckIfAllowedToMakeRequestOrSleep(returnType, ref result, apiKey, parameters))
            {
                /*Because we need to make sure after wakeup that we have a new request pool and thus we recursly call build.
                Then when we have the result, we can just return it, if we do not then the request would subtract 2 from the pool*/
                return result;   
            }

            LimitHelper.Instance(apiKey).SubtractFrom(returnType);

            return result;
        }


        private static bool CheckIfAllowedToMakeRequestOrSleep(DataType returnType, ref string result, string apiKey, params string[] parameters)
        {
            if (!LimitHelper.Instance(apiKey).AllowedToMakeRequest(returnType))
            {
                TimeSpan sleepTime = LimitHelper.Instance(apiKey).GetResetTime(returnType);
                if (sleepTime.TotalMinutes > 0)
                {
                    Log.Warn("Sleep at " + DateTime.Now + " until " + LimitHelper.Instance(apiKey).GetResetDateTime(returnType));
                    Thread.Sleep(sleepTime);
                    Log.Warn("Wakeup at " + DateTime.Now);
                }
                LimitHelper.Instance(apiKey).SetLimit(new WebHandler().MakeRequest<Limit>(BuildStartupRequest()));
                result = BuildRequest(returnType, apiKey, parameters);
                return true;
            }
            return false;
        }

        private static string Build(DataType returnType, params string[] parameters)
        {
            string returnString = "";

            switch (returnType)
            {
                case DataType.friendsId:
                    returnString += baseUrl+"friends/ids.json?";
                    break;
                case DataType.friendsObj:
                    returnString += baseUrl + "friends/list.json?";
                    break;
                case DataType.tweets:
                    returnString += baseUrl + "statuses/user_timeline.json?include_rts=false&tweet_mode=extended&";
                    break;
                case DataType.limit:
                    returnString += baseUrl + "application/rate_limit_status.json?resources=friends,statuses,application,users";
                    break;
                case DataType.user:
                    returnString += baseUrl + "users/show.json?";
                    break;
                case DataType.database:
                    returnString += "http://localhost:8000/api/twitter/?";
                    break;
                default:
                    break;
            }

            foreach (string par in parameters)
            {
                returnString += par + "&";
            }

            returnString = returnString.TrimEnd('&');
            return returnString;
        }
    }
}
