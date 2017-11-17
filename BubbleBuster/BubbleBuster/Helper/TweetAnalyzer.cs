using BubbleBuster.Web.ReturnedObjects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BubbleBuster.Helper.Objects;
using System.Text.RegularExpressions;

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

        public double[] AnalyzeAndDecorateTweets(List<Tweet> tweetList)
        {
            //double conclusion = 0; //Higher value means more right leaning. Lower value means more left leaning.

            double[] output = { 0.0, 0.0, 0.0, 0.0, 0.0}; //hashtag, media, count, pos, neg

            List<Tweet> returnList = tweetList;

            returnList = CalculateSentiment(returnList);
            returnList = CalculateHashtagSentiment(returnList);
            returnList = CalculateUrlSentiment(returnList);


            foreach (Tweet tweet in returnList)
            {
                if (!CheckForQuotes(tweet))
                {
                    output[0] += tweet.hashtagBias * Constants.HASHTAG_WEIGHT;
                    output[1] += tweet.mediaBias * Constants.URL_WEIGHT;
                    output[3] += tweet.negativeValue;
                    output[4] += tweet.positiveValue;
                }
            }

            output[2] = tweetList.Count;

            return output;
        }

        public bool CheckForQuotes(Tweet tweet)
        {
            Regex regex = new Regex("\"(.*?)\"");

            if (regex.IsMatch(tweet.Text))
            {
                tweet.quotes = regex.Matches(tweet.Text)
                               .Cast<Match>()
                               .Select(m => m.Value)
                               .ToArray();
                return true;
            }
            return false;
        }

        //Calculates the general sentiment of a tweet. This is done by looking at the positive and negative words.
        public List<Tweet> CalculateSentiment(List<Tweet> tweetList)
        {
            List<Tweet> returnList = tweetList;

            foreach (Tweet tweet in returnList)
            {
                foreach (string word in analysisWords.Keys)
                {
                    var puncturation = tweet.Text.Where(Char.IsPunctuation).Distinct().ToArray();
                    List<String> wordList = tweet.Text.Split(' ').Select(x => x.Trim(puncturation)).ToList<String>();

                    foreach (string iWord in wordList)
                    {
                        if (iWord.Equals(word, StringComparison.InvariantCultureIgnoreCase))
                        {
                            if (!tweet.posList.Contains(word) && analysisWords[word] == 1)
                            {
                                tweet.posList.Add(word);
                                tweet.positiveValue++;
                            }

                            else if (!tweet.negList.Contains(word) && analysisWords[word] == -1)
                            {
                                tweet.negList.Add(word);
                                tweet.negativeValue++;
                            }
                        }
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
                    var puncturation = tweet.Text.Where(Char.IsPunctuation).Distinct().ToArray();
                    List<String> wordList = tweet.Text.Split(' ').Select(x => x.Trim(puncturation)).ToList<String>();

                    foreach(string iWord in wordList)
                    {
                        if(iWord.Equals(hashtag, StringComparison.InvariantCultureIgnoreCase) && !tweet.tagList.Contains(hashtag))
                        {
                            tweet.tagList.Add(hashtag);

                            int sentiment = tweet.getSentiment();

                            if (sentiment > 1)
                                tweet.hashtagBias += hashtags[hashtag].pos;
                            else if (sentiment < -1)
                                tweet.hashtagBias += hashtags[hashtag].neg;
                            else
                                tweet.hashtagBias += hashtags[hashtag].bas;
                        }
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

        private bool InsensitiveContains(string source, string toCheck)
        {
            return source != null && toCheck != null && source.IndexOf(toCheck, StringComparison.OrdinalIgnoreCase) >= 0;
        }
    }
}
