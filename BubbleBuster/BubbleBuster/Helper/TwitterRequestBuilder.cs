using BubbleBuster.Helper;
using BubbleBuster.Helper.Objects;
using BubbleBuster.Web;
using BubbleBuster.Web.ReturnedObjects.RateLimit;
using System;
using System.Threading;

namespace BubbleBuster
{
    /// <summary>
    /// Type to tell the request builder what kind of request it is.
    /// </summary>
    public enum RequestType { friendsId, friendsObj, tweets, limit, user };

    public static class TwitterRequestBuilder
    {
        private static string baseUrl = "https://api.twitter.com/1.1/";

        /// <summary>
        /// Builds the startup request. Which means that the limit request is built. 
        /// </summary>
        /// <returns>The startup request object</returns>
        public static RequestUrlObject BuildStartupRequest()
        {
            return Build(RequestType.limit);
        }

        /// <summary>
        /// Builds a request using the request type to determine which url to use. 
        /// </summary>
        /// <param name="requestType">The request type</param>
        /// <param name="auth">The auth object</param>
        /// <param name="parameters">The parameters of the request</param>
        /// <returns>The request url object</returns>
        public static RequestUrlObject BuildRequest(RequestType requestType, AuthObj auth, params string[] parameters)
        {
            RequestUrlObject result = Build(requestType, parameters);

            //Checks if the worker is allowed to make the request
            if (CheckIfAllowedToMakeRequestOrSleep(requestType, ref result, auth, parameters))
            {
                /*Because we need to make sure after wakeup that we have a new request pool and thus we recursly call build.
                Then when we have the result, we can just return it, if we do not then the request would subtract 2 from the pool*/
                return result;
            }

            //Subtracts one from the pool of request the worker is allowed to make.
            LimitHelper.Instance(auth).SubtractFrom(requestType);

            return result;
        }

        /// <summary>
        /// Checks if the request is a allowed to be made, else the thread will sleep until the request pool should be refiled
        /// So the return value should not be able to become false.
        /// </summary>
        /// <param name="requestType">The request type</param>
        /// <param name="result">The result requestUrl object</param>
        /// <param name="auth">The auth object</param>
        /// <param name="parameters">The request parameters</param>
        /// <returns>True if allowed to make the request, else false.</returns>
        private static bool CheckIfAllowedToMakeRequestOrSleep(RequestType requestType, ref RequestUrlObject result, AuthObj auth, params string[] parameters)
        {
            if (!LimitHelper.Instance(auth).AllowedToMakeRequest(requestType))
            {
                TimeSpan sleepTime = LimitHelper.Instance(auth).GetResetTime(requestType);
                if (sleepTime.TotalMinutes > 0)
                {
                    Log.Debug("Sleep at " + DateTime.Now + " until " + LimitHelper.Instance(auth).GetResetDateTime(requestType));
                    Thread.Sleep(sleepTime);
                    Log.Debug("Wakeup at " + DateTime.Now);
                }
                LimitHelper.Instance(auth).InitPropertises(new WebHandler(auth).TwitterGetRequest<Limit>(BuildStartupRequest()));
                result = BuildRequest(requestType, auth, parameters); //Such that we check again
                return true;
            }
            return false;
        }

        /// <summary>
        /// Builds the request based on the request type and the parameters
        /// </summary>
        /// <param name="requestType">The request type</param>
        /// <param name="parameters">The request parameters</param>
        /// <returns>Request url object</returns>
        private static RequestUrlObject Build(RequestType requestType, params string[] parameters)
        {
            RequestUrlObject returnObject = new RequestUrlObject();

            //Assigns the base url needed for OAuth and the intermediate full url.
            switch (requestType)
            {
                case RequestType.friendsId:
                    returnObject.BaseUrl += baseUrl + "friends/ids.json";
                    returnObject.Url += returnObject.BaseUrl + "?";
                    break;
                case RequestType.friendsObj:
                    returnObject.BaseUrl += baseUrl + "friends/list.json";
                    returnObject.Url += returnObject.BaseUrl + "?";
                    break;
                case RequestType.tweets:
                    returnObject.Params.Add("include_rts", "false");
                    returnObject.Params.Add("tweet_mode", "extended");
                    returnObject.BaseUrl += baseUrl + "statuses/user_timeline.json";
                    returnObject.Url += returnObject.BaseUrl + "?include_rts=false&tweet_mode=extended&";
                    break;
                case RequestType.limit:
                    returnObject.Params.Add("resources", "friends,statuses,application,users");
                    returnObject.BaseUrl += baseUrl + "application/rate_limit_status.json";
                    returnObject.Url += returnObject.BaseUrl + "?resources=friends,statuses,application,users";
                    break;
                case RequestType.user:
                    returnObject.BaseUrl += baseUrl + "users/show.json";
                    returnObject.Url += returnObject.BaseUrl + "?";
                    break;
                default:
                    break;
            }

            //Adds the parameters to the intermediate full url. Such that it becomes the full url.
            foreach (string par in parameters)
            {
                string[] param = par.Split('=');
                returnObject.Params.Add(param[0], param[1]);
                returnObject.Url += par + "&";
            }

            //Trims the last & such that it is a valid url.
            returnObject.Url = returnObject.Url.TrimEnd('&');
            return returnObject;
        }
    }
}
