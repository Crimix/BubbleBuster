using BubbleBuster.Helper;
using BubbleBuster.Web.ReturnedObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BubbleBuster.WordUpdater
{
    public class WordWorker
    {
        private static WordWorker _instance;

        private WordWorker()
        {

        }

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


        private Dictionary<PolUserObj, List<Tweet>> GetTweets(List<PolUserObj> userList)
        {
            Dictionary<PolUserObj, List<Tweet>> returnObj = new Dictionary<PolUserObj, List<Tweet>>();

            foreach (PolUserObj polUser in userList)
            {
                List<Tweet> tweetList = TweetRetriever.Instance.GetTweetsFromUser(polUser.twitterId);
                returnObj.Add(polUser, tweetList);
            }

            return returnObj;

        }

        private Dictionary<string, double> IdentifyUncommonWords(List<Tweet> tweetList)
        {
            List<string> commonWords = FileHelper.GetCommonWords();
            Dictionary<string, double> uncommonWords = new Dictionary<string, double>();

            foreach (Tweet tweet in tweetList)
            {
                double sentiment = tweet.getSentiment();

                var puncturation = tweet.Text.Where(Char.IsPunctuation).Distinct().ToArray();
                List<String> wordList = tweet.Text.Split(' ').Select(x => x.Trim(puncturation)).ToList<String>();

                foreach (string listWord in wordList)
                {
                    if (!commonWords.Contains(listWord, StringComparer.InvariantCultureIgnoreCase))
                    {
                        if (!uncommonWords.Keys.Contains(listWord, StringComparer.InvariantCultureIgnoreCase))
                        {
                            if (!listWord.StartsWith("https://"))
                            {
                                Console.WriteLine(listWord);
                                uncommonWords.Add(listWord, sentiment);
                            }
                            
                        }
                    }
                }
            }

            return uncommonWords;
        }

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

                    switch (user.affiliation)
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

        public void UpdateWords(List<PolUserObj> users)
        {
            Dictionary<PolUserObj, List<Tweet>> usersAndTweets = GetTweets(users);
            Dictionary<string, UncommonWordObj> returnList = DetermineWords(usersAndTweets);

            FileHelper.WriteObjectToFile("WordsTest", returnList.Values);
        }
    }
}
