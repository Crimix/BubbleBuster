using System;
using System.IO;
using System.Collections.Generic;
using BubbleBuster.Web.ReturnedObjects;
using TextProcesserLib;
using Accord.MachineLearning;
using Accord.Statistics.Distributions.Fitting;
using Accord.MachineLearning.Bayes;
using Accord.Statistics.Distributions.Univariate;


namespace BubbleBuster.Helper
{
    public class Classifier
    {
        private BagOfWords bagOfWords;
        private TextProcessor tp;

        public Classifier()
        {
            tp = new TextProcessor();
        }

        /// <summary>
        /// Loads a NB model from a file
        /// Formats the tweets and decides on their bias.
        /// </summary>
        /// <param name="tweets"> The list of tweets</param>
        /// <returns> The bias </returns>
        public double RunNaiveBayes(List<Tweet> tweets)
        {
            var model = FileHelper.GetModel();

            double[][] inputs = FormatTweets(tweets);
            
            //Predicts each tweets class
            int[] answers = model.Decide(inputs);

            List<int> result = new List<int>(){ 0, 0, 0};
            foreach (var item in answers)
            {
                result[item] += 1;
            }

            return CalcBias(result);
        }

        /// <summary>
        /// Formats a list of Tweets to Bag-of-Words format
        /// </summary>
        /// <param name="tweets"> A list of tweets </param>
        /// <returns>  Formatted Tweets in Bag of Words format </returns>
        double[][] FormatTweets (List<Tweet> tweets)
        {
            List<string> _tweets = new List<string>();
            foreach (Tweet item in tweets)
            {
                _tweets.Add(item.Text);
            }
            bagOfWords = FileHelper.GetBagOfWords();

            //Whitespace tokenizer
            //string[][] tokens = tweets.ToArray().Tokenize();

            //Custom Tokenizer
            string[][] tokens = tp.Tokenizer(_tweets);
            
            double[][] input = bagOfWords.Transform(tokens);

            return input;
        }

        /// <summary>
        /// Calculates the Bias according to an arbitrary method
        /// </summary>
        /// <param name="results"> A list of predictions </param>
        /// <returns> Personal bias </returns>
        double CalcBias(List<int> results)
        {
            double left = results[0] - (results[1] / 2);
            if (left < 0)
            {
                left = 0;
            }

            double right = results[2] - (results[1] / 2);
            if (right < 0)
            {
                right = 0;
            }

            double bias = left - right;

            return bias;
        }

        public void TrainNaiveBayes(string inputFile, string outputFile)
        {
            double[][] inputs;
            int[] outputs;

            bagOfWords = new BagOfWords()
            {
                MaximumOccurance = 1
            };

            inputs = ReadInput(inputFile);
            outputs = ReadOutput(outputFile);

            var teacher = new NaiveBayesLearning<NormalDistribution>();

            teacher.Options.InnerOption = new NormalOptions
            {
                Regularization = 1e-6 // to avoid zero variances
            };
            
            var nb = teacher.Learn(inputs, outputs);

            FileHelper.WriteModelToFile("Bayes90.accord", nb);
        }

        /// <summary> 
        /// Saves the formatted tokens for future use.
        /// Formats a list of strings using a Bag of Words.
        /// </summary>
        /// <param name="path">Path to the folder</param>
        /// <param name="inputDoc">Name of training tweets document</param>
        /// <returns> Formatted tweets to Bag of Words format </returns>
        double[][] ReadInput(string inputDoc)
        {
            double[][] input;

            using (StreamReader r = new StreamReader(Constants.PROGRAM_DATA_FILEPATH + @"\"  +inputDoc))
            {
                List<string> tweets = new List<string>();

                while (!r.EndOfStream)
                {
                    tweets.Add(r.ReadLine());
                }
                
                //Use custom tokenizer
                string[][] tokens = tp.Tokenizer(tweets);

                FileHelper.WriteObjectToFile("BagOfWords90.txt", tokens);

                bagOfWords.Learn(tokens);

                input = bagOfWords.Transform(tokens);

                r.DiscardBufferedData();
                r.Close();
            };

            return input;
        }

        /// <summary>
        /// Reads a training document of labels
        /// </summary>
        /// <param name="path">Path to the Folder</param>
        /// <param name="outputDoc">Name of training labels document</param>
        /// <returns>A list of integers from 0-2 </returns>
        int[] ReadOutput(string outputDoc)
        {
            List<int> result = new List<int>();
            using (StreamReader r2 = new StreamReader(Constants.PROGRAM_DATA_FILEPATH + @"\" + outputDoc))
            {
                //Transforms from 5 labels to 3
                while (!r2.EndOfStream)
                {
                    string line = r2.ReadLine();
                    int i = int.Parse(line);

                    if (i == 0 || i == 1)
                    {
                        result.Add(0);
                    }
                    else if (i == 3 || i == 4)
                    {
                        result.Add(2);
                    }
                    else
                    {
                        result.Add(1);
                    }
                }

                r2.DiscardBufferedData();
                r2.Close();
            };

            return result.ToArray();
        }
    }
}

