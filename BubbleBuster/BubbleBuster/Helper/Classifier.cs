using Accord.MachineLearning;
using Accord.MachineLearning.Bayes;
using Accord.Statistics.Distributions.Fitting;
using Accord.Statistics.Distributions.Univariate;
using BubbleBuster.Web.ReturnedObjects;
using System.Collections.Generic;
using System.IO;
using TextProcesserLib;
using System;

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
            if (tweets.Count == 0)
                return 0;

            var model = FileHelper.GetModel();
            bagOfWords = FileHelper.GetBagOfWords();

            double[][] inputs = FormatTweets(tweets);

            //Predicts each tweets class
            int[] answers = model.Decide(inputs);

            List<int> result = new List<int>() { 0, 0, 0 };
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
        double[][] FormatTweets(List<Tweet> tweets)
        {
            List<string> _tweets = new List<string>();
            foreach (Tweet item in tweets)
            {
                _tweets.Add(item.Text);
            }

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
            double left = results[0];
            double neutral = results[1];
            double right = results[2];

            double bias = (right - left) / (left + neutral + right) * 10;

            return bias;
        }

        /// <summary>
        /// Trains a Naive Bayes classifier
        /// </summary>
        /// <param name="inputFile">File containing tweets</param>
        /// <param name="outputFile">File containing labels</param>
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

            FileHelper.WriteModelToFile("NaiveBayes90.accord", nb);
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

            using (StreamReader r = new StreamReader(Constants.PROGRAM_DATA_FILEPATH + @"\" + inputDoc))
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

#region AccuracyTest
        /// <summary>
        /// Used for testing purposes
        /// Reads a specific portion of a file
        /// </summary>
        /// <param name="inputDoc">tweet file</param>
        /// <param name="includeStart">Inclusion start</param>
        /// <param name="includeEnd">Inclusion end</param>
        /// <returns></returns>
        string[][] ReadInputIn(string inputDoc, int includeStart, int includeEnd)
        {
            string[][] tokens;

            using (StreamReader r = new StreamReader(Constants.PROGRAM_DATA_FILEPATH + @"\" + inputDoc))
            {
                List<string> tweets = new List<string>();
                int i = 0;
                while (!r.EndOfStream)
                {
                    if (i >= includeStart && i < includeEnd)
                    {
                        tweets.Add(r.ReadLine());
                    }
                    else r.ReadLine();
                    i++;
                }

                //Use custom tokenizer
                tokens = tp.Tokenizer(tweets);

                r.DiscardBufferedData();
                r.Close();
            };

            return tokens;
        }

        /// <summary>
        /// Used for testing purposes
        /// Reads everything excluding boundaries
        /// </summary>
        /// <param name="inputDoc">tweet file</param>
        /// <param name="excludeStart">Exclusion start</param>
        /// <param name="excludeEnd">Exclusion end</param>
        /// <returns></returns>
        string[][] ReadInputEx(string inputDoc, int excludeStart, int excludeEnd)
        {
            string[][] tokens;

            using (StreamReader r = new StreamReader(Constants.PROGRAM_DATA_FILEPATH + @"\" + inputDoc))
            {
                List<string> tweets = new List<string>();
                int i = 0;
                while (!r.EndOfStream)
                {
                    if (i < excludeStart || i > excludeEnd)
                    {
                        tweets.Add(r.ReadLine());
                    }
                    else r.ReadLine();
                    i++;
                }

                //Use custom tokenizer
                tokens = tp.Tokenizer(tweets);

                r.DiscardBufferedData();
                r.Close();
            };

            return tokens;
        }

        /// <summary>
        /// Used for testing purposes
        /// Reads everything excluding boundaries
        /// </summary>
        /// <param name="outputDoc">File containing labels</param>
        /// <param name="excludeStart">Exclusion start</param>
        /// <param name="excludeEnd">Exclusion end</param>
        /// <returns></returns>
        int[] ReadOutputEx(string outputDoc, int excludeStart, int excludeEnd)
        {
            List<int> result = new List<int>();
            using (StreamReader r2 = new StreamReader(Constants.PROGRAM_DATA_FILEPATH + @"\" + outputDoc))
            {
                int n = 0;
                //Transforms from 5 labels to 3
                while (!r2.EndOfStream)
                {
                    if (n < excludeStart || n > excludeEnd)
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
                    else r2.ReadLine();
                    n++;
                }

                r2.DiscardBufferedData();
                r2.Close();
            };

            return result.ToArray();
        }
        
        /// <summary>
        /// Used for testing purposes
        /// Reads a specific portion within boundaries
        /// </summary>
        /// <param name="outputDoc">label file</param>
        /// <param name="includeStart"> inclusion start</param>
        /// <param name="includeEnd">inclusion end</param>
        /// <returns></returns>
        int[] ReadOutputIn(string outputDoc, int includeStart, int includeEnd)
        {
            List<int> result = new List<int>();
            using (StreamReader r2 = new StreamReader(Constants.PROGRAM_DATA_FILEPATH + @"\" + outputDoc))
            {
                int n = 0;
                //Transforms from 5 labels to 3
                while (!r2.EndOfStream)
                {
                    if (n >= includeStart && n < includeEnd)
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
                    else r2.ReadLine();
                    n++;
                }

                r2.DiscardBufferedData();
                r2.Close();
            };

            return result.ToArray();
        }


        /// <summary>
        /// Initiates the required components and runs 4 accuracy tests
        /// Uses 90 tweets for training
        /// uses 30 tweets for testing
        /// </summary>
        /// <param name="inputFile">Tweets</param>
        /// <param name="outputFile">Labels</param>
        public void TestNaiveBayes(string inputFile, string outputFile)
        {
            //Create training features
            //4 sets of 90
            string[][] tokens1 = ReadInputEx(inputFile, 0, 29);
            string[][] tokens2 = ReadInputEx(inputFile, 30, 59);
            string[][] tokens3 = ReadInputEx(inputFile, 60, 89);
            string[][] tokens4 = ReadInputEx(inputFile, 90, 119);

            //Read training output
            int[] outputs1 = ReadOutputEx(outputFile, 0, 29);
            int[] outputs2 = ReadOutputEx(outputFile, 30, 59);
            int[] outputs3 = ReadOutputEx(outputFile, 60, 89);
            int[] outputs4 = ReadOutputEx(outputFile, 90, 119);
            
            //Create BOW for each training set
            BagOfWords bow1 = new BagOfWords()
            {
                MaximumOccurance = 1
            };
            bow1.Learn(tokens1);

            BagOfWords bow2 = new BagOfWords()
            {
                MaximumOccurance = 1
            };
            bow2.Learn(tokens2);

            BagOfWords bow3 = new BagOfWords()
            {
                MaximumOccurance = 1
            };
            bow3.Learn(tokens3);

            BagOfWords bow4 = new BagOfWords()
            {
                MaximumOccurance = 1
            };
            bow4.Learn(tokens4);

            //Transform to feature vector
            double[][] inputs1 = bow1.Transform(tokens1);
            double[][] inputs2 = bow2.Transform(tokens2);
            double[][] inputs3 = bow3.Transform(tokens3);
            double[][] inputs4 = bow4.Transform(tokens4);
            
            //Create teachers
            var teacher1 = new NaiveBayesLearning<NormalDistribution>();
            teacher1.Options.InnerOption = new NormalOptions
            {
                Regularization = 1e-6 // to avoid zero variances
            };
            var teacher2 = new NaiveBayesLearning<NormalDistribution>();
            teacher2.Options.InnerOption = new NormalOptions
            {
                Regularization = 1e-6 // to avoid zero variances
            };
            var teacher3 = new NaiveBayesLearning<NormalDistribution>();
            teacher3.Options.InnerOption = new NormalOptions
            {
                Regularization = 1e-6 // to avoid zero variances
            };
            var teacher4 = new NaiveBayesLearning<NormalDistribution>();
            teacher4.Options.InnerOption = new NormalOptions
            {
                Regularization = 1e-6 // to avoid zero variances
            };

            //Create the Naive Bayes
            var nb1 = teacher1.Learn(inputs1, outputs1);
            var nb2 = teacher2.Learn(inputs2, outputs2);
            var nb3 = teacher3.Learn(inputs3, outputs3);
            var nb4 = teacher4.Learn(inputs4, outputs4);

            //Create the training sets
            //the remaining 30
            double[][] testInputs1 = bow1.Transform(ReadInputIn(inputFile, 0, 30));
            double[][] testInputs2 = bow2.Transform(ReadInputIn(inputFile, 30, 60));
            double[][] testInputs3 = bow3.Transform(ReadInputIn(inputFile, 60, 90));
            double[][] testInputs4 = bow4.Transform(ReadInputIn(inputFile, 90, 120));
            int[] testOutputs1 = ReadOutputIn(outputFile, 0, 30);
            int[] testOutputs2 = ReadOutputIn(outputFile, 30, 60);
            int[] testOutputs3 = ReadOutputIn(outputFile, 60, 90);
            int[] testOutputs4 = ReadOutputIn(outputFile, 90, 120);

            //predict answers
            int[] answers1 = nb1.Decide(testInputs1);
            int[] answers2 = nb2.Decide(testInputs2);
            int[] answers3 = nb3.Decide(testInputs3);
            int[] answers4 = nb4.Decide(testInputs4);

            int correct1 = 0;
            int correct2 = 0;
            int correct3 = 0;
            int correct4 = 0;

            for (int i = 0; i < testOutputs1.Length; i++)
            {
                if (answers1[i] == testOutputs1[i])
                {
                    correct1++;
                }
                if (answers2[i] == testOutputs2[i])
                {
                    correct2++;
                }
                if (answers3[i] == testOutputs3[i])
                {
                    correct3++;
                }
                if (answers4[i] == testOutputs4[i])
                {
                    correct4++;
                }
            }

            double accuracy1 = ((double)correct1 / 30);
            double accuracy2 = ((double)correct2 / 30);
            double accuracy3 = ((double)correct3 / 30);
            double accuracy4 = ((double)correct4 / 30);
            double averageAccuracy = (((double)(correct1 + correct2 + correct3 + correct4)) / 120);

            Console.WriteLine(accuracy1 + " " + accuracy2 + " " + accuracy3 + " " + accuracy4);
            Console.WriteLine(averageAccuracy);

        }
#endregion
    }
}

