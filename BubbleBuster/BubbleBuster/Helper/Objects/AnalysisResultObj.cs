namespace BubbleBuster.Helper.Objects
{
    public class AnalysisResultObj
    {
        //General Info
        public double Count { get; set; } = 0;

        //Analysis Algorithm
        public double KeywordBias { get; set; } = 0;
        public double MediaBias { get; set; } = 0;
        public double PositiveSentiment { get; set; } = 0;
        public double NegativeSentiment { get; set; } = 0;

        //Machine Learning, does not need to be divide by the count of tweets. This action has been performed by the classifier.
        public double MIResult { get; set; } = 0;


        /// <summary>
        /// Get the sentiment value
        /// </summary>
        /// <returns>A double</returns>
        public double GetSentiment()
        {
            if (Count != 0)
                return (PositiveSentiment + NegativeSentiment) / Count;
            else
                return 0;
        }

        /// <summary>
        /// Get the algorithm result value
        /// </summary>
        /// <returns>A double</returns>
        public double GetAlgorithmResult()
        {
            if (Count != 0)
                return (KeywordBias + MediaBias) / Count;
            else
                return 0;
        }

        /// <summary>
        /// Get the media result value
        /// </summary>
        /// <returns>A double</returns>
        public double GetMediaResult()
        {
            if (Count != 0)
                return MediaBias / Count;
            else
                return 0;
        }

    }
}
