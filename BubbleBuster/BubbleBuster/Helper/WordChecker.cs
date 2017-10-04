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
        private List<string> importantWords = new List<string>();
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
                    try
                    {                        
                        _instance.importantWords.AddRange(File.ReadAllLines("important_words"));
                    }
                    catch(Exception e)
                    {
                        Console.WriteLine("Important Words file not loaded: " + e.Message);
                    }

                    try
                    {
                        foreach (string link in File.ReadAllLines("news_hyperlinks"))
                        {
                            string[] arr = link.Split(';');
                            _instance.newsHyperlinks.Add(arr[0], Convert.ToInt32(arr[1]));
                        }

                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("News Sources file not loaded: " + e.Message);
                    }

                }
                return _instance;
            }
        }

        public bool CheckTweetForWords(Tweet tweet)
        {
            foreach(string word in importantWords)
            {
                if (tweet.Text.Contains(word))
                {
                    tweet.ImportantWords.Add(word);
                }
            }

            return tweet.ImportantWords.Count > 0;
        }

        public void CheckTweetForHyperlink(Tweet tweet)
        {
            foreach (Url link in tweet.Entities.Urls)
            {
                if (newsHyperlinks.Keys.Contains(link.ExpandedUrl))
                {
                    
                }
            }
        }
    }
}
