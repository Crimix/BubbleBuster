using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BubbleBuster
{
    class Program
    {
        static void Main(string[] args)
        {
            string url = "https://api.twitter.com/1.1/friends/list.json?cursor=-1&screen_name=pewdiepie&skip_status=true&include_user_entities=false";

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);


            string _cred = "Bearer AAAAAAAAAAAAAAAAAAAAAPRw2QAAAAAAsXqGsVRPgYFVjSScMX3ZVa9YifA%3DkPvipEcLJj3QooYO7aVke3vZ9ruSJp9CgkTlKKtvlmSsGqLUdG";
            request.Headers[HttpRequestHeader.Authorization] = _cred;

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

                string data = readStream.ReadToEnd();


                
                response.Close();
                readStream.Close();

                

                Console.WriteLine(data);
                Console.ReadLine();
            }
        }
    }
}
