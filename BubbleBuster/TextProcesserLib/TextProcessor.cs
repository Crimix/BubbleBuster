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

        //initialize regex dictionary
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

        //Puts a token through a series of regexes
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

            if (regexes["negChecker"].IsMatch(procToken))
            {
                isNeg = false;
            }

            procToken = regexes["punctuation"].Replace(procToken, "");

            if (isNeg && !String.IsNullOrWhiteSpace(procToken))
            {
                //procToken = String.Concat("NEG_", procToken);
            }

            if (KeyWords.NotWordsList.Contains(procToken))
            {
                isNeg = true;
            }
            
            return procToken;
        }

        //Tokenize list of tweets 
        public string[][] Tokenizer(List<string> tweets)
        {
            List<List<string>> tokens = new List<List<string>>();

            string stemmedWord;

            foreach (string tweet in tweets)
            {
                isNeg = false;
                List<string> tokenTweet = new List<string>();

                foreach (var token in tweet.ToLower().Split(' '))
                {
                    if (!KeyWords.StopWordsList.Contains(token))
                    {
                        stemmedWord = ProcessToken(token, isNeg);
                        stemmedWord = stem.StemWord(token);

                        tokenTweet.Add(stemmedWord);
                    }
                }
                tokens.Add(tokenTweet);
            }

            string[][] res = new string[tokens.Count][];

            for (int i = 0; i < tokens.Count; i++)
            {
                res[i] = tokens[i].ToArray();
            }
            return res;
        }

    }
}