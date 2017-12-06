using BubbleBuster.Helper.Objects;
using BubbleBuster.Web;
using BubbleBuster.Web.ReturnedObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BubbleBuster.Helper
{
    public class TweetRetriever
    {
        private static TweetRetriever _instance; //Variable for the singleton instance

        //To make it a singleton 
        private TweetRetriever()
        {
        }

        /// <summary>
        /// Method to get access to the TweetRetriever.
        /// Is the only static method, because it is not possible to create an instance outside of this class
        /// </summary>
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

        /// <summary>
        /// Get the tweets a user has posted, but does not return them, 
        /// but only if the database does not contain a result.
        /// It analyses the tweets using both our approaches.
        /// Then uploads it to the database.
        /// </summary>
        /// <param name="user">The user</param>
        /// <param name="auth">The auth object</param>
        /// <param name="get">The method to check the database for previous results</param>
        /// <param name="classifyMethod">The method to classify tweets</param>
        /// <param name="post">The method to post the result to the database</param>
        public void GetTweetsFromUserAndAnalyse(User user, AuthObj auth, Func<User, bool> get, Func<List<Tweet>, AnalysisResultObj> classifyMethod, Func<AnalysisResultObj, User, bool> post)
        {
            GetTweetsFromUserHelper(user, auth, (() => TweetThreadMethod(user, auth, get, classifyMethod, post)));
        }

        /// <summary>
        /// Get the tweets a user has posted, but does nothing to them.
        /// Kept for legacy purposes
        /// </summary>
        /// <param name="user">The user</param>
        /// <param name="auth">The auth object</param>
        /// <returns>A list of tweets a user has posted</returns>
        public List<Tweet> GetTweetsFromUser(User user, AuthObj auth)
        {
            return GetTweetsFromUserHelper(user, auth, (() => TweetThreadMethod(user, auth)));
        }

        /// <summary>
        /// Helper method, that is used by both GetTweetsFromUser and GetTweetsFromUserAndAnalyse
        /// </summary>
        /// <param name="user">The user</param>
        /// <param name="auth">The auth object</param>
        /// <param name="retrieveMethod">Method to retrieve the tweets</param>
        /// <returns>A list of tweets a user has posted</returns>
        private List<Tweet> GetTweetsFromUserHelper(User user, AuthObj auth, Func<List<Tweet>> retrieveMethod)
        {
            List<Tweet> tweetList = new List<Tweet>();
            Task<List<Tweet>> task = new Task<List<Tweet>>(() => retrieveMethod());
            task.Start();
            task.Wait();
            tweetList.AddRange(task.Result);

            return tweetList;
        }

        /// <summary>
        /// Gets and analyses the tweets from each of the friends.
        /// In the same way that GetTweetsFromUserAndAnalyse does.
        /// </summary>
        /// <param name="friends">The friends of an user </param>
        /// <param name="auth">The auth object</param>
        /// <param name="get">The method to check the database for previous results</param>
        /// <param name="classifyMethod">The method to classify tweets</param>
        /// <param name="post">The method to post the result to the database</param>
        public void GetTweetsFromFriendsAndAnalyse(Friends friends, AuthObj auth, Func<User, bool> get, Func<List<Tweet>, AnalysisResultObj> classifyMethod, Func<AnalysisResultObj, User, bool> post)
        {
            GetTweetsFromFriendsHelper(friends, auth, ((x) => TweetThreadMethod(x, auth, get, classifyMethod, post)));
        }

        /// <summary>
        /// Gets the tweets from each of the friends.
        /// Kept for legacy purposes.
        /// </summary>
        /// <param name="friends">The friends of an user </param>
        /// <param name="auth">The auth object</param>
        /// <returns>A list of tweets from the friends</returns>
        public List<Tweet> GetTweetsFromFriends(Friends friends, AuthObj auth)
        {
            return GetTweetsFromFriendsHelper(friends, auth, ((x) => TweetThreadMethod(x, auth)));
        }

        /// <summary>
        /// Helper method for both GetTweetsFromFriendsAndAnalyse and GetTweetsFromFriends.
        /// </summary>
        /// <param name="friends">The friends of an user </param>
        /// <param name="auth">The auth object</param>
        /// <param name="retrieveMethod">Method to retrieve the tweets</param>
        /// <returns>A list of tweets from the friends</returns>
        private List<Tweet> GetTweetsFromFriendsHelper(Friends friends, AuthObj apiKey, Func<User, List<Tweet>> retrieveMethod)
        {
            List<Tweet> tweetList = new List<Tweet>();
            List<Task<List<Tweet>>> runningTasks = new List<Task<List<Tweet>>>();
            List<Task<List<Tweet>>> taskList = new List<Task<List<Tweet>>>();
            Queue<Task<List<Tweet>>> taskQueue = new Queue<Task<List<Tweet>>>();

            foreach (User user in friends.Users)
            {
                Task<List<Tweet>> task = new Task<List<Tweet>>(() => retrieveMethod(user));
                taskQueue.Enqueue(task);
            }

            friends = null;

            while (taskQueue.Count != 0)
            {
                //Startup process of starting 3 tasks
                if (taskList.Count < 3)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        if (taskQueue.Count != 0)
                        {
                            var task = taskQueue.Dequeue();
                            task.Start();
                            runningTasks.Add(task);
                            taskList.Add(task);
                        }
                    }
                }
                else //Just start a new task when one finishes
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

        /// <summary>
        /// The threaded tweet retrieve method which is used in the task 
        /// </summary>
        /// <param name="user">The user</param>
        /// <param name="auth">The auth object</param>
        /// <param name="get">The method to check the database for previous results</param>
        /// <param name="classifyMethod">The method to classify tweets</param>
        /// <param name="post">The method to post the result to the database</param>
        /// <returns>A list of tweets </returns>
        private List<Tweet> TweetThreadMethod(User user, AuthObj auth, Func<User, bool> get = null, Func<List<Tweet>, AnalysisResultObj> classifyMethod = null, Func<AnalysisResultObj, User, bool> post = null)
        {
            bool alreadyExist = false;
            AnalysisResultObj tempResult = new AnalysisResultObj();
            List<Tweet> temp = new List<Tweet>();
            if (get != null)
            {
                alreadyExist = get(user); //Uses the supplied method to find out if the result already exist on the database
                if (alreadyExist)
                {
                    Log.Debug(String.Format("{0,-30} {1,-20} {2,-11}", user.Name, user.Id, "Already exist in db"));
                }
            }

            if (!alreadyExist)
            {
                temp = RetrieveTweets(user, auth);
                if (user.IsProtected)
                {
                    Log.Debug(String.Format("{0,-30} {1,-20} {2,-11}", user.Name, user.Id, "Protected"));
                }
                else
                {
                    Log.Debug(String.Format("{0,-30} {1,-20} {2,-11}", user.Name, user.Id, temp.Count));
                }

                if (classifyMethod != null && post != null)
                {
                    tempResult = classifyMethod(temp); //Use the supplied method to classify the list of tweets
                    post(tempResult, user); //Use the supplied method to post the result to the database
                    temp = new List<Tweet>();
                }
            }

            return temp;
        }

        /// <summary>
        /// The method to retrieve tweets from a specific user.
        /// This method can retrieve as few tweets as 200 or as many as the constant TWEETS_TO_RETRIEVE (max.  3200) 
        /// </summary>
        /// <param name="user">The user</param>
        /// <param name="auth">The auth object</param>
        /// <returns>A list of tweet posted by the user</returns>
        private List<Tweet> RetrieveTweets(User user, AuthObj auth)
        {
            List<Tweet> tweetList = new List<Tweet>();
            List<Tweet> tempList = new List<Tweet>();

            if (!user.IsProtected)
            {
                long lastTweetID = 0;
                tempList.AddRange(new WebHandler(auth).TwitterGetRequest<List<Tweet>>(TwitterRequestBuilder.BuildRequest(RequestType.tweets, auth, "user_id=" + user.Id, "count=200")));
                if (tempList.Count != 0)
                {
                    lastTweetID = tempList.ElementAt(tempList.Count - 1).Id;
                    tweetList.AddRange(tempList.Where(x => !tweetList.Contains(x)));

                    while (tweetList.Count < Constants.TWEETS_TO_RETRIEVE)
                    {
                        tempList.AddRange(new WebHandler(auth).TwitterGetRequest<List<Tweet>>(TwitterRequestBuilder.BuildRequest(RequestType.tweets, auth, "user_id=" + user.Id, "count=200", "max_id=" + lastTweetID)));

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
