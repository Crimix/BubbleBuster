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
        //Varible to contain the supplied auth object
        private AuthObj auth;

        public WebHandler(AuthObj auth)
        {
            this.auth = auth;
            ServicePointManager.DefaultConnectionLimit = 4; //Because at normal operation at most 4 threads should be running
        }

        /// <summary>
        /// Get request for twitter.
        /// </summary>
        /// <param name="requestObject">The request object</param>
        /// <returns>A string representation of the result of the request</returns>
        public string TwitterGetRequest(RequestUrlObject requestObject)
        {
            string res = "";
            string authHeader = "";
            if (auth.Type == AuthObj.AuthType.User)
            {
                authHeader = OAuthHelper.Instance.BuildAuthHeader(OAuthHelper.RequestMethod.GET, auth.Name, auth.OAuthToken, auth.OAuthTokenSecret, requestObject.BaseUrl, requestObject.Params);
            }
            else if (auth.Type == AuthObj.AuthType.App)
            {
                authHeader = auth.APIKey;
            }
            GetRequestBody(requestObject, authHeader, ref res);
            return res;
        }


        /// <summary>
        /// Get request for twitter.
        /// </summary>
        /// <typeparam name="T">The return type T</typeparam>
        /// <param name="requestObject">The request object</param>
        /// <returns>The result represented as an object of type T</returns>
        public T TwitterGetRequest<T>(RequestUrlObject requestObject) where T : new() //Needed such that a new instance of an object can be made
        {
            T res = default(T); // First set it to its default

            //Then if it is a list (IEnumerable), then it creates a new instance of it. 
            //Such that it is not null
            if (typeof(IEnumerable).IsAssignableFrom(typeof(T)))
            {
                res = new T();
            }

            //Such that if it fails to get an result or deserialize it just is null
            T tempRes = default(T);

            try
            {
                string data = TwitterGetRequest(requestObject);
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


        /// <summary>
        /// Database Get request to retrieve information from the database
        /// </summary>
        /// <param name="requestUrl">The request url</param>
        /// <param name="result">The result of the request</param>
        /// <param name="parameters">The parameters of the request</param>
        /// <returns>True if the request succeed</returns>
        public bool DatabaseGetRequest(string requestUrl, ref object result, params string[] parameters)
        {
            bool res = false;
            string tempResult = "";

            //Appends the parameters
            foreach (var item in parameters)
            {
                requestUrl += item + "&";
            }
            requestUrl = requestUrl.Trim('&');

            //Uses the private method to get the result
            if(GetRequestBody(requestUrl, "Bearer " + Constants.DB_CREDS, ref tempResult))
            {
                res = true;
                result = tempResult;
            }
            return res;
        }

        /// <summary>
        /// Database post request to post something to the database
        /// </summary>
        /// <param name="requestUrl">The request url</param>
        /// <param name="method">Can be either POST or PUT</param>
        /// <param name="parameters">The parameters of the request</param>
        /// <returns>True if the request succeed</returns>
        public bool DatabaseSendDataRequest(string requestUrl, string method, params string[] parameters)
        {
            foreach (var item in parameters)
            {
                requestUrl += item + "&";
            }
            requestUrl = requestUrl.Trim('&');

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(requestUrl);
            request.Headers[HttpRequestHeader.Authorization] = "Bearer " + Constants.DB_CREDS;
            request.Method = method;
            request.Timeout = 1800000;

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            if(response.StatusCode == HttpStatusCode.OK || response.StatusCode == HttpStatusCode.Created)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //Private helper method to return the result of an get request
        private bool GetRequestBody(RequestUrlObject requestObject, object auth, ref string result)
        {
            return GetRequestBody(requestObject.Url, auth, ref result);
        }

        //Private helper method to return the result of an get request
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
    }
}
