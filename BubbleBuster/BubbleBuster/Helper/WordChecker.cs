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
        private List<string> ImportantWords = new List<string>();

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
                        _instance.ImportantWords.AddRange(File.ReadAllLines("important_words"));
                    }
                    catch(Exception e)
                    {
                        Console.WriteLine("Important Words file not loaded: " + e.Message);
                    }
                    
                }
                return _instance;
            }
        }

        public void checkTweetForWords(Tweet tweet)
        {
            List<string> returnList = new List<string>();
            foreach(string word in ImportantWords)
            {
                if (tweet.Text.Contains(word))
                {
                    tweet.ImportantWords.Add(word);
                }
            }
        }
    }
}
