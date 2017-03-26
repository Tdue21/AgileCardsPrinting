// ********************************************************************
// * Copyright © 2014 Scanvaegt Nordic A/S
// *
// * This file is the property of Scanvaegt Nordic A/S and may not be 
// * distributed without the written consent of the author.    
// ********************************************************************
// 	

using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using PrintIssueCards.Properties;

namespace PrintIssueCards.Jira.Rest
{
    public class SimpleRestClient
    {
        private readonly Uri _hostAddress;
        private readonly string _userName;
        private readonly string _password;

        public SimpleRestClient(string hostAddress, string userName, string password)
        {
            if (hostAddress == null)
            {
                throw new ArgumentNullException("hostAddress");
            }
            _hostAddress = new Uri(hostAddress);
            _userName = userName;
            _password = password;
        }

        public async Task<string> GetAsync([NotNull] string resource)
        {
            if (string.IsNullOrEmpty(resource))
            {
                throw new ArgumentNullException("resource");
            }
            var authHeader = string.Format("{0}:{1}", _userName, _password);
            var authBytes = Encoding.GetEncoding("ibm850").GetBytes(authHeader);
            var authEncoded = Convert.ToBase64String(authBytes);


            var uri = new Uri(_hostAddress, resource);
            var request = (HttpWebRequest) WebRequest.Create(uri);
            request.Headers[HttpRequestHeader.Authorization] = "Basic " + authEncoded;
            request.Method = "GET";

            Stream stream = null;
            StreamReader reader = null;
            WebResponse response = null;
            try
            {
                response = await request.GetResponseAsync();
                stream = response.GetResponseStream();
                if (stream != null)
                {
                    reader = new StreamReader(stream);
                }
                return reader.ReadToEnd();
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                    reader.Dispose();
                }
                if (stream != null)
                {
                    stream.Close();
                    stream.Dispose();
                }
                if (response != null)
                {
                    response.Close();
                    response.Dispose();
                }
    
            }
        }
    }
}
