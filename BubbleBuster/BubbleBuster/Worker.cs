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
    public class Worker
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

            //Analyse the tweets of the user and post the result to the database. Does not use linking because of this is the user we need to ananlyse
            TweetRetriever.Instance.GetTweetsFromUserAndAnalyse(user, auth, CheckIfResultExistOnDB, ClassifyTweet, PostResultToDB);
            //Finds the posted users recordId and assigns it to the global userRecordId variable to be used in the CheckIfResultExistOnDBAndLink and PostResultToDBAndLink
            if (!GetUsersRecordIdOnDb(user,ref userRecordId))
            {
                Log.Error("Could not find the posted user, something is wrong!");
                return;
            } 

            //Gets the friends for the user
            var friends = FriendsRetriever.Instance.GetFriends(user, auth);
            Log.Debug("Following " + friends.Users.Count + " users");

            //Analyse the tweets of each frind and post the result to the database, then link the users result to the global userRecordId
            TweetRetriever.Instance.GetTweetsFromFriendsAndAnalyse(friends, auth, CheckIfResultExistOnDBAndLink, ClassifyTweet, PostResultToDBAndLink);

            //Finalizes the user by informing the database that the user has been proccessed.
            FinalizeUser();
            Log.Debug("Done!!!");
        }

#region Methods to be passed to TweetRetriever

        /// <summary>
        /// Returns using the ref value the users record id in the database. 
        /// If the return value is true the ref result contain a valid value
        /// </summary>
        /// <param name="user">The user</param>
        /// <param name="result">Ref result value</param>
        /// <returns>True if the result value contains a valid value</returns>
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

        /// <summary>
        /// Checks if a result already exist.
        /// </summary>
        /// <param name="user">The user</param>
        /// <returns>True if the result exist</returns>
        private bool CheckIfResultExistOnDB(User user)
        {
            long result = -1;
            bool succes = GetUsersRecordIdOnDb(user, ref result);

            return succes;
        }

        /// <summary>
        /// Checks if a result already exist, and then links the user to the global record.
        /// </summary>
        /// <param name="user">The user</param>
        /// <returns>True if the result exist</returns>
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

        /// <summary>
        /// Classifies a list of tweets using both of our approaches. 
        /// </summary>
        /// <param name="tweetList">List of tweets</param>
        /// <returns>An AnalysisResultObj which contains both results</returns>
        private AnalysisResultObj ClassifyTweet(List<Tweet> tweetList)
        {
            Classifier c = new Classifier();
            AnalysisResultObj result = TweetAnalyzer.Instance.AnalyzeAndDecorateTweetsThreaded(tweetList);
            result.MIResult = c.RunNaiveBayes(tweetList);

            return result;
        }

        /// <summary>
        /// Post method to be used by the TweetRetriever.
        /// </summary>
        /// <param name="resultObj">The result object returned by algorithm</param>
        /// <param name="user">The user</param>
        /// <returns>Returns true if the post request succeeded</returns>
        private bool PostResultToDB(AnalysisResultObj resultObj, User user)
        {
            //Create the post request
            bool succes = webHandler.DatabaseSendDataRequest(Constants.DB_SERVER_IP + "twitter", "POST", 
                "twitter_name=" + user.ScreenName, "twitter_id=" + user.Id, "analysis_val=" + resultObj.GetAlgorithmResult().ToString(CultureInfo.InvariantCulture), 
                "media_val=" + resultObj.GetMediaResult().ToString(CultureInfo.InvariantCulture), "mi_val=" + resultObj.MIResult.ToString(CultureInfo.InvariantCulture), 
                "sentiment_val=" + resultObj.GetSentiment().ToString(CultureInfo.InvariantCulture), "tweet_count=" + resultObj.Count, "protect=" + Convert.ToInt32(user.IsProtected));
            if (!succes)
            {
                Log.Error("Could not post the user to the database");
                return false;
            }

            return succes;
        }

        /// <summary>
        /// Post method to be used in the analysis of the friends of a user in the TweetRetriever
        /// </summary>
        /// <param name="resultObj">The result object returned by algorithm</param>
        /// <param name="user">The user</param>
        /// <returns>Returns true if the post request succeeded</returns>
        private bool PostResultToDBAndLink(AnalysisResultObj resultObj, User user)
        {
            //Uses the post method
            bool succes = PostResultToDB(resultObj, user);

            //Then it tries to find the posted user and use that users record id to link using the add follower method
            long tempRecordId = -1;
            succes = GetUsersRecordIdOnDb(user, ref tempRecordId);
            if (!succes)
            {
                Log.Error("Could not find the posted user");
                return false;
            }
            AddFollower(tempRecordId);

            return succes;
        }

        /// <summary>
        /// Links the record id to that of the user being analysed
        /// </summary>
        /// <param name="frinedRecordId">The record id of the friend</param>
        /// <returns></returns>
        private bool AddFollower(long frinedRecordId)
        {
            //Uses the global assigned userRecordId.
            //There can not be a race condition because the userRecordId is updatedonce, before this code even runs.
            bool succes = webHandler.DatabaseSendDataRequest(Constants.DB_SERVER_IP + "twitter/add_follower", "PUT", "record_id=" + userRecordId, "follows_id=" + frinedRecordId);
            if (!succes)
            {
                Log.Error("Could not add the follower");
            }
            return succes;
        }

        /// <summary>
        /// Finalizes the user on the database. 
        /// </summary>
        private void FinalizeUser()
        {
            //This just updates the processed value of the record.
            //The request id is to inform which request has been proccessed
            //If the request id is null then the request was not from the GUI application, and as such the id should not be sent
            if(auth.RequestID != null)
            {
                if (!webHandler.DatabaseSendDataRequest(Constants.DB_SERVER_IP + "twitter/finalize", "PUT", "record_id=" + userRecordId, "request_id=" + auth.RequestID))
                {
                    Log.Error("Could not finalize the request");
                }
            }
            else
            {
                if (!webHandler.DatabaseSendDataRequest(Constants.DB_SERVER_IP + "twitter/finalize", "PUT", "record_id=" + userRecordId))
                {
                    Log.Error("Could not finalize the request");
                }

            }

        }

#endregion

    }
}
