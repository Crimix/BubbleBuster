﻿using BubbleBuster.Helper;
using BubbleBuster.Helper.Objects;
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
    public enum DataType { friendsId, friendsObj, tweets, limit, user }; //Expand based on what data is needed

    public static class TwitterRequestBuilder
    {
        private static string baseUrl = "https://api.twitter.com/1.1/";

        public static UrlObject BuildStartupRequest()
        {
            return Build(DataType.limit);
        }
     
        public static UrlObject BuildRequest(DataType returnType, AuthObj apiKey, params string[] parameters)
        {
            UrlObject result = Build(returnType, parameters);

            if(CheckIfAllowedToMakeRequestOrSleep(returnType, ref result, apiKey, parameters))
            {
                /*Because we need to make sure after wakeup that we have a new request pool and thus we recursly call build.
                Then when we have the result, we can just return it, if we do not then the request would subtract 2 from the pool*/
                return result;   
            }

            LimitHelper.Instance(apiKey).SubtractFrom(returnType);

            return result;
        }


        private static bool CheckIfAllowedToMakeRequestOrSleep(DataType returnType, ref UrlObject result, AuthObj apiKey, params string[] parameters)
        {
            if (!LimitHelper.Instance(apiKey).AllowedToMakeRequest(returnType))
            {
                TimeSpan sleepTime = LimitHelper.Instance(apiKey).GetResetTime(returnType);
                if (sleepTime.TotalMinutes > 0)
                {

                    lock (Log.LOCK)
                    {
                        Log.Warn("Sleep at " + DateTime.Now + " until " + LimitHelper.Instance(apiKey).GetResetDateTime(returnType));
                    }
                    Thread.Sleep(sleepTime);
                    lock (Log.LOCK)
                    {
                        Log.Warn("Wakeup at " + DateTime.Now);
                    }
                }
                LimitHelper.Instance(apiKey).SetLimit(new WebHandler(apiKey).MakeRequest<Limit>(BuildStartupRequest()));
                result = BuildRequest(returnType, apiKey, parameters);
                return true;
            }
            return false;
        }

        private static UrlObject Build(DataType returnType, params string[] parameters)
        {
            UrlObject returnObject = new UrlObject();

            switch (returnType)
            {
                case DataType.friendsId:
                    returnObject.BaseUrl += baseUrl + "friends/ids.json";
                    returnObject.Url += returnObject.BaseUrl + "?";
                    break;
                case DataType.friendsObj:
                    returnObject.BaseUrl += baseUrl + "friends/list.json";
                    returnObject.Url += returnObject.BaseUrl + "?";
                    break;
                case DataType.tweets:
                    returnObject.Params.Add("include_rts", "false");
                    returnObject.Params.Add("tweet_mode", "extended");
                    returnObject.BaseUrl += baseUrl + "statuses/user_timeline.json";
                    returnObject.Url += returnObject.BaseUrl + "?include_rts=false&tweet_mode=extended&";
                    break;
                case DataType.limit:
                    returnObject.Params.Add("resources", "friends,statuses,application,users");
                    returnObject.BaseUrl += baseUrl + "application/rate_limit_status.json";
                    returnObject.Url += returnObject.BaseUrl + "?resources=friends,statuses,application,users";
                    break;
                case DataType.user:
                    returnObject.BaseUrl += baseUrl + "users/show.json";
                    returnObject.Url += returnObject.BaseUrl + "?";
                    break;
                default:
                    break;
            }

            foreach (string par in parameters)
            {
                string[] param = par.Split('=');
                returnObject.Params.Add(param[0], param[1]);
                returnObject.Url += par + "&";
            }

            returnObject.Url = returnObject.Url.TrimEnd('&');
            return returnObject;
        }
    }
}