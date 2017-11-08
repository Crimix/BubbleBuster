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
    public class TweetAnalyzer
    {
        private static TweetAnalyzer _instance;
        private Dictionary<string, int> analysisWords = new Dictionary<string, int>();
        private Dictionary<string, int> newsHyperlinks = new Dictionary<string, int>();
        private Dictionary<string, HashtagObj> hashtags = new Dictionary<string, HashtagObj>();

        private TweetAnalyzer()
        {

        }

        public static TweetAnalyzer Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new TweetAnalyzer();
                    _instance.analysisWords = FileHelper.GetAnalysisWords();
                    _instance.newsHyperlinks = FileHelper.GetHyperlinks();
                    _instance.hashtags = FileHelper.GetHashtags();
                }
                return _instance;
            }
        }

        public double AnalyzeAndDecorateTweets(List<Tweet> tweetList)
        {
            double conclusion = 0; //Higher value means more right leaning. Lower value means more left leaning.
            double sentiment = 0;


            List<Tweet> returnList = tweetList;

            returnList = CalculateSentiment(returnList);
            returnList = CalculateHashtagSentiment(returnList);
            returnList = CalculateUrlSentiment(returnList);


            foreach(Tweet tweet in returnList)
            {
                conclusion += tweet.hashtagBias * Constants.HASHTAG_WEIGHT;
                conclusion += tweet.mediaBias * Constants.URL_WEIGHT;
            }

            return conclusion / tweetList.Count;
        }

        //Calculates the general sentiment of a tweet. This is done by looking at the positive and negative words.
        private List<Tweet> CalculateSentiment(List<Tweet> tweetList)
        {
            List<Tweet> returnList = tweetList;

            foreach (Tweet tweet in returnList)
            {
                foreach (string word in analysisWords.Keys)
                {
                    if (tweet.Text.Contains(word))
                    {
                        if (analysisWords[word] == 1)
                            tweet.positiveValue++;
                        else
                            tweet.negativeValue++;
                    }
                }
            }
            return returnList;

        }

        private List<Tweet> CalculateHashtagSentiment(List<Tweet> tweetList)
        {
            List<Tweet> returnList = tweetList;

            foreach (Tweet tweet in returnList)
            {
                foreach (string hashtag in hashtags.Keys)
                {
                    if (tweet.Text.Contains(hashtag))
                    {
                        int sentiment = tweet.getSentiment();

                        if (sentiment > 1)
                            tweet.hashtagBias += hashtags[hashtag].pos;
                        else if (sentiment == 1)
                            tweet.hashtagBias += hashtags[hashtag].bas;
                        else if (sentiment < 1)
                            tweet.hashtagBias += hashtags[hashtag].neg;
                    }
                }
            }

            return returnList;
        }

        private List<Tweet> CalculateUrlSentiment(List<Tweet> tweetList)
        {
            List<Tweet> returnList = tweetList;

            foreach (Tweet tweet in tweetList)
            {
                foreach (Url link in tweet.Entities.Urls)
                {
                    string shortenedUrl = UrlHelper.Instance.ShortenUrl(link.ExpandedUrl);
                    if (newsHyperlinks.Keys.Contains(shortenedUrl))
                    {
                        tweet.mediaBias += newsHyperlinks[shortenedUrl];
                    }
                }
            }

            return returnList;
        }
    }
}
