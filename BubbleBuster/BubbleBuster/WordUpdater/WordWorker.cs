using BubbleBuster.Helper;
using BubbleBuster.Helper.Objects;
using BubbleBuster.Web;
using BubbleBuster.Web.ReturnedObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// This class is used to scan the tweets of a number of twitter users, and determine what words they use 
/// that are not part of the 1000(0) most common. (We should know the political affiliation of each of these users before hand).
/// In addition we count each time a specific word is encountered, and note the contex of the person (affiliation, positive/negative).
/// Can theoretically be used to dynamically update the list of keywords used for our political classification.
/// </summary>
namespace BubbleBuster.WordUpdater
{
    public class WordWorker
    {
        //Instance variable
        private static WordWorker _instance;

        private WordWorker()
        {
        }

        /// <summary>
        /// Method to get access to the TweetRetriever.
        /// Is the only static method, because it is not possible to create an instance outside of this class
        /// </summary>
        public static WordWorker Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new WordWorker();
                }
                return _instance;
            }
        }

        //Private method to get the tweets a list of pol users have posted 
        private Dictionary<PolUserObj, List<Tweet>> GetTweets(List<PolUserObj> userList, AuthObj auth)
        {
            Dictionary<PolUserObj, List<Tweet>> returnObj = new Dictionary<PolUserObj, List<Tweet>>();

            foreach (PolUserObj polUser in userList)
            {
                List<Tweet> tweetList = TweetRetriever.Instance.GetTweetsFromUser(polUser.ToUser(), auth);

                returnObj.Add(polUser, tweetList);
            }

            return returnObj;
        }

        //Identifes uncommon words in a list of tweets
        private Dictionary<string, double> IdentifyUncommonWords(List<Tweet> tweetList)
        {
            List<string> commonWords = FileHelper.GetCommonWords();
            Dictionary<string, double> uncommonWords = new Dictionary<string, double>();

            foreach (Tweet tweet in tweetList)
            {
                var puncturation = tweet.Text.Where(Char.IsPunctuation).Distinct().ToArray();
                List<String> wordList = tweet.Text.Split(' ').Select(x => x.Trim(puncturation)).ToList<String>();

                foreach (string listWord in wordList)
                {
                    if (!commonWords.Contains(listWord, StringComparer.InvariantCultureIgnoreCase))
                    {
                        if (!uncommonWords.ContainsKey(listWord))
                        {
                            if (!listWord.StartsWith("https://"))
                            {
                                Console.WriteLine(listWord);
                                double sentiment = tweet.GetSentiment();
                                uncommonWords.Add(listWord, sentiment);
                            }                          
                        }
                    }
                }
            }

            return uncommonWords;
        }

        //Determines each words affiliation
        private Dictionary<string, UncommonWordObj> DetermineWords(Dictionary<PolUserObj, List<Tweet>> dic)
        {
            Dictionary<string, UncommonWordObj> returnObj = new Dictionary<string, UncommonWordObj>();            

            foreach(PolUserObj user in dic.Keys)
            {
                Dictionary<string, double> tempWordList = IdentifyUncommonWords(dic[user]);

                foreach(string word in tempWordList.Keys)
                {
                    if (!returnObj.ContainsKey(word))
                    {
                        returnObj.Add(word, new UncommonWordObj(word));
                    }

                    switch (user.Affiliation)
                    {
                        case -1:
                            if(tempWordList[word] > 1)
                                returnObj[word].LeftPosCount++;
                            else if (tempWordList[word] < -1)
                                returnObj[word].LeftNegCount++;
                            else 
                                returnObj[word].LeftNeuCount++;
                            break;
                        case 0:
                            if (tempWordList[word] > 1)
                                returnObj[word].CenterPosCount++;
                            else if (tempWordList[word] < -1)
                                returnObj[word].CenterNegCount++;
                            else
                                returnObj[word].CenterNeuCount++;
                            break;
                        case 1:
                            if (tempWordList[word] > 1)
                                returnObj[word].RightPosCount++;
                            else if (tempWordList[word] < -1)
                                returnObj[word].RightNegCount++;
                            else
                                returnObj[word].RightNeuCount++;
                            break;
                        default:
                            break;
                    }
                }
            }

            return returnObj;
        } 

        /// <summary>
        /// Method used to update the words based on a set of pol users
        /// </summary>
        /// <param name="users">The pol users</param>
        /// <param name="auth">The auth object</param>
        public void UpdateWords(List<PolUserObj> users, AuthObj auth)
        {
            Dictionary<PolUserObj, List<Tweet>> usersAndTweets = GetTweets(users, auth);
            Dictionary<string, UncommonWordObj> returnList = DetermineWords(usersAndTweets);
            FileHelper.WriteObjectToFile("WordsTest", returnList.Values);
        }
    }
}
