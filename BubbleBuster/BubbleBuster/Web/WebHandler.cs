using BubbleBuster.Helper;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using BubbleBuster.Helper.Objects;

namespace BubbleBuster.Web
{
    public class WebHandler
    {
        private AuthObj auth;

        public WebHandler(AuthObj auth) : base()
        {
            this.auth = auth;
        }

        private WebHandler()
        {
            ServicePointManager.DefaultConnectionLimit = 4; //Because at normal operation at most 4 threads should be running
        }

        public string MakeRequest(string requestString)
        {
            string res = "";
            string[] urlParts = requestString.Split('?');
            string authHeader = "";
            if(urlParts.Length < 1)
            {
                authHeader = OAuthHelper.Instance.BuildAuthHeader(OAuthHelper.DataType.GET, auth.RequesterName, auth.OAuthToken, auth.OAuthTokenSecret, urlParts[0]);
            }
            else
            {
                OAuthHelper.Instance.BuildAuthHeader(OAuthHelper.DataType.GET, auth.Name, auth.OAuthToken, auth.OAuthTokenSecret,requestString);
            }
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(requestString);
            request.Headers[HttpRequestHeader.Authorization] = 
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

        public bool DBPostRequest (string requestString)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(requestString);
            request.Headers[HttpRequestHeader.Authorization] = "Bearer " + Constants.DB_CREDS;
            request.Method = "POST";
            request.Timeout = 1800000;

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            if(response.StatusCode == HttpStatusCode.OK)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
