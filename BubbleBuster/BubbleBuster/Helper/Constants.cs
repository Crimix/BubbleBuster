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
        public const int TWEETS_TO_RETRIEVE = 200;
        private static string PROGRAM_DATA = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);

        public static string PROGRAM_DATA_FILEPATH { get { return PROGRAM_DATA + @"\" + AppDomain.CurrentDomain.FriendlyName.Replace(".exe",""); } }
    }
}
