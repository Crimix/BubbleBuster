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

        //Auth string for DB server on Christoffer's pc
        public const string DB_CREDS = "eyJ0eXAiOiJKV1QiLCJhbGciOiJSUzI1NiIsImp0aSI6IjczYTNkNWIzOGJhNDk5OTEwOTUzOGM3MjU5ZmRlOGVkZjE0NGQ3NmM2MjdmMDE3NTQxMWFhYjI5MzVlYjI3NzdhNzYyZTRhN2RlYTA2YTEwIn0.eyJhdWQiOiIxIiwianRpIjoiNzNhM2Q1YjM4YmE0OTk5MTA5NTM4YzcyNTlmZGU4ZWRmMTQ0ZDc2YzYyN2YwMTc1NDExYWFiMjkzNWViMjc3N2E3NjJlNGE3ZGVhMDZhMTAiLCJpYXQiOjE1MTEzNTU3NDIsIm5iZiI6MTUxMTM1NTc0MiwiZXhwIjoxNTQyODkxNzQyLCJzdWIiOiIiLCJzY29wZXMiOltdfQ.S6UxLdJHwoI2j_P_pdz4hFF7W95TKwoXwLZKf30JsLscjCa3GVrIA_dNVkTxxDjtF6-v_Kjh4L_gXXGPp_Qw15pQ-sb_e-5ZMJ6dyCV8y1dhDB0Geb61_6eV8FAEhrwUd7WtNAOMOZOD92ZiBdreXpEXbhmYruOpLyepmSOLaOgCErARPDU-7J9DFZiLgAC3k_Xfq-ln-cBoTcQDBcoAf3G1MU5QjKhgfW1Ior8hliWrPTV2tkp61wi82JCCkllvJoSDxfylDehiLtxFWe0mfjJfsaTSVvxMlx1pVRUXRXOkpdV9llv0oXV8LXmihQgfTgEEUQsqkbOQ2M0lcIvVTofPqWaA8rd_wQ-xHXr05jH2fq_P7v_RaVrujEnYgwnLFZC84i7VDT3FfI-w8weDtt9Mfnhm1XIel9mHZg4EAFtlzg_JJeHvZxIKHXS4thI83N_q2V1vWy6ZaIxuGKli9yMB-Pz_6G02SLZXKqhwwxlkOxbAYCC5LmqNLSZmSZdRVXA-0UJGyaBPFb68BadkiwIyE8kvmXm1A6uYsz9-5GRTI4_l2jmaJm7QxuNft3u1L_AHHmFhzYriAvFGbH2F7-wstlKkwJsB8l0DrAupb4xEOC4H4MwaH5H2Poqd9rMK1IC9Y1WKyfqhYUmOnKoiLo6IzOigHaDa-0Pnim8JODc";

        public static string PROGRAM_DATA_FILEPATH { get { return PROGRAM_DATA + @"\\BubbleBuster"; } }

        //Constants used for tweet analysis
        public const int HASHTAG_WEIGHT = 1; //Factor. 1 = base. Do not use negative values.
        public const int URL_WEIGHT = 1; //Factor. 1 = base. Do not use negative values.

        //Application keys
        public const string CONSUMER_KEY = "fIbIn9yMl0F7dpWUoihhwAD3N";
        public const string CONSUMER_SECRET = "qZOQqOia1XnGuWbfDKmuxxykTCAzLbmVbdZie40w6AFuuFHy4F";
        public const int TWEET_LIST_AMOUNT = 8; //How many theads should the tweet list be split to 

        public const string DB_SERVER_IP = "http://localhost:8000/api/";
    }
}
