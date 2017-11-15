using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using System.IO;
using BubbleBuster.Web.ReturnedObjects;



namespace TextProcesserLib
{
    class TextProcesser
    {
        Dictionary<string, int> wordBag = new Dictionary<string, int>();
        Dictionary<string, Regex> regexes;

        double[][] vectorTweets;

        public double[][] VectorTweets { get => vectorTweets; }
        public List<int> result = new List<int>();

        bool isNeg;
        Stemmer stem = new Stemmer();

        //initialize regex dictionary
        public TextProcesser()
        {
            this.regexes = new Dictionary<string, Regex>
            {
                {
                    "multiSpaces",
                    new Regex("[ ]{2,}")
                },
                {
                    "negChecker",
                    new Regex("[.:;!?]")
                },

                {
                     "lineBreaks",
                     new Regex(@"[\u000A\u000B\u000C\u000D\u2028\u2029\u0085]+")
                },
                {
                    "users",
                    new Regex(@"@[^\s]+")
                },
                {
                    "links",
                    new Regex(@"((www\.[^\s]+)|(https?://[^\s]+))")
                },
                {
                    "hashtags",
                    new Regex(@"#")
                },
                {
                    "apostrophe",
                    new Regex(@"'")
                },
                {
                    "duplicateLetters",
                    new Regex(@"(.)\1{2,}")
                },
                {
                    "punctuation",
                    new Regex(@"([!-/]|[;-?)")
                },
                {
                    "numbers",
                    new Regex("[0-9]")
                }
            };
        }

        //Puts a token through a series of regexes
        string RegexText(string token, bool _isNeg)
        {
            string procToken = token;

            procToken = regexes["links"].Replace(procToken, "LINK");
            procToken = regexes["users"].Replace(procToken, "AT_USER");
            procToken = regexes["lineBreaks"].Replace(procToken, " ");
            procToken = regexes["hashtags"].Replace(procToken, "");
            procToken = regexes["apostrophe"].Replace(procToken, "");
            procToken = regexes["duplicateLetters"].Replace(procToken, "$1");

            if (regexes["negChecker"].IsMatch(procToken))
            {
                isNeg = false;
            }

            procToken = regexes["punctuation"].Replace(procToken, "");
            procToken = regexes["numbers"].Replace(procToken, "");
            procToken = regexes["multiSpaces"].Replace(procToken, "");

            if (isNeg && !String.IsNullOrWhiteSpace(procToken))
            {
                procToken = String.Concat("NEG_", procToken);
            }

            if (KeyWords.NotWordsList.Contains(procToken))
            {
                isNeg = true;
            }

            return procToken;
        }

        //Process a string with regexes and negation
        public List<string> TextProcessTweet(string tweet)
        {
            List<string> tokens = new List<string>();
            isNeg = false;

            string procText;

            foreach (var token in tweet.ToLower().Split(' '))
            {
                if (!KeyWords.StopWordsList.Contains(token))
                {
                    procText = RegexText(token, isNeg);

                    if (!String.IsNullOrWhiteSpace(procText))
                    {
                        tokens.Add(procText);
                    }
                }
            }

            return tokens;
        } 

        //unigram
        //converts a tweet to a list a where each words corresponds to an int
        Dictionary<int, double> VectorizeUnigram(string _tweet)
        {
            //string tweet = ProcessTweet(_tweet);
            List<int> intVector = new List<int>();

            string stemmedWord;

            isNeg = false;

            Dictionary<int, double> termFrequency = new Dictionary<int, double>();

            foreach (var token in _tweet.ToLower().Split(' '))
            {
                if (!KeyWords.StopWordsList.Contains(token))
                {
                    stemmedWord = RegexText(token, isNeg);

                    if (String.IsNullOrWhiteSpace(token))
                    {
                        continue;
                    }

                    stemmedWord = stem.StemWord(token);
                    if (wordBag.TryGetValue(stemmedWord, out int intWord)) //has the word been seen before?
                    {
                        if (termFrequency.TryGetValue(intWord, out double frequency)) //are there multiples of the word in the tweet?
                        {
                            termFrequency[intWord] = frequency + 1;
                        }
                        else
                        {
                            termFrequency.Add(intWord, 1);
                        }
                    }
                    else
                    {

                        int nrOfWords = wordBag.Count;
                        wordBag.Add(stemmedWord, nrOfWords);
                        termFrequency.Add(nrOfWords, 1);
                    }
                }
            }

            return termFrequency;
        }

        //Reads a JSON tweet, converts it to a list<int> (unigram) and add it to a list .
        public void TweetsToVector(string path, string filename)
        {
            var json = JsonConvert.DeserializeObject<List<Tweet>>(File.ReadAllText(path + filename));

            List<Dictionary<int, double>> terms = new List<Dictionary<int, double>>();
            int i = 0;
            foreach (var item in json)
            {
                if (i > 30)
                {
                    break;
                }
                else
                {
                    terms.Add(VectorizeUnigram(item.Text));
                    i++;
                }
            }

            FormatVectors(terms);
        }

        //converts list<dicts> to correct double[][] format
        void FormatVectors(List<Dictionary<int, double>> dicts)
        {
            int max = wordBag.Count;
            int nrOfVectors = dicts.Count;

            double[] vector = new double[max];
            vectorTweets = new double[nrOfVectors][];

            for (int i = 0; i < nrOfVectors; i++)
            {
                for (int j = 0; j < max; j++)
                {
                    if (dicts[i].TryGetValue(j, out double frequency))
                    {
                        vector[j] = frequency;
                    }
                    else
                    {
                        vector[j] = 0;
                    }
                }
                vectorTweets[i] = vector;
            }
        }
    }
}