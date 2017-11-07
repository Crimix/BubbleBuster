using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BubbleBuster.Web.ReturnedObjects;
using System.Threading;
using BubbleBuster.Web;

namespace BubbleBuster.Helper
{
    public class TweetRetriever
    {

        private static TweetRetriever _instance;
        private static int userTweetCount = 1;

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

        public List<Tweet> GetTweetsFromUser(long userId)
        {
            List<Tweet> tweetList = new List<Tweet>();
            User user = new User();
            user.Id = userId;
            Task<List<Tweet>> task = new Task<List<Tweet>>(() => TweetThreadMethod(user));
            task.Start();
            task.Wait();

            tweetList.AddRange(task.Result);

            return tweetList;
        }

        public List<Tweet> GetTweetsFromFriends(Friends friends)
        {
            List<Tweet> tweetList = new List<Tweet>();
            List<Task<List<Tweet>>> runningTasks = new List<Task<List<Tweet>>>();
            List<Task<List<Tweet>>> taskList = new List<Task<List<Tweet>>>();
            Queue<Task<List<Tweet>>> taskQueue = new Queue<Task<List<Tweet>>>();
            Log.Info(String.Format("{0,5}: {1,-20} {2,-20} {3,-11}", "Count", "User name", "User id", "Tweet count"));
            foreach (User user in friends.Users)
            {
                Task<List<Tweet>> task = new Task<List<Tweet>>(() => TweetThreadMethod(user));
                taskQueue.Enqueue(task);
                task = null;
            }
            friends = null;

            while(taskQueue.Count != 0)
            {
                if(taskList.Count < 3)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        if(taskQueue.Count != 0)
                        { 
                            var task = taskQueue.Dequeue();
                            task.Start();
                            runningTasks.Add(task);
                            taskList.Add(task);
                        }
                    }
                }
                else
                {
                    int index = Task.WaitAny(runningTasks.ToArray(), -1);
                    runningTasks.RemoveAt(index);
                    var task = taskQueue.Dequeue();
                    task.Start();
                    runningTasks.Add(task);
                    taskList.Add(task);
                }
            }
            taskQueue = null;

            Task.WaitAll(taskList.ToArray());

            foreach (var item in taskList)
            {
                tweetList.AddRange(item.Result);
            }

            taskList = null;
            runningTasks = null;

            return tweetList;
        }

        private List<Tweet> TweetThreadMethod(User user)
        {
            List<Tweet> temp = GetUserTweets(user);
            if (user.IsProtected)
            {
                Log.Info(String.Format("{0,5}: {1,-20} {2,-20} {3,-11}", userTweetCount, user.Name, user.Id, "Protected"));
            }
            else
            {
                Log.Info(String.Format("{0,5}: {1,-20} {2,-20} {3,-11}", userTweetCount, user.Name, user.Id, temp.Count));
            }
            Interlocked.Increment(ref userTweetCount);
            return temp;
        }

        public List<Tweet> GetUserTweets(User user)
        {
            List<Tweet> tweetList = new List<Tweet>();
            List<Tweet> tempList = new List<Tweet>();

            if (!user.IsProtected)
            {
                long lastTweetID = 0;
                tempList.AddRange(new WebHandler().MakeRequest<List<Tweet>>(RequestBuilder.BuildRequest(DataType.tweets, "user_id=" + user.Id, "count=200")));
                if (tempList.Count != 0)
                {
                    lastTweetID = tempList.ElementAt(tempList.Count - 1).Id;
                    tweetList.AddRange(tempList.Where(x => !tweetList.Contains(x)));


                    while (tweetList.Count < Constants.TWEETS_TO_RETRIEVE)
                    {
                        tempList.AddRange(new WebHandler().MakeRequest<List<Tweet>>(RequestBuilder.BuildRequest(DataType.tweets, "user_id=" + user.Id, "count=200", "max_id=" + lastTweetID)));
                        if (tempList.Count == 0 || tempList.ElementAt(tempList.Count - 1).Id == lastTweetID)
                        {
                            break;
                        }
                        lastTweetID = tempList.ElementAt(tempList.Count - 1).Id;
                        tweetList.AddRange(tempList.Where(x => !tweetList.Contains(x)));

                    }
                }
            }
            tempList = null;
            return tweetList;
        }
    }
}
