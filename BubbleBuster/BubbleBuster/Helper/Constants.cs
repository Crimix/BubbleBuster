using System;

namespace BubbleBuster.Helper
{
    public static class Constants
    {
        public const int REMAINING_OFFSET = 5; //How many there should be left of each request type
        public const string USER_AGENT = "FilterBubble_SW709"; //The name of the APP
        public const int TWEETS_TO_RETRIEVE = 1000; //How many tweets to retrieve per user.
        private static string PROGRAM_DATA = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData); //The path to the files
        public const bool DEBUG = true; //Such that the log file can be for just warnings infos and errors
        public const int QUEUE_SERVER_TASK_LIMIT = 5; //Task limit for the queue server

        //Auth string for DB server on Christoffer's pc
        public const string DB_CREDS = "eyJ0eXAiOiJKV1QiLCJhbGciOiJSUzI1NiIsImp0aSI6ImZjODE1OTgzM2EzYTczMjhhMDkyZmQzZmM3MTdlNWQzZWY2ZDE1OTc5ODJmNGVmNmJkNmY1NWQ0YjEyZjdlY2YyMGNkMWY5ODQxYzhiYzcyIn0.eyJhdWQiOiIxIiwianRpIjoiZmM4MTU5ODMzYTNhNzMyOGEwOTJmZDNmYzcxN2U1ZDNlZjZkMTU5Nzk4MmY0ZWY2YmQ2ZjU1ZDRiMTJmN2VjZjIwY2QxZjk4NDFjOGJjNzIiLCJpYXQiOjE1MTUxNTQxMzksIm5iZiI6MTUxNTE1NDEzOSwiZXhwIjoxNTQ2NjkwMTM5LCJzdWIiOiIiLCJzY29wZXMiOltdfQ.JSEfLSDDcm0d1emCiI7pHqnUCwJ_D_1f8bm8nR4M-HwS5eif2jo7SI8Tmirz6makpE9AZ4b5JG8FiPZEiOaT3_5GJoENEsa1s7rh0K86EsGtIKOvbxZipd3oLBa4uzfRY_o-zWDJypKLv0q1OdUdcM2LKSaVLGvEyWahSn8gq-BYfeXW0htytr_FpqHN_ttoDNq_Fwf-0aeuIACoqqo3MbOztK7XH7YtuTO1_i_IO9WA6Kp3meuwxXLKOtWkd9ZjkknGYYPGkwP_zi4ZEO1ujG24_4_5r_YhU8KD32XB9-tsA8aH7KLG5Zo45IyaFsLJEllGIbmvPqWTumWtn-tYuggoz84E5_JXqLrD8UmdHSmBDiZZKGBujDoV9W3iBOqGjGsGyE6LPUCxXqDwrduDpt7Q1DlsC3zX5SrtIOCxk3I_zOW_Gk3j78Zrh5ptoe67G_lCgnlSr_lB4Kn_UIfFyDKWxIAN_5aHKr7Voxn2cX_oCpVSZ1pEcviTPFDReRelciICWr0001zd2UaU7r6tSowmjqdCWzreK9IT65y5AAUxUh6IRAs8fo-cOB4vu24Y_pGMflp-Cg9fiHQBA03BzZ9ZF9_IXh6XIDHFyjMLdyh2xFuA8r7uDs0xACN8bxYjY6IJLlmGm5ITxa3hyyNN-VIjHO1oLfoP2KO1c-RwCe8";

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
        public const string DB_SERVER_IP = "http://localhost:8001/api/";

        //Pol% Threshold for not neutral
        public const int POL_PERCENT_THRESHOLD = 1; //Percent
        public const double POL_VALUE_THRESHOLD = 0.5; //Value
    }
}
