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
    public class Classifier
    {
        private BagOfWords bagOfWords;
        private TextProcessor tp;

        /// <summary>
        /// Initialize a TextProcessor
        /// </summary>
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
        /// Formats a list of Tweets to the correct Accord.Net format
        /// </summary>
        /// <param name="tweets"> A list of tweets </param>
        /// <returns>  Formatted Tweets </returns>
        public double[][] FormatTweets (List<Tweet> tweets)
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
    }
}

