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
                List<Tweet> temp = getTweetsFromUser(user);
                tweetList.AddRange(temp);
                Console.WriteLine(user.Name + " " + temp.Count);
            }

            return tweetList;
        }

        private List<Tweet> getTweetsFromUser(User user)
        {
            List<Tweet> tweetList = new List<Tweet>();
            List<Tweet> tempList = new List<Tweet>();

            long lastTweetID = 0;
            tempList.AddRange(Web.WebHandler.MakeRequest<List<Tweet>>(RequestBuilder.BuildRequest(DataType.tweets, "user_id=" + user.Id, "count=200")));
            lastTweetID = tempList.ElementAt(tempList.Count - 1).Id;
            tweetList.AddRange(tempList.Where(x => !tweetList.Contains(x)));


            while (tweetList.Count < Constants.TWEETS_TO_RETRIEVE)
            {
                tempList.AddRange(Web.WebHandler.MakeRequest<List<Tweet>>(RequestBuilder.BuildRequest(DataType.tweets, "user_id=" + user.Id, "count=200", "max_id=" + lastTweetID)));
                if(tempList.ElementAt(tempList.Count - 1).Id == lastTweetID)
                {
                    break;
                }
                lastTweetID = tempList.ElementAt(tempList.Count - 1).Id;
                tweetList.AddRange(tempList.Where(x => !tweetList.Contains(x)));
                
            }

            return tweetList;
        }
    }
}
