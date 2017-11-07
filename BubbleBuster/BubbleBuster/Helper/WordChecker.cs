using BubbleBuster.Web.ReturnedObjects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BubbleBuster.Helper.HelperObjects;

namespace BubbleBuster.Helper
{
    public class WordChecker
    {
        private static WordChecker _instance;
        private Dictionary<string, int> analysisWords = new Dictionary<string, int>();
        private Dictionary<string, int> newsHyperlinks = new Dictionary<string, int>();
        private Dictionary<string, HashtagObj> hashtags = new Dictionary<string, HashtagObj>();

        private WordChecker()
        {

        }

        public static WordChecker Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new WordChecker();
                    _instance.analysisWords = FileHelper.GetAnalysisWords();
                    _instance.newsHyperlinks = FileHelper.GetHyperlinks();
                    _instance.hashtags = FileHelper.GetHashtags();
                }
                return _instance;
            }
        }

        public void CalculateSentiment(List<Tweet> tweetList)
        {
            foreach (Tweet tweet in tweetList)
            {
                foreach (string word in analysisWords.Keys)
                {
                    if (tweet.Text.Contains(word))
                    {
                        tweet.ImportantWords.Add(word);
                        tweet.EmotionValue += analysisWords[word];
                    }
                }
            }

        }

        public void CalculateHashtagSentiment(List<Tweet> tweetList)
        {
            foreach (Tweet tweet in tweetList)
            {
                foreach (string hashtag in hashtags.Keys)
                {
                    if (tweet.Text.Contains(hashtag))
                    {
                        if (tweet.EmotionValue > 0)
                        {

                        }
                    }
                }
            }
        }

        public void CheckTweetsForHyperlinks(List<Tweet> tweetList)
        {
            double totalScore = 0;
            double tweetNr = 0;

            foreach (Tweet tweet in tweetList)
            {
                foreach (Url link in tweet.Entities.Urls)
                {
                    string shortenedUrl = UrlHelper.Instance.ShortenUrl(link.ExpandedUrl);
                    if (newsHyperlinks.Keys.Contains(shortenedUrl))
                    {
                        tweetNr++;

                        if (tweet.Bias == 0)
                        {
                            tweet.Bias = newsHyperlinks[shortenedUrl];
                            totalScore += tweet.Bias;
                        }
                        else
                        {
                            tweet.Bias = (tweet.Bias + newsHyperlinks[shortenedUrl]) / 2;
                            totalScore += tweet.Bias;
                        }

                        //tweet.NewsHyperlinks.Add(shortenedUrl, newsHyperlinks[shortenedUrl]);
                    }
                }
            }

            Console.WriteLine("Your average political bias is: " + totalScore / tweetNr);
        }
    }
}
