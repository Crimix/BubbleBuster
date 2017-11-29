using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BubbleBuster.Helper.Objects;
using Accord.IO;
using Accord.MachineLearning.Bayes;
using Accord.Statistics.Distributions.Univariate;

namespace BubbleBuster.Helper
{
    public static class FileHelper
    {
        static Dictionary<string, int> newsHyperlinks;
        static Dictionary<string, HashtagObj> hashtags;
        static Dictionary<string, int> analysisWords; //Value: -1=negativeWord, 1=positiveWord
        static List<string> commonWords;
        static string hyperlinkFilePath = Constants.PROGRAM_DATA_FILEPATH + @"\" + "news_hyperlinks";
        static string commonWordsFilePath = Constants.PROGRAM_DATA_FILEPATH + @"\" + "commonWords";
        static string posWordsFilePath = Constants.PROGRAM_DATA_FILEPATH + @"\" + "positive-words";
        static string negWordsFilePath = Constants.PROGRAM_DATA_FILEPATH + @"\" + "negative-words";
        static string hashtagsFilePath = Constants.PROGRAM_DATA_FILEPATH + @"\" + "hashtags";

        public static Dictionary<string, int> GetHyperlinks()
        {
            if(newsHyperlinks != null)
            {
                return newsHyperlinks;
            }

            newsHyperlinks = new Dictionary<string, int>();

            try
            {
                foreach (string link in File.ReadAllLines(hyperlinkFilePath))
                {
                    string[] arr = link.Split(';');
                    newsHyperlinks.Add(arr[0], Convert.ToInt32(arr[1]));
                }
            }
            catch (FileNotFoundException e)
            {
                Log.Error("News Source file not loaded: " + e.Message);
            }
            
            return newsHyperlinks;
        }
        
        public static Dictionary<string, int> GetAnalysisWords()
        {
            if (analysisWords != null)
            {
                return analysisWords;
            }

            analysisWords = new Dictionary<string, int>();

            try
            {
                foreach (string word in File.ReadAllLines(posWordsFilePath).Skip(35)) //Skip: Start reading from line 36
                {
                    if(!analysisWords.ContainsKey(word))
                        analysisWords.Add(word, 1);
                }

                foreach (string word in File.ReadAllLines(negWordsFilePath).Skip(35))
                {
                    if (!analysisWords.ContainsKey(word))
                        analysisWords.Add(word, -1);
                }
            }
            catch (FileNotFoundException e)
            {
                Log.Error("Words Source file not loaded: " + e.Message);
            }

            return analysisWords;
        }

        public static Dictionary<string, HashtagObj> GetHashtags()
        {
            if (hashtags != null)
            {
                return hashtags;
            }

            hashtags = new Dictionary<string, HashtagObj>();

            try
            {
                List<String> temp = File.ReadAllLines(hashtagsFilePath).Skip(7).ToList<String>();

                foreach (string hashtag in temp)
                {
                    string[] tempArray = hashtag.Split(';'); 
                    hashtags.Add(tempArray[0], new HashtagObj(hashtag, int.Parse(tempArray[1]), int.Parse(tempArray[2]), int.Parse(tempArray[3])));
                }
            }
            catch (FileNotFoundException e)
            {
                Log.Error("Words Source file not loaded: " + e.Message);
            }

            return hashtags;
        }

        public static void GenerateDirectoryStructure()
        {
            if (!Directory.Exists(Constants.PROGRAM_DATA_FILEPATH))
            {
                Directory.CreateDirectory(Constants.PROGRAM_DATA_FILEPATH);
            }
        }

        public static void WriteStringToFile(string fileName, string data)
        {
            fileName = CheckFileName(fileName);
            string filePath = Constants.PROGRAM_DATA_FILEPATH + @"\" + fileName;
            GenerateDirectoryStructure();
            File.WriteAllText(filePath, data);
        }

        public static string ReadStringFromFile(string fileName)
        {
            fileName = CheckFileName(fileName);
            string filePath = Constants.PROGRAM_DATA_FILEPATH + @"\" + fileName;
            return File.ReadAllText(filePath);
        }

        public static void WriteObjectToFile(string fileName, Object data)
        {
            fileName = CheckFileName(fileName);
            string filePath = Constants.PROGRAM_DATA_FILEPATH + @"\" + fileName;
            GenerateDirectoryStructure();
            File.WriteAllText(filePath, JsonConvert.SerializeObject(data));
            data = null;
        }

        public static T ReadObjectFromFile<T>(string fileName)
        {
            fileName = CheckFileName(fileName);
            string filePath = Constants.PROGRAM_DATA_FILEPATH + @"\" + fileName;
            return JsonConvert.DeserializeObject<T>(File.ReadAllText(filePath));
        }

        public static T ReadModelFromFile<T>(string filename)
        {
            string filePath = Constants.PROGRAM_DATA_FILEPATH + @"\" + filename;
            Serializer.Load(filePath, out T model);

            return model;
        }

        private static string CheckFileName(string input)
        {
            if (input.EndsWith(".txt"))
            {
                return input;
            }
            else
            {
                return input + ".txt";
            }
        }

        public static List<string> GetCommonWords()
        {
            if (commonWords != null)
            {
                return commonWords;
            }

            commonWords = new List<string>();

            try
            {
                commonWords = File.ReadAllLines(commonWordsFilePath).ToList<String>();
            }
            catch (FileNotFoundException e)
            {
                Log.Error("Words Source file not loaded: " + e.Message);
            }

            return commonWords;
        }
    }
}
