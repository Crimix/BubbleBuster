using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BubbleBuster.Helper.Objects;
using Accord.IO;
using Accord.MachineLearning;
using Accord.Statistics.Distributions.Fitting;
using Accord.MachineLearning.Bayes;
using Accord.Statistics.Distributions.Univariate;
using Accord.MachineLearning;


namespace BubbleBuster.Helper
{
    //This class is static because it is only a helper class.
    public static class FileHelper
    {
        //Such that we do only need to read the file once
        private static Dictionary<string, int> newsHyperlinks;
        private static Dictionary<string, KeywordObj> keywords;
        private static Dictionary<string, int> analysisWords; //Value: -1=negativeWord, 1=positiveWord
        private static List<string> commonWords;
        private static BagOfWords bagOfWords;
        private static NaiveBayes<NormalDistribution> model;

        //path variables
        private static string hyperlinkFilePath = Constants.PROGRAM_DATA_FILEPATH + @"\" + "news_hyperlinks";
        private static string commonWordsFilePath = Constants.PROGRAM_DATA_FILEPATH + @"\" + "commonWords";
        private static string posWordsFilePath = Constants.PROGRAM_DATA_FILEPATH + @"\" + "positive-words";
        private static string negWordsFilePath = Constants.PROGRAM_DATA_FILEPATH + @"\" + "negative-words";
        private static string keywordsFilePath = Constants.PROGRAM_DATA_FILEPATH + @"\" + "keywords";

        /// <summary>
        /// Gets the dictionary of hyperlinks and bias value
        /// </summary>
        /// <returns>A dictionary of hyperlinks and bias value</returns>
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

        /// <summary>
        /// Gets the dictionary of analysis words and if the word is positive or negative
        /// /// </summary>
        /// <returns>A dictionary of analysis words</returns>
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

        /// <summary>
        /// Gets the dictionary of keywords
        /// /// </summary>
        /// <returns>A dictionary of keywords</returns>
        public static Dictionary<string, KeywordObj> GetKeywords()
        {
            if (keywords != null)
            {
                return keywords;
            }

            keywords = new Dictionary<string, KeywordObj>();

            try
            {
                List<String> temp = File.ReadAllLines(keywordsFilePath).Skip(7).ToList<String>();

                foreach (string keyword in temp)
                {
                    string[] tempArray = keyword.Split(';'); 
                    keywords.Add(tempArray[0], new KeywordObj(keyword, int.Parse(tempArray[1]), int.Parse(tempArray[2]), int.Parse(tempArray[3])));
                    keywords.Add('#' + tempArray[0], new KeywordObj('#' + keyword, int.Parse(tempArray[1]), int.Parse(tempArray[2]), int.Parse(tempArray[3])));
                }
            }
            catch (FileNotFoundException e)
            {
                Log.Error("Words Source file not loaded: " + e.Message);
            }

            return keywords;
        }

        public static BagOfWords GetBagOfWords()
        {
            if(bagOfWords == null)
            {
                bagOfWords = new BagOfWords()
                {
                    MaximumOccurance = 1
                };
                bagOfWords.Learn(ReadObjectFromFile<string[][]>(@"BagOfWords90.txt"));
            }
            return bagOfWords;
        }

        public static NaiveBayes<NormalDistribution> GetModel()
        {
            if(model == null)
            {
                model = ReadModelFromFile<NaiveBayes<NormalDistribution>>("NaiveBayes90.accord");
            }
            return model;
        }


        /// <summary>
        /// Generates the directory structure
        /// </summary>
        public static void GenerateDirectoryStructure()
        {
            if (!Directory.Exists(Constants.PROGRAM_DATA_FILEPATH))
            {
                Directory.CreateDirectory(Constants.PROGRAM_DATA_FILEPATH);
            }
        }

        /// <summary>
        /// Writes the string to the file
        /// </summary>
        /// <param name="fileName">The name of the file</param>
        /// <param name="data">The data string</param>
        public static void WriteStringToFile(string fileName, string data)
        {
            fileName = CheckFileName(fileName);
            string filePath = Constants.PROGRAM_DATA_FILEPATH + @"\" + fileName;
            GenerateDirectoryStructure();
            File.WriteAllText(filePath, data);
        }

        /// <summary>
        /// Reads a file and returns the content as a string
        /// </summary>
        /// <param name="fileName">The name of the file</param>
        /// <returns>A string</returns>
        public static string ReadStringFromFile(string fileName)
        {
            fileName = CheckFileName(fileName);
            string filePath = Constants.PROGRAM_DATA_FILEPATH + @"\" + fileName;
            return File.ReadAllText(filePath);
        }

        /// <summary>
        /// Writes an object to the file
        /// </summary>
        /// <param name="fileName">The name of the file</param>
        /// <param name="data">The data object</param>
        public static void WriteObjectToFile(string fileName, Object data)
        {
            fileName = CheckFileName(fileName);
            string filePath = Constants.PROGRAM_DATA_FILEPATH + @"\" + fileName;
            GenerateDirectoryStructure();
            File.WriteAllText(filePath, JsonConvert.SerializeObject(data));
            data = null;
        }
        
        /// <summary>
        /// Writes an Accord Object to file
        /// </summary>
        /// <param name="fileName"> The name of the file</param>
        /// <param name="classifier">The data object</param>
        public static void WriteModelToFile(string fileName, NaiveBayes<NormalDistribution> model)
        {
            Console.WriteLine("Entered WriteModelToFile");
            string filePath = Constants.PROGRAM_DATA_FILEPATH + @"\" + fileName;
            Console.WriteLine("Models: " + filePath);
            GenerateDirectoryStructure();
            Serializer.Save(model, fileName);
        }

        /// <summary>
        /// Reads a file and tries to deserialize it from Json to the type T
        /// </summary>
        /// <typeparam name="T">The type T</typeparam>
        /// <param name="fileName">The name of the file</param>
        /// <returns>An object of type T</returns>
        public static T ReadObjectFromFile<T>(string fileName)
        {
            T obj = default(T);
            fileName = CheckFileName(fileName);
            string filePath = Constants.PROGRAM_DATA_FILEPATH + @"\" + fileName;
            try
            {
                obj = JsonConvert.DeserializeObject<T>(File.ReadAllText(filePath));
            }
            catch (Exception e)
            {
                Log.Error("Could not deserialize the file" + e.Message);
            }
            
            return obj;
        }

        /// <summary>
        /// Reads a model from file
        /// </summary>
        /// <typeparam name="T">The type T</typeparam>
        /// <param name="filename">The name of the file</param>
        /// <returns>An object of type T</returns>
        public static T ReadModelFromFile<T>(string filename)
        {
            string filePath = Constants.PROGRAM_DATA_FILEPATH + @"\" + filename;
            Serializer.Load(filePath, out T model);

            return model;
        }

        /// <summary>
        /// Checks the name of the input file, if it does not contain a file extenstion, adds .txt to the end of it
        /// and returns the strng
        /// </summary>
        /// <param name="input">The input name of the file</param>
        /// <returns>A string</returns>
        private static string CheckFileName(string input)
        {
            if (!input.Contains('.'))
            {
                return input + ".txt";
            }
            else
            {
                return input;
            }
        }

        /// <summary>
        /// Returns a list of common words
        /// </summary>
        /// <returns>A list of common words</returns>
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
