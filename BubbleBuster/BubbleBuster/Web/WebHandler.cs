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

        public WebHandler(AuthObj auth)
        {
            this.auth = auth;
            ServicePointManager.DefaultConnectionLimit = 4; //Because at normal operation at most 4 threads should be running
        }

        public string MakeRequest(UrlObject requestObject, string apiKey)
        {
            string res = "";
            GetRequestBody(requestObject, apiKey, ref res);
            return res;
        }

        public string MakeRequest(UrlObject requestObject)
        {
            string res = "";
            string authHeader = OAuthHelper.Instance.BuildAuthHeader(OAuthHelper.DataType.GET, auth.RequesterName, auth.OAuthToken, auth.OAuthTokenSecret, requestObject.BaseUrl, requestObject.Params);
            GetRequestBody(requestObject, authHeader, ref res);
            return res;
        }

        private bool GetRequestBody(string requestString, object auth, ref string result)
        {
            bool res = false;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(requestString);
            request.Headers[HttpRequestHeader.Authorization] = auth.ToString();
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
                    result = readStream.ReadToEnd();
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

        private bool GetRequestBody(UrlObject requestObject, object auth, ref string result)
        {
            return GetRequestBody(requestObject.Url, auth, ref result);
        }

        public T MakeRequest<T>(UrlObject requestObject,string apiKey) where T : new()
        {
            return GetRequestBody<T>(requestObject, (()=> MakeRequest(requestObject, apiKey)));
        }

        public T MakeRequest<T>(UrlObject requestObject) where T : new()
        {
            return GetRequestBody<T>(requestObject, (() => MakeRequest(requestObject)));
        }

        private T GetRequestBody<T>(UrlObject requestObject, Func<String> func) where T : new()
        {
            T res = default(T);

            if (typeof(IEnumerable).IsAssignableFrom(typeof(T)))
            {
                res = new T();
            }

            T tempRes = default(T);

            try
            {
                string data = func();
                tempRes = JsonConvert.DeserializeObject<T>(data);
            }
            catch (JsonException e)
            {
                lock (Log.LOCK)
                {
                    Log.Info(e.Message);
                }
            }

            if (tempRes != null)
            {
                res = tempRes;
            }

            return res;
        }

        public bool DBGetRequest(string requestString, ref int result, params string[] parameters)
        {
            bool res = false;
            string tempResult = "";
            foreach (var item in parameters)
            {
                requestString += item + "&";
            }
            requestString = requestString.Trim('&');


            if(GetRequestBody(requestString, "Bearer " + Constants.DB_CREDS, ref tempResult))
            {
                res = true;
                result = Convert.ToInt32(tempResult);
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
