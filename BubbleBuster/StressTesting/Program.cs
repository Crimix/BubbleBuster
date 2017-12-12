using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace StressTesting
{
    class Program
    {
        static void Main(string[] args)
        {

            List<string> name = new List<string>();
            name.Add("fahrenthold");
            name.Add("wpjenna");
            name.Add("cbellantoni");
            name.Add("Atrios");
            name.Add("Nicopitney");
            name.Add("ggreenwald");
            name.Add("stevebenen");
            name.Add("AlanColmes");
            name.Add("MuslimIQ");
            name.Add("MaggieNYT");
            name.Add("StephenAtHome");
            name.Add("TrevorNoah");
            name.Add("Gavin_McInnes");
            name.Add("PrisonPlanet");
            name.Add("Ewerickson");
            name.Add("mindyfinn");
            name.Add("dmataconis");
            name.Add("TPCarney");
            name.Add("jbarro");
            name.Add("heminator");
            name.Add("reihansalam");
            name.Add("michellemalkin");
            name.Add("sarahpalinUSA");
            name.Add("glennbeck");
            name.Add("mitchellvii");

            Console.WriteLine(name.Count);
            Console.ReadLine();

            foreach (var item in name)
            {
                Console.WriteLine(DatabaseSendDataRequest("Token=14594669-CUFTDY6oMmC6FIu3IwHY65G26cQHmmuc9XhHuQFaL", "Name=" +item , "Secret=uzV2suIyfoTaHDgbZERCsxBlqjMaFFCfGJ0yNDbra5b5K", "RequestID=" + item, "RequesterName=test"));
            }


            Console.ReadLine();


        }


        private static bool DatabaseSendDataRequest(params string[] parameters)
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

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://localhost:62020/API/AnalyzeTwitterAccount");
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
            }

            return result;

        }
    }
}
