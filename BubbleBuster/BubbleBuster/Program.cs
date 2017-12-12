using BubbleBuster.Helper;
using BubbleBuster.Helper.Objects;
using BubbleBuster.Web;
using BubbleBuster.Web.ReturnedObjects;
using BubbleBuster.WordUpdater;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;

namespace BubbleBuster
{
    /// <summary>
    /// This class is just used for testing. Even though this is the Program class with the Main method.
    /// This is because in normal use, this project should be used as a libray and as such does not use the Program class
    /// </summary>
    public class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            List<Tweet> tweetList = new List<Tweet>();
            List<Tweet> tweetList2 = new List<Tweet>();

            tweetList = FileHelper.ReadObjectFromFile<List<Tweet>>("\\TestData\\MaggieNYT-Left.json");
            tweetList2 = FileHelper.ReadObjectFromFile<List<Tweet>>("\\TestData\\MaggieNYT-Left.json");


            AnalysisResultObj result = TweetAnalyzer.Instance.AnalyzeAndDecorateTweetsThreaded(tweetList);
            AnalysisResultObj result2 = TweetAnalyzer.Instance.AnalyzeAndDecorateTweets(tweetList2);

            Console.WriteLine(tweetList[10].Text);
            Console.WriteLine(result.GetPolPercent() + " " + result.GetAlgorithmResult() + " " + result.GetMediaResult() + " " + result.Count + " " + result.PolCount + " " + result.KeywordBias + " " + result.MediaBias);
            Console.WriteLine(result2.GetPolPercent() + " " + result2.GetAlgorithmResult() + " " + result2.GetMediaResult() + " " + result2.Count + " " + result2.PolCount + " " + result2.KeywordBias + " " + result2.MediaBias);


            Console.WriteLine("?");
            Console.ReadLine();
        }

    }
}
