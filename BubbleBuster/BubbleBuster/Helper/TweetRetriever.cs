using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BubbleBuster.Web.ReturnedObjects;

namespace BubbleBuster.Helper
{
    public class TweetRetriever
    {

        private static TweetRetriever _instance;

        private TweetRetriever()
        {

        }

        public static TweetRetriever Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new TweetRetriever();
                }
                return _instance;
            }
        }

        public List<Tweet> getTweets(Friends friends)
        {
            List<Tweet> tweetList = new List<Tweet>();

            foreach(User user in friends.Users)
            {
                tweetList.AddRange(Web.WebHandler.MakeRequest<List<Tweet>>(RequestBuilder.BuildRequest(DataType.tweets, "user_id=" + user.Id)));
            }

            return tweetList;
        }
    }
}
