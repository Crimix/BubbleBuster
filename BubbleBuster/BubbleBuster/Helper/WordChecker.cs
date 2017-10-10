﻿using BubbleBuster.Web.ReturnedObjects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BubbleBuster.Helper
{
    public class WordChecker
    {
        private static WordChecker _instance;
        private Dictionary<string, int> analysisWords = new Dictionary<string, int>();
        private Dictionary<string, int> newsHyperlinks = new Dictionary<string, int>();

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
                }
                return _instance;
            }
        }

        public bool CheckTweetForWords(Tweet tweet)
        {
            foreach(string word in analysisWords.Keys)
            {
                if (tweet.Text.Contains(word))
                {
                    tweet.ImportantWords.Add(word);
                }
            }

            return tweet.ImportantWords.Count > 0;
        }

        public void CheckTweetForHyperlink(List<Tweet> tweetList)
        {
            int totalScore = 0;
            int tweetNr = 0;

            foreach (Tweet tweet in tweetList)
            {
                foreach (Url link in tweet.Entities.Urls)
                {
                    string shortenedUrl = UrlHelper.Instance.ShortenUrl(link.ExpandedUrl);
                    if (newsHyperlinks.Keys.Contains(shortenedUrl))
                    {
                        tweetNr++;
                        totalScore += newsHyperlinks[shortenedUrl];
                        tweet.NewsHyperlinks.Add(shortenedUrl, newsHyperlinks[shortenedUrl]);
                    }
                }
            }

            Console.WriteLine("Your average political bias is: " + totalScore / tweetNr);
        }
    }
}
