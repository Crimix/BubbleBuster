using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
/// <summary>
/// This object is used to represent words which are identified as not being among the 1000(0) most common english words.
/// This object has a number of counters, which are incremented whenever the word is found in a tweet.
/// There are individual counters for either right wing, left wing, and neutral affiliations of the user making the tweet.
/// </summary>
namespace BubbleBuster.WordUpdater
{
    public class UncommonWordObj
    {
        public UncommonWordObj(string wordString)
        {
            Word = wordString;
            LeftPosCount = 0;
            LeftNegCount = 0;
            LeftNeuCount = 0;
            RightPosCount = 0;
            RightNegCount = 0;
            RightNeuCount = 0;
            CenterPosCount = 0;
            CenterNegCount = 0;
            CenterNeuCount = 0;
        }

        public string Word { get; set; } = "";
        public int LeftPosCount { get; set; }
        public int RightPosCount { get; set; }
        public int CenterPosCount { get; set; }
        public int LeftNegCount { get; set; }
        public int RightNegCount { get; set; }
        public int CenterNegCount { get; set; }
        public int LeftNeuCount { get; set; }
        public int RightNeuCount { get; set; }
        public int CenterNeuCount { get; set; }

        public int Count { get { return LeftNegCount + LeftNeuCount + LeftPosCount + CenterNegCount + CenterNeuCount 
                    + CenterPosCount + RightNegCount + RightNeuCount + RightPosCount; } }

    }
}
