using System;
using System.Collections.Generic;
using System.Net;
using System.Web.Script.Serialization;

namespace Vimeo
{
    public partial class VimeoClient
    {
        /// <summary>
        /// Gets the JSON response from Vimeo API
        /// </summary>
        /// <param name="url">The endpoint. It should be a relative URL specifying the API method you want to call, such as "/me"</param>
        /// <param name="parameters">Call parameters. Put null if you don't feel like it.</param>
        /// <param name="method">HTTP method: e.g. "GET", "POST", "PUT", etc.</param>
        /// <param name="jsonBody">true: set content type to json</param>
        /// <returns>JSON response string</returns>
        public string RequestJSON(string url, Dictionary<string, string> parameters, string method, bool jsonBody = true)
        {

            var headers = new WebHeaderCollection()
            {
                { "Authorization", String.Format("Bearer {0}", AccessToken) }
            };
            method = method.ToUpper();
            url = apiRoot + url;
            string body = "";
            string contentType = "application/x-www-form-urlencoded";

            if (parameters != null && parameters.Count > 0)
            {
                if (method == "GET")
                {
                    url += "?" + Helpers.KeyValueToString(parameters);
                }
                else if (method == "POST" || method == "PATCH" || method == "PUT" || method == "DELETE")
                {
                    if (jsonBody)
                    {
                        contentType = "application/json";
                        body = jsonEncode(parameters);
                    }
                    else
                    {
                        body = Helpers.KeyValueToString(parameters);
                    }
                }
            }

            return Helpers.HTTPFetch(url, method, headers, body, contentType);
        }
    }
}