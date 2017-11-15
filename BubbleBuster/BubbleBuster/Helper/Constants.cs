using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BubbleBuster.Helper
{
    public static class Constants
    {
        public const int REMAINING_OFFSET = 5;
        public const string USER_AGENT = "FilterBubble_SW709";
        public const int TWEETS_TO_RETRIEVE = 3200;
        private static string PROGRAM_DATA = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);

        public static string PROGRAM_DATA_FILEPATH { get { return PROGRAM_DATA + @"\\BubbleBuster"; } }

        //Constants used for tweet analysis
        public const int HASHTAG_WEIGHT = 1; //Factor. 1 = base. Do not use negative values.
        public const int URL_WEIGHT = 1; //Factor. 1 = base. Do not use negative values.
    }
}
