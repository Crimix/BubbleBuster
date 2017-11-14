using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BubbleBuster.Helper.HelperObjects;

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


        public static void WriteStringToFile(string folderName, string fileName, string data)
        {
            fileName = CheckFileName(fileName);
            string folderPath = Path.GetTempPath() + folderName;
            string filePath = Path.GetTempPath() + folderName + @"\" + fileName;
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
            File.WriteAllText(filePath, data);
        }

        public static string ReadStringFromFile(string folderName, string fileName)
        {
            fileName = CheckFileName(fileName);
            string folderPath = Path.GetTempPath() + folderName;
            string filePath = Path.GetTempPath() + folderName + @"\" + fileName;
            return File.ReadAllText(filePath);
        }

        public static void WriteObjectToFile(string folderName, string fileName, Object data)
        {
            fileName = CheckFileName(fileName);
            string folderPath = Path.GetTempPath() + folderName;
            string filePath = Path.GetTempPath() + folderName + @"\" + fileName;
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
            File.WriteAllText(filePath, JsonConvert.SerializeObject(data));
            data = null;
        }

        public static T ReadObjectFromFile<T>(string folderName, string fileName)
        {
            fileName = CheckFileName(fileName);
            string folderPath = Path.GetTempPath() + folderName;
            string filePath = Path.GetTempPath() + folderName + @"\" + fileName;
            return JsonConvert.DeserializeObject<T>(File.ReadAllText(filePath));
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
