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

        public string MakeRequest(UrlObject requestObject)
        {
            string res = "";
            string authHeader = OAuthHelper.Instance.BuildAuthHeader(OAuthHelper.DataType.GET, auth.RequesterName, auth.OAuthToken, auth.OAuthTokenSecret, requestObject.BaseUrl, requestObject.Params);

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(requestObject.Url);
            request.Headers[HttpRequestHeader.Authorization] = authHeader;
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

        public T MakeRequest<T>(UrlObject requestObject) where T : new()
        {
            T res = default(T);

            if (typeof(IEnumerable).IsAssignableFrom(typeof(T)))
            {
                res = new T();
            }
            
            T tempRes = default(T);

            try
            {
                string data = MakeRequest(requestObject);
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

        public bool DBGetRequest(string requestString, ref int result, params string[] parameters)
        {
            bool res = false;
            foreach (var item in parameters)
            {
                requestString += item + "&";
            }
            requestString = requestString.Trim('&');

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(requestString);
            request.Headers[HttpRequestHeader.Authorization] = "Bearer " + Constants.DB_CREDS;
            request.Method = "Get";
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
                    result = Convert.ToInt32(readStream.ReadToEnd());
                    res = true;
                }
                catch (IOException)
                {
                }

                response.Close();
                readStream.Close();
            }
            return res;
        }

        public bool DBPostRequest (string requestString, params string[] parameters)
        {
            foreach (var item in parameters)
            {
                requestString += item + "&";
            }
            requestString = requestString.Trim('&');

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
