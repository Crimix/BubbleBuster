﻿using BubbleBuster.Helper;
using BubbleBuster.Web;
using BubbleBuster.Web.ReturnedObjects;
using BubbleBuster.Web.ReturnedObjects.RateLimit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BubbleBuster
{
    public class Worker
    {
        public Worker (string twitterApiKey, string twitterName)
        {
            string username = twitterName;
            string apiKey = twitterApiKey;

            //Sets the limits such that we do not excced the limts
            LimitHelper.Instance(apiKey).SetLimit(new WebHandler().MakeRequest<Limit>(RequestBuilder.BuildStartupRequest()));

            var returned = FriendsRetriever.Instance.GetFriends(username,apiKey);
            Log.Info("Following " + returned.Users.Count);

            List<Tweet> returned3 = TweetRetriever.Instance.GetTweetsFromFriends(returned,apiKey);
            FileHelper.WriteObjectToFile("multTweets", returned3);

            Log.Info("Done!!! " + returned3.Count);
        }
    }
}
