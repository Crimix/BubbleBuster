using System;

namespace BubbleBuster.Helper
{
    public static class Constants
    {
        public const int REMAINING_OFFSET = 5; //How many there should be left of each request type
        public const string USER_AGENT = "FilterBubble_SW709"; //The name of the APP
        public const int TWEETS_TO_RETRIEVE = 3200; //How many tweets to retrieve per user.
        private static string PROGRAM_DATA = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData); //The path to the files
        public const bool DEBUG = true; //Such that the log file can be for just warnings infos and errors

        //Auth string for DB server on Christoffer's pc
        public const string DB_CREDS = "eyJ0eXAiOiJKV1QiLCJhbGciOiJSUzI1NiIsImp0aSI6IjI4NjFlZDliN2I0N2NmNjVjZjUyMThhMDIyZmNjYjI5YzViNzBjODMwOWU0MTFiMWExMzEyNjk0YzQ3OWRhOWFjMjE3YWQxNWRiOTZlYTA5In0.eyJhdWQiOiIxIiwianRpIjoiMjg2MWVkOWI3YjQ3Y2Y2NWNmNTIxOGEwMjJmY2NiMjljNWI3MGM4MzA5ZTQxMWIxYTEzMTI2OTRjNDc5ZGE5YWMyMTdhZDE1ZGI5NmVhMDkiLCJpYXQiOjE1MTI0NjMwNzcsIm5iZiI6MTUxMjQ2MzA3NywiZXhwIjoxNTQzOTk5MDc2LCJzdWIiOiIiLCJzY29wZXMiOltdfQ.lSZoJKFbhZAucVQKkkAZCjg5en4b27K6kGsI7uQHS7ZcgWQbrLhu0ICiWlAvTaB5eq30ACmu1k3BfdY06DO-qgEeXI9d6fgUVZ-L9g1GWpRHNBM-kIJo1nqcJuCBHyezoQR_9um1NB2FfjjE8naStgOq0xUx6Vv6jJRa7rQJ9Xf65GbhJ5qHT9Hd63yTMk9mBZ9bDA4kW3DRKK82tg_UChdFG5wEC0N3mFtaL-PeiFq8W1zZLU-37NtHbPSIQTH8UMWZd2GxQc7V5nhWsxCO1goocwuSE--NzeSZGcwTmohP8_4fS4vINvFPe8ZH9bQ8UHygCpH6gY-q6n1qQCer-6hAnmuKOOY2JPJV1u7u5BVJhtytQgacvw5hBU_DP7xfSKNncsHp5LgBfLMUYXCeVmfG9UEVvOnabFDenELszY6qGBSKfu6pR7venm85JJUtsp5039yOsuviJzX1ydu5MWC5KbHaGywwEZgbylOfIR3ZUA2k47ZxEAV2cZTDEj1r_7p41lh6-SglGyMc7LFmkPjXsPrerbWmqCscT9oEWYiP3J2RSYfU127iPkizu5lQtT8QqjPErRp3ghfM040ONzfjFVDt0On9oEg2jMPT9CJvrMu41ATT7DeXMs6YhyIJzWbLkcqMxLIc8cFGaqyxAFDHGz_A5xkiAjgfj_ezQns";

        //Auth string for the APP
        public const string APP_API_CREDS = "Bearer AAAAAAAAAAAAAAAAAAAAAPRw2QAAAAAAsXqGsVRPgYFVjSScMX3ZVa9YifA%3DkPvipEcLJj3QooYO7aVke3vZ9ruSJp9CgkTlKKtvlmSsGqLUdG";

        public static string PROGRAM_DATA_FILEPATH { get { return PROGRAM_DATA + @"\BubbleBuster"; } }

        //Constants used for tweet analysis
        public const int HASHTAG_WEIGHT = 1; //Factor. 1 = base. Do not use negative values.
        public const int URL_WEIGHT = 1; //Factor. 1 = base. Do not use negative values.

        //Application keys
        public const string CONSUMER_KEY = "fIbIn9yMl0F7dpWUoihhwAD3N";
        public const string CONSUMER_SECRET = "qZOQqOia1XnGuWbfDKmuxxykTCAzLbmVbdZie40w6AFuuFHy4F";
        public const int TWEET_LIST_AMOUNT = 8; //How many theads should the tweet list be split to 

        //IP to the database server
        public const string DB_SERVER_IP = "http://localhost:8000/api/";

        //Pol% Threshold for not neutral
        public const int POL_PERCENT_THRESHOLD = 1; //Percent
        public const double POL_VALUE_THRESHOLD = 0.5; //Value
    }
}
