using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace TextProcesserLib
{
    public class TextProcessor
    {
        Dictionary<string, Regex> regexes;

        bool isNeg;
        Stemmer stem = new Stemmer();

        /// <summary>
        /// Initialize regex dictionaries
        /// </summary>
        public TextProcessor()
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
                     new Regex(@"([\u000A\u000B\u000C\u000D\u2028\u2029\u0085]+|(\\nl))")
                },
                {
                    "users",
                    new Regex(@"@[^\s]+")
                },
                {
                    "links",
                    new Regex(@"((www\.[^\s]+)|(https?:\/[^\s]+))")
                },
                {
                    "hashtags",
                    new Regex(@"#")
                },
                {
                    "duplicateLetters",
                    new Regex(@"(.)\1{2,}")
                },
                {
                    "punctuation",
                    new Regex(@"([!-/]|[;-?]|['])")
                },
                {
                    "numbers",
                    new Regex("[0-9]")
                }
            };
        }


        /// <summary>
        /// Puts a token through a series of regexes
        /// </summary>
        /// <param name="token"> A token </param>
        /// <param name="_isNeg"> Decides if we add "NEG_" in front of the token </param>
        /// <returns> A processed token </returns>
        string ProcessToken(string token, bool _isNeg)
        {
            string procToken = token;

            procToken = regexes["links"].Replace(procToken, "LINK");
            procToken = regexes["users"].Replace(procToken, "AT_USER");
            procToken = regexes["lineBreaks"].Replace(procToken, "");
            procToken = regexes["hashtags"].Replace(procToken, "");
            procToken = regexes["duplicateLetters"].Replace(procToken, "$1");
            procToken = regexes["numbers"].Replace(procToken, "");
            procToken = regexes["multiSpaces"].Replace(procToken, "");


            /* Not currently used as it decreases accuracy of NaiveBayes
             * As there are too few training samples
            if (regexes["negChecker"].IsMatch(procToken))
            {
                isNeg = false;
            }
            if (isNeg && !String.IsNullOrWhiteSpace(procToken))
            {
                procToken = String.Concat("NEG_", procToken);
            }
            if (KeyWords.NotWordsList.Contains(procToken))
            {
                isNeg = true;
            }
            */

            procToken = regexes["punctuation"].Replace(procToken, "");

            return procToken;
        }


        /// <summary>
        /// Tokenize list of tweets
        /// </summary>
        /// <param name="tweets">A list of strings</param>
        /// <returns> A jagged string array </returns>
        public string[][] Tokenizer(List<string> tweets)
        {
            List<List<string>> tokenizedTweets = new List<List<string>>();

            string feature;

            foreach (string tweet in tweets)
            {
                isNeg = false;
                List<string> tokens = new List<string>();

                //process each token in a tweet with regexes and stemming
                foreach (string token in tweet.ToLower().Split(' '))
                {
                    if (!KeyWords.StopWordsList.Contains(token))
                    {
                        feature = ProcessToken(token, isNeg);
                        feature = stem.StemWord(feature);

                        tokens.Add(feature);
                    }
                }
                tokenizedTweets.Add(tokens);
            }

            //Transform to string[][] format
            string[][] res = new string[tokenizedTweets.Count][];
            for (int i = 0; i < tokenizedTweets.Count; i++)
            {
                res[i] = tokenizedTweets[i].ToArray();
            }
            return res;
        }

    }
}