using BubbleBuster.Helper;
using BubbleBuster.Helper.Objects;
using BubbleBuster.Web;
using BubbleBuster.Web.ReturnedObjects;
using BubbleBuster.Web.ReturnedObjects.RateLimit;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace BubbleBuster
{
    public class Worker //TODO: Implement the last things
    {
        private long userRecordId = -1;
        private AuthObj auth;
        private WebHandler webHandler;

        public Worker(AuthObj auth, string twitterName)
        {
            Run(auth, twitterName);
        }

        public void Run(AuthObj auth, string twitterName) //Executes the task parsed by the ServerTask class
        {
            this.auth = auth;
            webHandler = new WebHandler(auth);
            //Sets the limits such that we do not exceed the limits
            LimitHelper.Instance(auth).InitPropertises(new WebHandler(auth).TwitterGetRequest<Limit>(TwitterRequestBuilder.BuildStartupRequest()));
            User user = webHandler.TwitterGetRequest<User>(TwitterRequestBuilder.BuildRequest(RequestType.user, auth, "screen_name=" + twitterName)); //Used for getting the users political value

            //Analyse the tweets while retrieving and post them to the database
            TweetRetriever.Instance.GetTweetsFromUserAndAnalyse(user, auth, CheckIfResultExistOnDB, ClassifyTweet, PostResultToDB); 
            if(!GetUsersRecordIdOnDb(user,ref userRecordId))
            {
                Log.Error("Could not find the posted user, something is wrong!");
                return;
            } 

            //Gets the friends for the user
            var friends = FriendsRetriever.Instance.GetFriends(user, auth);
            Log.Debug("Following " + friends.Users.Count + " users");

            //Analyse the tweets while retrieving and post them to the database
            TweetRetriever.Instance.GetTweetsFromFriendsAndAnalyse(friends, auth, CheckIfResultExistOnDBAndLink, ClassifyTweet, PostResultToDBAndLink);

            FinalizeUser();
            Log.Debug("Done!!!");
        }

        private bool GetUsersRecordIdOnDb(User user, ref long result)
        {
            object temp = new object();
            if (webHandler.DatabaseGetRequest(Constants.DB_SERVER_IP + "twitter/has-id/" + user.Id, ref temp))
            {
                try
                {
                    result = Convert.ToInt64(temp);
                }
                catch (Exception e)
                {
                    Log.Error(e.Message);
                    return false;
                }
                return true;
            }
            else
            {
                return false;
            }
            
        }

        private bool CheckIfResultExistOnDB(User user)
        {
            long result = -1;
            bool succes = GetUsersRecordIdOnDb(user, ref result);

            return succes;
        }

        private bool CheckIfResultExistOnDBAndLink(User user)
        {
            long result = -1;
            bool succes = GetUsersRecordIdOnDb(user, ref result);
            if (succes)
            {
                AddFollower(result);
            }

            return succes;
        }

        private AnalysisResultObj ClassifyTweet(List<Tweet> tweetList)
        {
            Classifier c = new Classifier();
            AnalysisResultObj result = TweetAnalyzer.Instance.AnalyzeAndDecorateTweetsThreaded(tweetList);
            result.MIResult = c.RunNaiveBayes(tweetList);

            return result;
        }

        private bool PostResultToDB(AnalysisResultObj resultObj, User user)
        {
            bool succes = webHandler.DatabaseSendDataRequest(Constants.DB_SERVER_IP + "twitter", "POST", "twitter_name=" + user.ScreenName, "twitter_id=" + user.Id, "analysis_val=" + resultObj.GetAlgorithmResult().ToString(CultureInfo.InvariantCulture), "media_val=" + resultObj.GetMediaResult().ToString(CultureInfo.InvariantCulture), "mi_val=" + resultObj.GetMIResult().ToString(CultureInfo.InvariantCulture), "sentiment_val=" + resultObj.GetSentiment().ToString(CultureInfo.InvariantCulture), "tweet_count=" + resultObj.Count, "protect=" + Convert.ToInt32(user.IsProtected));
            if (!succes)
            {
                Log.Error("Could not post the user to the database");
                return false;
            }

            return succes;
        }

        private bool PostResultToDBAndLink(AnalysisResultObj resultObj, User user)
        {
            bool succes = PostResultToDB(resultObj, user);

            long temp = -1;
            succes = GetUsersRecordIdOnDb(user, ref temp);
            if (!succes)
            {
                Log.Error("Could not find the posted user");
                return false;
            }
            AddFollower(temp);

            return succes;
        }

        private bool AddFollower(long frinedRecordId)
        {
            bool succes = webHandler.DatabaseSendDataRequest(Constants.DB_SERVER_IP + "twitter/add_follower", "PUT", "record_id=" + userRecordId, "follows_id=" + frinedRecordId);
            if (!succes)
            {
                Log.Error("Could not add the follower");
            }
            return succes;
        }

        private void FinalizeUser()
        {
            if(!webHandler.DatabaseSendDataRequest(Constants.DB_SERVER_IP + "twitter/finalize", "PUT", "record_id=" + userRecordId))
            {
                Log.Error("Could not finalize the request");
            }
        }


    }
}
