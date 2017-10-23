using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BubbleBuster.Helper
{
    public static class FileHelper
    {
        static Dictionary<string, int> newsHyperlinks;
        static Dictionary<string, bool> analysisWords; //Value: false=negativeWord, true=positiveWord
        static string hyperlinkFilePath = Constants.PROGRAM_DATA_FILEPATH + @"\" + "news_hyperlinks";
        static string posWordsFilePath = Constants.PROGRAM_DATA_FILEPATH + @"\" + "positive-words";
        static string negWordsFilePath = Constants.PROGRAM_DATA_FILEPATH + @"\" + "negative-words";

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
        
        public static Dictionary<string, bool> GetAnalysisWords()
        {
            if (analysisWords != null)
            {
                return analysisWords;
            }

            analysisWords = new Dictionary<string, bool>();

            try
            {
                foreach (string word in File.ReadAllLines(posWordsFilePath).Skip(35)) //Skip: Start reading from line 36
                {
                    analysisWords.Add(word, true);
                }

                foreach (string word in File.ReadAllLines(negWordsFilePath).Skip(35))
                {
                    analysisWords.Add(word, false);
                }
            }
            catch (FileNotFoundException e)
            {
                Log.Error("Words Source file not loaded: " + e.Message);
            }

            return analysisWords;
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
    }
}
