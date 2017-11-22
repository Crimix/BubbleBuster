using BubbleBuster.Web.ReturnedObjects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BubbleBuster.Helper.Objects;
using System.Text.RegularExpressions;
/// <summary>
/// This class is used to classify the political leaning of a user, based on the content of the tweets they post.
/// This is done by looking at the media they share (political leaning of different news outlets. This info is stored in a file, 
/// and is gathered fro the site: Allsides.com).
/// We also determine this by looking at the words a user use, and the context in which they are used.
/// This is done using a sentiment analysis to determine if the user is happy/angry/passive, and a list of contextual keywords.
/// The keywords has a value (negative for left leaning and positive for right.). Each word has a distinct such value for each context:
/// positive/negative/passive.
/// </summary>
namespace BubbleBuster.Helper
{
    public class TweetAnalyzer
    {
        private static TweetAnalyzer _instance;

        //"Hashtags" are key-words used in analysis. Each has a number of fields determining their political value in a given positive/negative sentimental context.
        private Dictionary<string, HashtagObj> hashtags = new Dictionary<string, HashtagObj>();

        //Words from file. Has an emotional value used in sentimental analysis.
        private Dictionary<string, int> analysisWords = new Dictionary<string, int>();

        //Formatted URLs from a numer of news media. Each has a politcal value 1-5.
        private Dictionary<string, int> newsHyperlinks = new Dictionary<string, int>(); 
       
        

        private TweetAnalyzer()
        {

        }

        /// <summary>
        /// Returns a static instance of the class. This works as a singleton.
        /// </summary>
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

        /// <summary>
        /// Analyzes a list of Tweets, and returns the following values as a double-array:
        /// Hashtag-Bias: Determined political value of the words used in the tweets
        /// Media-Bias: Determined political value of news media linked to in the tweets
        /// Count: Number of tweets analtyzed
        /// Pos: Number of positive words found
        /// Neg: Number of negative words found
        /// </summary>
        /// <param name="tweetList"></param>
        /// <returns></returns>
        public double[] AnalyzeAndDecorateTweets(List<Tweet> tweetList)
        {
            //double conclusion = 0; //Higher value means more right leaning. Lower value means more left leaning.

            double[] output = { 0.0, 0.0, 0.0, 0.0, 0.0}; //hashtag, media, count, pos, neg

            List<Tweet> returnList = tweetList;

            returnList = CalculateSentiment(returnList);        //Calculate the sentiment of each tweet. Positive/negative sentiment.
            returnList = CalculateHashtagSentiment(returnList); //Calculate a political value based on key-words and sentimental context. E.g. "Hate Trump": Negative sentiment, Trump-keyword.
            returnList = CalculateUrlSentiment(returnList);     //


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
