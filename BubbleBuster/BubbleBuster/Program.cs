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
                SaveDataToFile(@"\BubbleBuster", "test.txt", data);

                response.Close();
                readStream.Close();

                Console.WriteLine();
                Console.ReadLine();
            }
        }

        private static void SaveDataToFile(string folderName, string fileName, string data)
        {
            string folderPath = Path.GetTempPath() + folderName;
            string filePath = Path.GetTempPath() + folderName + @"\" + fileName;
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
            File.WriteAllText(filePath, data);
        }
    }
}
