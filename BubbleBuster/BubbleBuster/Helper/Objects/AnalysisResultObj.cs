using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        //Machine Learning
        public double MIResult { get; set; } = 0;

        //Methods
        public double GetSentiment
        {
            get
            {
                if (Count != 0)
                    return (PositiveSentiment + NegativeSentiment) / Count;
                else
                    return 0;
            }
        }

        public double GetAlgorithmResult
        {
            get
            {
                if (Count != 0)
                    return (KeywordBias + MediaBias) / Count;
                else
                    return 0;
            }
        }

        public double GetMIResult
        {
            get
            {
                if (Count != 0)
                    return MIResult / Count;
                else
                    return 0;
            }
        }
    }
}
