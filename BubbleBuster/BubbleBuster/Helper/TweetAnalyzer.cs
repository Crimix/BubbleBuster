﻿using BubbleBuster.Web.ReturnedObjects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BubbleBuster.Helper.Objects;
using System.Text.RegularExpressions;
using System.Globalization;
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
        private Dictionary<string, HashtagObj> hashtags = new Dictionary<string, HashtagObj>(StringComparer.InvariantCultureIgnoreCase);

        //Words from file. Has an emotional value used in sentimental analysis.
        private Dictionary<string, int> analysisWords = new Dictionary<string, int>(StringComparer.InvariantCultureIgnoreCase);

        //Formatted URLs from a numer of news media. Each has a politcal value 1-5.
        private Dictionary<string, int> newsHyperlinks = new Dictionary<string, int>(StringComparer.InvariantCultureIgnoreCase);

        private TweetAnalyzer()
        {
        }

        /// <summary>
        /// Returns a static instance of the class. This works a    s a singleton.
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
        /// Splits the tweets int sub lists and analyzes them using tasks
        /// </summary>
        /// <param name="tweetList"></param>
        /// <returns></returns>
        public double[] AnalyzeAndDecorateTweetsThreaded(List<Tweet> tweetList)
        {
            Log.Info("Spliting " + tweetList.Count + " tweets");
            int tweets = tweetList.Count;
            List<Task<double[]>> tasks = new List<Task<double[]>>();
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
                Task<double[]> t = new Task<double[]>(() => AnalyzeAndDecorateTweets(item));
                t.Start();
                tasks.Add(t);
            }

            Task.WaitAll(tasks.ToArray());
            double[] res = { 0.0, 0.0, 0.0, 0.0, 0.0 };

            //Combines the result 
            foreach (var task in tasks)
            {
                res[0] += task.Result[0];
                res[1] += task.Result[1];
                res[2] += task.Result[2];
                res[3] += task.Result[3];
                res[4] += task.Result[4];
            }
            Log.Info("Combining tweets");
            Log.Info("Res " + (res[0] + res[1]));
            

            return res;
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
            double[] output = DoEverything(tweetList); //hashtag, media, count, pos, neg
                                                       /*
                                                       List<Tweet> returnList = tweetList;



                                                       returnList = CalculateSentiment(returnList);        //Calculate the sentiment of each tweet. Positive/negative sentiment.
                                                       returnList = CalculateHashtagSentiment(returnList); //Calculate a political value based on key-words and sentimental context. E.g. "Hate Trump": Negative sentiment, Trump-keyword.
                                                       returnList = CalculateUrlSentiment(returnList);     //

                                                       */

            //List<Tweet> returnList = DoEverything(tweetList);

            /* foreach (Tweet tweet in returnList)
             {
                 output[0] += tweet.hashtagBias * Constants.HASHTAG_WEIGHT;
                 output[1] += tweet.mediaBias * Constants.URL_WEIGHT;
                 output[3] += tweet.negativeValue;
                 output[4] += tweet.positiveValue;
             }

             output[2] = tweetList.Count;
             */
            return output;
        }

        private double[] DoEverything(List<Tweet> tweetList)
        {
            double[] output = { 0.0, 0.0, 0.0, 0.0, 0.0 }; //hashtag, media, count, pos, neg
            List<Tweet> returnList = tweetList;

            foreach (Tweet tweet in returnList)
            {
                tweet.hasQuotes = CheckForQuotationMarks(tweet);

                if (!tweet.hasQuotes)
                {
                    var puncturation = tweet.Text.Where(Char.IsPunctuation).Distinct().ToArray();
                    List<String> wordList = tweet.Text.Split(' ').Select(x => x.Trim(puncturation)).ToList<String>();

                    
                    foreach (string word in wordList)
                    {
                        //Sentiment Analysis
                        if (analysisWords.ContainsKey(word))
                        {
                            int wordValue;

                            if (analysisWords.TryGetValue(word, out wordValue))
                            {
                                if (!tweet.posList.Contains(word) && wordValue == 1)
                                {
                                    tweet.posList.Add(word);
                                    tweet.positiveValue++;
                                }

                                else if (!tweet.negList.Contains(word) && wordValue == -1)
                                {
                                    tweet.negList.Add(word);
                                    tweet.negativeValue++;
                                }
                            }
                        }

                        //Hashtag Analysis
                        if (hashtags.ContainsKey(word) && !tweet.tagList.Contains(word, StringComparer.InvariantCultureIgnoreCase))
                        {
                            HashtagObj hashtagObj;

                            if (hashtags.TryGetValue(word, out hashtagObj))
                            {
                                tweet.tagList.Add(word);

                                int sentiment = tweet.getSentiment();

                                if (sentiment > 1)
                                    tweet.hashtagBias += hashtagObj.pos;
                                else if (sentiment < -1)
                                    tweet.hashtagBias += hashtagObj.neg;
                                else
                                    tweet.hashtagBias += hashtagObj.bas;
                            }
                        }
                    }

                    //Media Analysis
                    foreach (Url link in tweet.Entities.Urls)
                    {
                        string shortenedUrl = UrlHelper.Instance.ShortenUrl(link.ExpandedUrl);
                        if (newsHyperlinks.Keys.Contains(shortenedUrl))
                        {
                            tweet.mediaBias += newsHyperlinks[shortenedUrl];
                        }
                    }

                    output[0] += tweet.hashtagBias * Constants.HASHTAG_WEIGHT;
                    output[1] += tweet.mediaBias * Constants.URL_WEIGHT;
                    output[2] = returnList.Count;
                    output[3] += tweet.negativeValue;
                    output[4] += tweet.positiveValue;
                }
            }
            return output;
        }

        private bool CheckForQuotationMarks(Tweet tweet)
        {
            return tweet.Text.Contains("\"");
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
    }
}
