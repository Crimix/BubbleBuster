using System;
using System.Net;
using System.Text;

namespace StressTesting
{
    class Program
    {
        static void Main(string[] args)
        {
            //string target = "localhost:62020/API/AnalyzeTwitterAccount";
            //
            //

            string[] para = { "Token", "Name", "Secret", "RequestID", "RequesterName" };

            Console.WriteLine( DatabaseSendDataRequest(para) );

            bool DatabaseSendDataRequest(params string[] parameters)
            {
                bool result = false;

                //Construct the parameter string 
                string postData = "";
                foreach (var item in parameters)
                {
                    postData += item + "&";
                }
                postData = postData.Trim('&');
                //Prepare the payload to be written to the request stream
                var data = Encoding.ASCII.GetBytes(postData);

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("localhost:62020/API/AnalyzeTwitterAccount");
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = data.Length;
                request.Timeout = 1800000;

                using (var stream = request.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }

                HttpWebResponse response = null;
                try
                {
                    response = (HttpWebResponse)request.GetResponse();

                    if (response.StatusCode == HttpStatusCode.OK || response.StatusCode == HttpStatusCode.Created)
                    {
                        result = true;
                    }
                    else
                    {
                        result = false;
                    }
                    //Close the connection
                    response?.Close();
                }
                catch (WebException e)
                {
                    //Close the connections
                    response?.Close();
                    //Log.Error("Stuff Broke, lol");
                }

                return result;

            }
        }
    }
}
