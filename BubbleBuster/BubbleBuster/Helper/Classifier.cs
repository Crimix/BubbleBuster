using Accord.IO;
using Accord.MachineLearning.Bayes;
using Accord.Statistics.Distributions.Univariate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BubbleBuster.Web.ReturnedObjects;
using Accord.MachineLearning;
using TextProcesserLib;

namespace BubbleBuster.Helper
{
    class Classifier
    {
        BagOfWords bagOfWords;
        TextProcessor tp;

        public Classifier()
        {
            tp = new TextProcessor();
        }

        public double RunNaiveBayes(List<Tweet> tweets)
        {
            var model = FileHelper.ReadModelFromFile<NaiveBayes<NormalDistribution>>("NaiveBayes90.accord");

            double[][] inputs = FormatTweets(tweets);
            
            int[] answers = model.Decide(inputs);
            List<int> result = new List<int>(){ 0, 0, 0};

            foreach (var item in answers)
            {
                result[item] += 1;
            }

            return CalcBias(result);
        }

        public double[][] FormatTweets (List<Tweet> tweets)
        {
            List<string> _tweets = new List<string>();
            foreach (Tweet item in tweets)
            {
                _tweets.Add(item.Text);
            }

            BagOfWords bagOfWords = new BagOfWords()
            {
                MaximumOccurance = 1
            };

            string[][] trainingTokens = FileHelper.ReadObjectFromFile<string[][]>(@"BagOfWords90.txt");

            bagOfWords.Learn(trainingTokens);

            //string[][] tokens = tweets.ToArray().Tokenize();
            string[][] tokens = tp.Tokenizer(_tweets);
            
            double[][] input = bagOfWords.Transform(tokens);

            return input;
        }

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
    }
}

