namespace BubbleBuster.Helper.Objects
{
    public class AnalysisResultObj
    {
        //General Info
        public double Count { get; set; } = 0;
        public double PolCount { get; set; } = 0;
        public int PositiveTweetsCount { get; set; } = 0;
        public int NegativeTweetsCount { get; set; } = 0;

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
                return (PositiveSentiment - NegativeSentiment) / Count;
            else
                return 0;
        }

        /// <summary>
        /// Get the algorithm result value
        /// </summary>
        /// <returns>A double</returns>
        public double GetAlgorithmResult()
        {
            if (GetPolPercent() < Constants.POL_PERCENT_THRESHOLD)
                return 0;
            else if (Count == 0)
                return 0;
            /*else if (((KeywordBias + MediaBias) / PolCount) < Constants.POL_VALUE_THRESHOLD && ((KeywordBias + MediaBias) / PolCount) > (-1 * Constants.POL_VALUE_THRESHOLD))
                return 0;*/
            else
            {
                if ((KeywordBias + MediaBias) / PolCount < -10)
                    return -10;
                else if ((KeywordBias + MediaBias) / PolCount > 10)
                    return 10;
                else
                    return (KeywordBias + MediaBias) / PolCount;
            }
        }

        public double GetUnprocessedAlgorithmResult()
        {
            if (PolCount > 0)
                return (KeywordBias + MediaBias) / PolCount;
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
                return MediaBias / PolCount;
            else
                return 0;
        }

        /// <summary>
        /// Get the media result value
        /// </summary>
        /// <returns>A double</returns>
        public double GetKeywordResult()
        {
            if (Count != 0)
                return KeywordBias / PolCount;
            else
                return 0;
        }

        public double GetPolPercent()
        {
            if (Count != 0)
                return (PolCount / Count) * 100;
            else
                return 0;
        }

    }
}
