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
            List<string> twittersListRight = new List<string>();
            //Right Wing People
            twittersListRight.Add("realDonaldTrump");
            twittersListRight.Add("AnnCoulter");
            twittersListRight.Add("StefanMolyneux");
            twittersListRight.Add("MarkSteynOnline");
            twittersListRight.Add("ezralevant");
            twittersListRight.Add("nntaleb");
            twittersListRight.Add("Lauren_Southern");
            twittersListRight.Add("RealJamesWoods");
            twittersListRight.Add("RandPaul");
            twittersListRight.Add("tedcruz");
            twittersListRight.Add("IngrahamAngle");
            twittersListRight.Add("benshapiro");
            twittersListRight.Add("charliekirk11");
            twittersListRight.Add("PressSec");
            twittersListRight.Add("jihadwatchRS");
            twittersListRight.Add("scrowder");
            twittersListRight.Add("RubinReport");
            twittersListRight.Add("Nigel_Farage");
            twittersListRight.Add("michellemalkin");
            twittersListRight.Add("PrisonPlanet");
            twittersListRight.Add("ScottAdamsSays");
            twittersListRight.Add("andrewklavan");
            twittersListRight.Add("Gavin_McInnes");
            twittersListRight.Add("Cernovich");
            twittersListRight.Add("TuckerCarlson");
            twittersListRight.Add("SheriffClarke");
            twittersListRight.Add("mitchellvii");
            twittersListRight.Add("JohnnoIte");
            twittersListRight.Add("newtgingrich");
            twittersListRight.Add("DineshDSouza");
            twittersListRight.Add("JamesOKeefeIII");
            twittersListRight.Add("DanaPerino");
            twittersListRight.Add("DLoesch");
            twittersListRight.Add("JackPosobiec");
            twittersListRight.Add("BuckSexton");
            twittersListRight.Add("KatiePavlich");
            twittersListRight.Add("marklevinshow");
            twittersListRight.Add("seanhannity");
            twittersListRight.Add("guypbenson");
            twittersListRight.Add("dbongino");
            twittersListRight.Add("AllenWest");
            twittersListRight.Add("greggutfeld");
            twittersListRight.Add("JimDeMint");
            twittersListRight.Add("jasoninthehouse");
            twittersListRight.Add("BrentBozell");
            twittersListRight.Add("KarlRove");
            twittersListRight.Add("larryelder");
            twittersListRight.Add("BillOReilly");
            twittersListRight.Add("nickgillespie");
            twittersListRight.Add("TRobinsonNewEra");

            List<string> twittersListLeft = new List<string>();
            //Left Wing People
            twittersListLeft.Add("donnabrazile");
            twittersListLeft.Add("NancyPelosi");
            twittersListLeft.Add("TheDemocrats");
            twittersListLeft.Add("JoeBiden");
            twittersListLeft.Add("SenatorReid");
            twittersListLeft.Add("HouseDemocrats");
            twittersListLeft.Add("SenateDems");
            twittersListLeft.Add("People4Bernie");
            twittersListLeft.Add("DWStweets");
            twittersListLeft.Add("politics_PR");
            twittersListLeft.Add("docrocktex26");
            twittersListLeft.Add("theclobra");
            twittersListLeft.Add("skookerG");
            twittersListLeft.Add("madeleine");
            twittersListLeft.Add("mcspocky");
            twittersListLeft.Add("kharyp");
            twittersListLeft.Add("npquarterly");
            twittersListLeft.Add("emilyslist");
            twittersListLeft.Add("ArkansasOnline");
            twittersListLeft.Add("co_rapunzel4");
            twittersListLeft.Add("AlanColmes");
            twittersListLeft.Add("Atrios");
            twittersListLeft.Add("cbellatoni");
            twittersListLeft.Add("fahrenthold");
            twittersListLeft.Add("ggreenwald");
            twittersListLeft.Add("MaggieNYT");
            twittersListLeft.Add("MuslimIQ");
            twittersListLeft.Add("Nicopitney");
            twittersListLeft.Add("StephenAtHome");
            twittersListLeft.Add("stevebenen");
            twittersListLeft.Add("TrevorNoah");
            twittersListLeft.Add("wpjenna");

            List<string> twittersListNeutral = new List<string>();
            //Assumed Neutral People
            twittersListNeutral.Add("vidadaugherty22");
            twittersListNeutral.Add("methularanvidu");
            twittersListNeutral.Add("francierooks309");
            twittersListNeutral.Add("laticiaandrie11");
            twittersListNeutral.Add("zoomburg007");
            twittersListNeutral.Add("MDakaou");
            twittersListNeutral.Add("Angelinasevere2");
            twittersListNeutral.Add("JLofitskaja");
            twittersListNeutral.Add("Mansourikhale10");
            twittersListNeutral.Add("Irumva7");
            twittersListNeutral.Add("elanorbaree2659");
            twittersListNeutral.Add("DgKany");
            twittersListNeutral.Add("Trangmon20");
            twittersListNeutral.Add("l4ndshaaaark");
            twittersListNeutral.Add("lucillademares8");
            twittersListNeutral.Add("lupita_plantan");
            twittersListNeutral.Add("hortensiakaras3");
            twittersListNeutral.Add("mugisa_george");
            twittersListNeutral.Add("WomenHealthStyl");
            twittersListNeutral.Add("meikelkrystyna2");
            twittersListNeutral.Add("norikograndt281");
            twittersListNeutral.Add("Emad83464114");
            twittersListNeutral.Add("LazarDaniela56");
            twittersListNeutral.Add("arsenyan13");
            twittersListNeutral.Add("Boubaca05838201");
            twittersListNeutral.Add("harperkristle11");
            twittersListNeutral.Add("sheryldemarko91");
            twittersListNeutral.Add("25Elchapo");
            twittersListNeutral.Add("placido49828895");
            twittersListNeutral.Add("carisaeinfalt11");
            twittersListNeutral.Add("LaPia34798174");
            twittersListNeutral.Add("SxppinArizona");
            twittersListNeutral.Add("dpoedciudni262");
            twittersListNeutral.Add("gouwe_gladman");
            twittersListNeutral.Add("helainecuirot13");
            twittersListNeutral.Add("NasirHu47529869");
            twittersListNeutral.Add("lavernawawi2182");
            twittersListNeutral.Add("AdonisDarko");
            twittersListNeutral.Add("Terps_Dk");
            twittersListNeutral.Add("gwyneth60102350");
            twittersListNeutral.Add("WillMPerkins");
            twittersListNeutral.Add("rexxcolt");
            twittersListNeutral.Add("tuyetjeon2201");
            twittersListNeutral.Add("ReyesBarradas");
            twittersListNeutral.Add("AlexLeu8");
            twittersListNeutral.Add("anemonacomro");
            twittersListNeutral.Add("mohmd044061");
            twittersListNeutral.Add("verdiekelly2380");
            twittersListNeutral.Add("margenehaworth1");
            twittersListNeutral.Add("iPsychicc");


            AuthObj auth = new AuthObj();
            WebHandler webHandler = new WebHandler(auth);
            
            foreach (string a in twittersListRight)
            {
                User user = webHandler.TwitterGetRequest<User>(TwitterRequestBuilder.BuildRequest(RequestType.user, auth, "screen_name=" + a)); //Used for getting the users political value
                if (user != null)
                {
                    List<Tweet> tweetList = TweetRetriever.Instance.GetTweetsFromUser(user, auth);
                    AnalysisResultObj result = TweetAnalyzer.Instance.AnalyzeAndDecorateTweets(tweetList);
                    File.AppendAllText(Constants.PROGRAM_DATA_FILEPATH + @"\RightWingTest.txt", "AlgRes: " + result.GetAlgorithmResult()
                        + "MediaRes: " + result.GetMediaResult()
                        + "KeywordRes: " + result.GetKeywordResult()
                        + "PolPercent: " + result.GetPolPercent()
                        + Environment.NewLine);
                }
                else
                {
                    Console.WriteLine("Could not find: " + a);
                }
            }

            foreach (string a in twittersListLeft)
            {
                User user = webHandler.TwitterGetRequest<User>(TwitterRequestBuilder.BuildRequest(RequestType.user, auth, "screen_name=" + a)); //Used for getting the users political value
                if (user != null)
                {
                    List<Tweet> tweetList = TweetRetriever.Instance.GetTweetsFromUser(user, auth);
                    AnalysisResultObj result = TweetAnalyzer.Instance.AnalyzeAndDecorateTweets(tweetList);
                    File.AppendAllText(Constants.PROGRAM_DATA_FILEPATH + @"\LefttWingTest.txt", "AlgRes: " + result.GetAlgorithmResult()
                        + "MediaRes: " + result.GetMediaResult()
                        + "KeywordRes: " + result.GetKeywordResult()
                        + "PolPercent: " + result.GetPolPercent()
                        + Environment.NewLine);
                }
                else
                {
                    Console.WriteLine("Could not find: " + a);
                }
            }

            foreach (string a in twittersListNeutral)
            {
                User user = webHandler.TwitterGetRequest<User>(TwitterRequestBuilder.BuildRequest(RequestType.user, auth, "screen_name=" + a)); //Used for getting the users political value
                if (user != null)
                {
                    List<Tweet> tweetList = TweetRetriever.Instance.GetTweetsFromUser(user, auth);
                    AnalysisResultObj result = TweetAnalyzer.Instance.AnalyzeAndDecorateTweets(tweetList);
                    File.AppendAllText(Constants.PROGRAM_DATA_FILEPATH + @"\NautralWingTest.txt", "AlgRes: " + result.GetAlgorithmResult()
                        + "MediaRes: " + result.GetMediaResult()
                        + "KeywordRes: " + result.GetKeywordResult()
                        + "PolPercent: " + result.GetPolPercent()
                        + Environment.NewLine);
                }
                else
                {
                    Console.WriteLine("Could not find: " + a);
                }
            }




            /*List<Tweet> tweetList = new List<Tweet>();
            List<Tweet> tweetList2 = new List<Tweet>();

            tweetList = FileHelper.ReadObjectFromFile<List<Tweet>>("\\TestData\\MaggieNYT-Left.json");
            tweetList2 = FileHelper.ReadObjectFromFile<List<Tweet>>("\\TestData\\MaggieNYT-Left.json");

            tweetList.AddRange(tweetList);
            tweetList.AddRange(tweetList);
            tweetList.AddRange(tweetList2);
            tweetList.AddRange(tweetList2);

            Console.WriteLine("Count: " + tweetList.Count);
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            for(int a = 0; a < 500; a++)
            {
                AnalysisResultObj result = TweetAnalyzer.Instance.AnalyzeAndDecorateTweetsThreaded(tweetList);
            }
            stopwatch.Stop();
            Console.WriteLine(stopwatch.ElapsedMilliseconds);
            /*
            AnalysisResultObj result2 = TweetAnalyzer.Instance.AnalyzeAndDecorateTweets(tweetList2);

            Console.WriteLine(tweetList[10].Text);
            Console.WriteLine(result.GetPolPercent() + " " + result.GetAlgorithmResult() + " " + result.GetMediaResult() + " " + result.Count + " " + result.PolCount + " " + result.KeywordBias + " " + result.MediaBias);
            Console.WriteLine(result2.GetPolPercent() + " " + result2.GetAlgorithmResult() + " " + result2.GetMediaResult() + " " + result2.Count + " " + result2.PolCount + " " + result2.KeywordBias + " " + result2.MediaBias);
            */

            Console.WriteLine("?");
            Console.ReadLine();
        }

    }
}
