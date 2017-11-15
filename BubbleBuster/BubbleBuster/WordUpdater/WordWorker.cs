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

        private List<string> IdentifyUncommonWords(List<Tweet> tweetList)
        {
            List<string> commonWords = FileHelper.GetCommonWords();
            List<string> uncommonWords = new List<string>();

            foreach (Tweet tweet in tweetList)
            {
                var puncturation = tweet.Text.Where(Char.IsPunctuation).Distinct().ToArray();
                List<String> wordList = tweet.Text.Split(' ').Select(x => x.Trim(puncturation)).ToList<String>();

                foreach (string listWord in wordList)
                {
                    if (!commonWords.Contains(listWord, StringComparer.InvariantCultureIgnoreCase))
                    {
                        if (!uncommonWords.Contains(listWord, StringComparer.InvariantCultureIgnoreCase))
                        {
                            uncommonWords.Add(listWord);
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
                List<string> tempWordList = new List<string>();
                tempWordList.AddRange(IdentifyUncommonWords(dic[user]));

                foreach(string word in tempWordList)
                {
                    if (!returnObj.ContainsKey(word))
                    {
                        returnObj.Add(word, new UncommonWordObj(word));
                    }

                    switch (user.affiliation)
                    {
                        case -1:
                            returnObj[word].LeftCount++;
                            break;
                        case 0:
                            returnObj[word].CenterCount++;
                            break;
                        case 1:
                            returnObj[word].RightCount++;
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

            FileHelper.WriteObjectToFile("abc", "WordsTest", returnList.Values);
        }
    }
}
