﻿using System;
using System.IO;
using System.Net;
using System.Text;
using Newtonsoft.Json;

namespace NetLab.Extensions.Request
{
    public class Request
    {
        public String URL { get; set; }

        /// <summary>
        /// HTTP Verb wich will be used. Eg. GET, POST, PUT, DELETE.
        /// </summary>
        public String Verb { get; set; }

        /// <summary>
        /// Request content, Json by default.
        /// </summary>
        public String Content
        {
            get { return "text/json"; }
        }

        public class Credentials
        {
            public string UserName { get; set; }
            public string Password { get; set; }
        }
        /// <summary>
        /// User and Password for Basic Authentication
        /// </summary>
        public Credentials Credentiales { get; set; }

        public HttpWebRequest HttpRequest { get; internal set; }
        public HttpWebResponse HttpResponse { get; internal set; }
        public CookieContainer CookieContainer = new CookieContainer();

        /// <summary>
        /// Constructor Overload that allows passing URL and the VERB to be used.
        /// </summary>
        /// <param name="url">URL which request will be created</param>
        /// <param name="verb">Http Verb that will be userd in this request</param>
        public Request(string url, string verb)
        {
            URL = url;
            Verb = verb;
        }

        /// <summary>
        /// Default constructor overload without any paramter
        /// </summary>
        public Request()
        {
            Verb = "GET";
        }

        public object Execute<TT>(string url, object obj, string verb)
        {
            if (url != null)
                URL = url;

            if (verb != null)
                Verb = verb;

            HttpRequest = CreateRequest();

            WriteStream(obj);

            try
            {
                HttpResponse = (HttpWebResponse)HttpRequest.GetResponse();
            }
            catch (WebException error)
            {
                HttpResponse = (HttpWebResponse)error.Response;
                return ReadResponseFromError(error);
            }

            return JsonConvert.DeserializeObject<TT>(ReadResponse());
        }

        public object Execute<TT>(string url)
        {
            if (url != null)
                URL = url;

            HttpRequest = CreateRequest();

            try
            {
                HttpResponse = (HttpWebResponse)HttpRequest.GetResponse();
            }
            catch (WebException error)
            {
                HttpResponse = (HttpWebResponse)error.Response;
                return ReadResponseFromError(error);
            }

            return JsonConvert.DeserializeObject<TT>(ReadResponse());
        }

        public object Execute<TT>()
        {
            if (URL == null)
                throw new ArgumentException("URL cannot be null.");

            HttpRequest = CreateRequest();

            try
            {
                HttpResponse = (HttpWebResponse)HttpRequest.GetResponse();
            }
            catch (WebException error)
            {
                HttpResponse = (HttpWebResponse)error.Response;
                return ReadResponseFromError(error);
            }

            return JsonConvert.DeserializeObject<TT>(ReadResponse());
        }

        public object Execute(string url, object obj, string verb)
        {
            if (url != null)
                URL = url;

            if (verb != null)
                Verb = verb;

            HttpRequest = CreateRequest();

            WriteStream(obj);

            try
            {
                HttpResponse = (HttpWebResponse)HttpRequest.GetResponse();
            }
            catch (WebException error)
            {
                HttpResponse = (HttpWebResponse)error.Response;
                return ReadResponseFromError(error);
            }

            return ReadResponse();
        }

        public object Execute(string url)
        {
            if (url != null)
                URL = url;

            HttpRequest = CreateRequest();

            try
            {
                HttpResponse = (HttpWebResponse)HttpRequest.GetResponse();
            }
            catch (WebException error)
            {
                HttpResponse = (HttpWebResponse)error.Response;
                return ReadResponseFromError(error);
            }

            return ReadResponse();
        }

        public object Execute()
        {
            if (URL == null)
                throw new ArgumentException("URL cannot be null.");

            HttpRequest = CreateRequest();

            try
            {
                HttpResponse = (HttpWebResponse)HttpRequest.GetResponse();
            }
            catch (WebException error)
            {
                HttpResponse = (HttpWebResponse)error.Response;
                return ReadResponseFromError(error);
            }

            return ReadResponse();
        }

        internal HttpWebRequest CreateRequest()
        {
            var basicRequest = (HttpWebRequest)WebRequest.Create(URL);
            basicRequest.ContentType = Content;
            basicRequest.Method = Verb;
            basicRequest.CookieContainer = CookieContainer;
            basicRequest.Headers.Add("x-api-token", "1b9903b16ad3cdff548d7fb58d06c93b7945947551a1c4b6389d40292a11faf3");
            basicRequest.Headers.Add("cache-control", "no-cache");
            basicRequest.ContentType = "application/json";

            if (Credentiales != null)
                basicRequest.Headers.Add("Authorization", "Basic" + " " + EncodeCredentials(Credentiales));

            return basicRequest;
        }

        internal void WriteStream(object obj)                   
        {
            if (obj != null)
            {
                using (var streamWriter = new StreamWriter(HttpRequest.GetRequestStream()))
                {
                    if (obj is string)
                        streamWriter.Write(obj);
                    else
                        streamWriter.Write(JsonConvert.SerializeObject(obj));
                }
            }
        }

        internal String ReadResponse()
        {
            if (HttpResponse != null)
                using (var streamReader = new StreamReader(HttpResponse.GetResponseStream()))
                    return streamReader.ReadToEnd();

            return string.Empty;
        }

        internal String ReadResponseFromError(WebException error)
        {
            using (var streamReader = new StreamReader(error.Response.GetResponseStream()))
                return streamReader.ReadToEnd();
        }

        internal static string EncodeCredentials(Credentials credentials)
        {
            var strCredentials = string.Format("{0}:{1}", credentials.UserName, credentials.Password);
            var encodedCredentials = Convert.ToBase64String(Encoding.UTF8.GetBytes(strCredentials));

            return encodedCredentials;
        }

    }
}