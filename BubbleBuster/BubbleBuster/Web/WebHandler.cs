using BubbleBuster.Helper;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace BubbleBuster.Web
{
    public class WebHandler
    {
        private static string _cred = "Bearer AAAAAAAAAAAAAAAAAAAAAPRw2QAAAAAAsXqGsVRPgYFVjSScMX3ZVa9YifA%3DkPvipEcLJj3QooYO7aVke3vZ9ruSJp9CgkTlKKtvlmSsGqLUdG";


        public WebHandler(string apiKey) : base()
        {

        }

        public WebHandler()
        {
            ServicePointManager.DefaultConnectionLimit = 4; //Because at normal operation at most 3 threads should be running
        }


        public string MakeRequest(string requestString)
        {
            string res = "";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(requestString);
            request.Headers[HttpRequestHeader.Authorization] = _cred;
            request.UserAgent = Constants.USER_AGENT;
            request.Method = "GET";
            request.Timeout = 1800000;

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

                try
                {
                    res = readStream.ReadToEnd();
                }
                catch (IOException)
                {

                }
                    
                

                response.Close();
                readStream.Close();

            }

            return res;
        }





        public T MakeRequest<T>(string requestString) where T : new()
        {
            T res = default(T);
            if (typeof(IEnumerable).IsAssignableFrom(typeof(T)))
            {
                res = new T();
            }
            
            T tempRes = default(T);

            try
            {
                string data = MakeRequest(requestString);

                tempRes = JsonConvert.DeserializeObject<T>(data);
            }
            catch (JsonException e)
            {
                Console.WriteLine(e.Message);
            }

            if(tempRes != null)
            {
                res = tempRes;
            }

            return res;
        }
    }
}
