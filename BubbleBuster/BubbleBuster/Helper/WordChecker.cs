using BubbleBuster.Web.ReturnedObjects;
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
        string impWordsFilename = "important_words";
        string hyperlinksFilename = "news_hyperlinks";

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

        public bool CheckTweetForHyperlink(Tweet tweet)
        {
            foreach (Url link in tweet.Entities.Urls)
            {
                string shortenedUrl = UrlHelper.Instance.ShortenUrl(link.ExpandedUrl);
                if (newsHyperlinks.Keys.Contains(shortenedUrl))
                {
                    tweet.NewsHyperlinks.Add(shortenedUrl, newsHyperlinks[shortenedUrl]);
                }
            }

            return tweet.NewsHyperlinks.Count > 0;
        }
    }
}
