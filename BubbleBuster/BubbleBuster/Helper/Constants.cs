using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BubbleBuster.Helper
{
    public static class Constants
    {
        public const int REMAINING_OFFSET = 5; //How many there should be left of each request type
        public const string USER_AGENT = "FilterBubble_SW709"; //The name of the APP
        public const int TWEETS_TO_RETRIEVE = 3200; //How many tweets to retrieve per user.
        private static string PROGRAM_DATA = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData); //The path to the files

        //Auth string for DB server on Christoffer's pc
        public const string DB_CREDS = "eyJ0eXAiOiJKV1QiLCJhbGciOiJSUzI1NiIsImp0aSI6ImNkMGFlOTAxNTgyNGIzMGMzYWYxMzE4ODU4MTUyZjEyMGRkYmVjZDc2NGZkZGEzM2NkYjllOTgzZWE1MDE4Zjg0MzIxODRiNjMzN2EwN2M5In0.eyJhdWQiOiIxIiwianRpIjoiY2QwYWU5MDE1ODI0YjMwYzNhZjEzMTg4NTgxNTJmMTIwZGRiZWNkNzY0ZmRkYTMzY2RiOWU5ODNlYTUwMThmODQzMjE4NGI2MzM3YTA3YzkiLCJpYXQiOjE1MTIzOTk1MzAsIm5iZiI6MTUxMjM5OTUzMCwiZXhwIjoxNTQzOTM1NTMwLCJzdWIiOiIiLCJzY29wZXMiOltdfQ.Cv30eccCWPCmC-1kFnrSLz4YMRR9UexKXOkizcpq_c6fjtOfHADZ35cpYHW5AV_f1ZnwAajwnpbY8YE4hc1HcSKn8n5Qepb0Ag0dKfqZr6xxs4J971I3ffHRSXGGTc49-N1hwLhs3NKbhp6qY6I77itlx2VdV57GZNmn3kLLTWB1dy_waBcaRA91HmEfrOU--2XNT8XugHo31qsuQrbyuiXMiWYAX-ReWPfXMormUaKbavxC8eYkIVkNiojQ6-p26GltiXw_L-idShLAWJuE3OWNhl0MRnDkz1FoESqDprcAQQ6lzWg3yNCwSYg3aXKZvQ0wREp9JNfZRQUkfZE6BZsjpcPkIASORVS0IKsXAXbtc970XfJCcXsLYD84LNk_08nsJcf4LUWDLw8MfuFtPrAaL3wvZQDv1zB1-wsUS247dIdEGG_YQqX7Dm92c_51siW36p46WMNSeLdRc3vtetNQ7xN9gFqm0mTEBwMRUjJ0XaFSvhcJDmw7T97xCdWi0kZ_qBcQ-d7ajunDBa2QyURb831GjU0yz1lUr4FJyzPoXs1paaZYuzyC8fiGW0KrMzemxEU6t_dksCyWzgin-lC7AtfqigbfHAO5T4X5Gmz1foYmR8zmHM-9pwQK-II_OhBymYow0kHd9DPZYOdBZBAU513zMGjdA7PuCx0nibA";

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
    }
}
