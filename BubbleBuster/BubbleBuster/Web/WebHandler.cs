using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BubbleBuster.Web
{
    public static class WebHandler
    {
        private static string _cred = "Bearer AAAAAAAAAAAAAAAAAAAAAPRw2QAAAAAAsXqGsVRPgYFVjSScMX3ZVa9YifA%3DkPvipEcLJj3QooYO7aVke3vZ9ruSJp9CgkTlKKtvlmSsGqLUdG";





        public static string MakeRequest(string requestString)
        {
            string res = "";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(requestString);
            request.Headers[HttpRequestHeader.Authorization] = _cred;
            request.UserAgent = "FilterBubble_SW709";
            request.Method = "GET";

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            if (response.StatusCode == HttpStatusCode.OK)
            {
                Stream receiveStream = response.GetResponseStream();
                StreamReader readStream = null;

                if (response.CharacterSet == null)
                {
                    readStream = new StreamReader(receiveStream);
                }
                else
                {
                    readStream = new StreamReader(receiveStream, Encoding.GetEncoding(response.CharacterSet));
                }

                res = readStream.ReadToEnd();

                response.Close();
                readStream.Close();

            }

            return res;
        }





        public static T MakeRequest<T>(string requestString)
        {
            T res = default(T);

            try
            {
                string data = MakeRequest(requestString);

                res = JsonConvert.DeserializeObject<T>(data);
            }
            catch (JsonException e)
            {
                Console.WriteLine(e.Message);
            }

            return res;
        }
    }
}
