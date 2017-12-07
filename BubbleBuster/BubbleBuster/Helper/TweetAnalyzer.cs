using BubbleBuster.Helper.Objects;
using BubbleBuster.Web.ReturnedObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

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
        //Instance variable
        private static TweetAnalyzer _instance;

        //"Keywords" are key-words used in analysis. Each has a number of fields determining their political value in a given positive/negative sentimental context.
        private Dictionary<string, KeywordObj> keywords = new Dictionary<string, KeywordObj>(StringComparer.InvariantCultureIgnoreCase);

        //Words from file. Has an emotional value used in sentimental analysis.
        private Dictionary<string, int> analysisWords = new Dictionary<string, int>(StringComparer.InvariantCultureIgnoreCase);

        //Formatted URLs from a numer of news media. Each has a politcal value 1-5.
        private Dictionary<string, int> newsHyperlinks = new Dictionary<string, int>(StringComparer.InvariantCultureIgnoreCase);

        //Private constructor such that it is a singleton 
        private TweetAnalyzer()
        {
        }

        /// <summary>
        /// Returns a static instance of the class
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
                    _instance.keywords = FileHelper.GetKeywords();
                }
                return _instance;
            }
        }

        /// <summary>
        /// Splits the tweets int sub lists and analyzes them using tasks
        /// </summary>
        /// <param name="tweetList"></param>
        /// <returns></returns>
        public AnalysisResultObj AnalyzeAndDecorateTweetsThreaded(List<Tweet> tweetList)
        {
            Log.Debug("Spliting " + tweetList.Count + " tweets");
            int tweets = tweetList.Count;
            List<Task<AnalysisResultObj>> tasks = new List<Task<AnalysisResultObj>>();
            var copyTweetList = tweetList;
            int e = tweetList.Count / Constants.TWEET_LIST_AMOUNT;
            List<List<Tweet>> splittedList = new List<List<Tweet>>();
            int tweetsSplitted = 0;

            // Splits to the max number of lists
            for (int i = 0; i < Constants.TWEET_LIST_AMOUNT; i++)
            {
                tweetsSplitted += e;
                splittedList.Add(copyTweetList.Take(e).ToList());
                copyTweetList = copyTweetList.Skip(e).ToList();

            }

            //If some tweets are not covered, adds another list such that all tweets are analyzed
            if (tweetsSplitted < tweets)
            {
                splittedList.Add(copyTweetList);
            }

            //Starts the tasks
            foreach (var item in splittedList)
            {
                Task<AnalysisResultObj> t = new Task<AnalysisResultObj>(() => AnalyzeAndDecorateTweets(item));
                t.Start();
                tasks.Add(t);
            }

            Task.WaitAll(tasks.ToArray());
            AnalysisResultObj res = new AnalysisResultObj();

            //Combines the result 
            foreach (var task in tasks)
            {
                res.KeywordBias += task.Result.KeywordBias;
                res.MediaBias += task.Result.MediaBias;
                res.Count += task.Result.Count;
                res.NegativeSentiment += task.Result.NegativeSentiment;
                res.PositiveSentiment += task.Result.PositiveSentiment;
            }
            Log.Debug("Combining tweets");
            Log.Debug("Result " + res.GetAlgorithmResult());

            return res;
        }

        /// <summary>
        /// Sentiment analysis for the tweet based on the word
        /// </summary>
        /// <param name="word">The word</param>
        /// <param name="tweet">The tweet</param>
        private void SentimentAnalysis(List<string> wordList, Tweet tweet)
        {
            foreach (string word in wordList)
            {
                if (analysisWords.ContainsKey(word) && analysisWords.TryGetValue(word, out int wordValue))
                {
                    if (wordValue == 1)
                    {
                        tweet.PosList.Add(word);
                        tweet.PositiveValue++;
                    }

                    else if (wordValue == -1)
                    {
                        tweet.NegList.Add(word);
                        tweet.NegativeValue++;
                    }
                }
            }
        }

        /// <summary>
        /// Keyword analysis of a tweet 
        /// </summary>
        /// <param name="keyword">The keyword</param>
        /// <param name="tweet">The tweet</param>
        private void KeywordAnalysis(List<string> wordList, Tweet tweet)
        {
            foreach (string word in wordList)
            {
                if (keywords.ContainsKey(word) &&
                    !tweet.TagList.Contains(word, StringComparer.InvariantCultureIgnoreCase) &&
                    keywords.TryGetValue(word, out KeywordObj keywordObj))
                {
                    tweet.TagList.Add(word);

                    int sentiment = tweet.GetSentiment();

                    if (sentiment > 1)
                        tweet.KeywordBias += keywordObj.Pos;
                    else if (sentiment < -1)
                        tweet.KeywordBias += keywordObj.Neg;
                    else
                        tweet.KeywordBias += keywordObj.Bas;
                }
            }
        }

        /// <summary>
        /// Media analysis of a tweets links
        /// </summary>
        /// <param name="tweet">The tweet</param>
        private void MediaAnalysis(Tweet tweet)
        {
            foreach (Url link in tweet.Entities.Urls)
            {
                string shortenedUrl = UrlHelper.Instance.ShortenUrl(link.ExpandedUrl);
                if (newsHyperlinks.ContainsKey(shortenedUrl))
                {
                    tweet.MediaBias += newsHyperlinks[shortenedUrl];
                }
            }
        }


        /// <summary>
        /// Analyzes a list of Tweets, and returns the following values as a double-array:
        /// Keyword-Bias: Determined political value of the words used in the tweets
        /// Media-Bias: Determined political value of news media linked to in the tweets
        /// Count: Number of tweets analtyzed
        /// Pos: Number of positive words found
        /// Neg: Number of negative words found
        /// </summary>
        /// <param name="tweetList">The list of tweets</param>
        /// <returns>AnalysisResultObj</returns>
        public AnalysisResultObj AnalyzeAndDecorateTweets(List<Tweet> tweetList)
        {
            AnalysisResultObj output = new AnalysisResultObj();

            foreach (Tweet tweet in tweetList)
            {
                tweet.HasQuotes = CheckForQuotationMarks(tweet);

                if (!tweet.HasQuotes)
                {
                    var puncturation = tweet.Text.Where(Char.IsPunctuation).Distinct().ToArray();
                    List<String> wordList = tweet.Text.Split(' ').Select(x => x.Trim(puncturation)).ToList<String>();

                    SentimentAnalysis(wordList, tweet);
                    KeywordAnalysis(wordList, tweet);
                    MediaAnalysis(tweet);

                    if (tweet.GetSentiment() > 0)
                        output.PositiveTweetsCount++;
                    else if (tweet.GetSentiment() < 0)
                        output.NegativeTweetsCount++;


                    if ((tweet.KeywordBias + tweet.MediaBias) != 0)
                    {
                        output.PolCount++;

                        output.KeywordBias += tweet.KeywordBias;
                        output.MediaBias += tweet.MediaBias;
                        output.NegativeSentiment += tweet.NegativeValue;
                        output.PositiveSentiment += tweet.PositiveValue;
                    }

                    //output.KeywordBias += tweet.KeywordBias;
                    //output.MediaBias += tweet.MediaBias;
                    //output.NegativeSentiment += tweet.NegativeValue;
                    //output.PositiveSentiment += tweet.PositiveValue;
                }
            }
            output.Count = tweetList.Count;
            return output;
        }

        /// <summary>
        /// Returns true if there are any quotation marks in the tweet.
        /// </summary>
        /// <param name="tweet">The tweet</param>
        /// <returns>True if any "</returns>
        private bool CheckForQuotationMarks(Tweet tweet)
        {
            return tweet.Text.Contains("\"");
        }

        /// <summary>
        /// Returns true if there are any quotes in the tweet
        /// Also decorates the tweet with the quotes
        /// </summary>
        /// <param name="tweet">The tweet</param>
        /// <returns>True if any quotes</returns>
        public bool CheckForQuotes(Tweet tweet)
        {
            Regex regex = new Regex("\"(.*?)\"");

            if (regex.IsMatch(tweet.Text))
            {
                tweet.Quotes = regex.Matches(tweet.Text)
                               .Cast<Match>()
                               .Select(m => m.Value)
                               .ToArray();
                return true;
            }
            return false;
        }
    }
}
