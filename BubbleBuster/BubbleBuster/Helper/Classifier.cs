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

        public void RunNaiveBayes(List<Tweet> tweets)
        {
            Serializer.Load(@"my_NB.accord", out NaiveBayes<NormalDistribution> nb);

            double[][] inputs = FormatTweets(tweets);

            int[] answers = nb.Decide(inputs);
        }

        public double[][] FormatTweets (List<Tweet> tweets)
        {
            List<string> _tweets = new List<string>();
            foreach (Tweet item in tweets)
            {
                _tweets.Add(item.Text);
            }
            
            //Load a trained Bag of Words
            //bagOfWords = ...

            //string[][] tokens = tweets.ToArray().Tokenize();
            string[][] tokens = tp.Tokenizer(_tweets);
            
            double[][] input = bagOfWords.Transform(tokens);

            return input;
        }
    }
}

