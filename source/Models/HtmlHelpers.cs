using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace Microsoft_Teams_Graph_RESTAPIs_Connect.Models
{
    public static class Statics
    {
        public static T Deserialize<T>(this string result)
        {
            if (typeof(T) == typeof(Group))
            {
                // Work around known Graph bug (fix in progress)
                //Thread.Sleep(10000);
            }
            return JsonConvert.DeserializeObject<T>(result, HttpHelpers.jsonSettings);
        }
    }

    public class HttpHelpers
    {
        public HttpClient httpClient = new HttpClient();
        public string accessToken;
        public string graphV1Endpoint = "https://graph.microsoft.com/v1.0";
        public string graphBetaEndpoint = "https://graph.microsoft.com/beta";
        public static readonly JsonSerializerSettings jsonSettings =
            new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() };

        public static async Task<IEnumerable<T>> ParseList<T>(HttpResponseMessage response)
        {
            if (response != null && response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                var t = JsonConvert.DeserializeObject<ResultList<T>>(content, HttpHelpers.jsonSettings);
                return t.value;
            }
            return new T[0];
        }

        public static T[] ParseList<T>(string content)
        {
            if (content != null)
            {
                var t = JsonConvert.DeserializeObject<ResultList<T>>(content, HttpHelpers.jsonSettings);
                return t.value;
            }
            return new T[0];
        }


        public async Task<T> HttpGet<T>(string uri, string endpoint = null, int retries = 0, int retryDelay = 30)
        {
            string response = await CallGraph(HttpMethod.Get, uri, endpoint: endpoint, retries: retries, retryDelay: retryDelay);
            T result = JsonConvert.DeserializeObject<T>(response, jsonSettings);
            return result;
        }

        public async Task<T[]> HttpGetList<T>(string uri, string endpoint = null, int retries = 0, int retryDelay = 30)
        {
            string response = await CallGraph(HttpMethod.Get, uri, endpoint: endpoint, retries: retries, retryDelay: retryDelay);
            T[] result = ParseList<T>(response);
            return result;
        }

        public async Task<string> HttpPost(string uri, object body, string endpoint = null, int retries = 0, int retryDelay = 30)
        {
            return await CallGraph(HttpMethod.Post, uri, body, endpoint: endpoint, retries: retries, retryDelay: retryDelay);
        }
        public async Task<HttpResponse> HttpPostWithHeaders(string uri, object body, int retries = 0, int retryDelay = 30)
        {
            return await CallGraphWithHeaders(HttpMethod.Post, uri, body, retries: retries, retryDelay: retryDelay);
        }

        public async Task<string> HttpPut(string uri, object body, string endpoint = null, int retries = 0, int retryDelay = 30)
        {
            return await CallGraph(HttpMethod.Put, uri, body, endpoint: endpoint, retries: retries, retryDelay: retryDelay);
        }

        public async Task<string> HttpPatch(string uri, object body, string endpoint = null, int retries = 0)
        {
            return await CallGraph(new HttpMethod("PATCH"), uri, body, endpoint: endpoint, retries: retries);
        }

        // Hack -- needed a static version that doesn't have the defaults for endpoint & bearer token
        public static async Task<string> POST(string url, string body)
        {
            HttpClient httpClient = new HttpClient();

            var request = new HttpRequestMessage(HttpMethod.Post, url);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            request.Content = new StringContent(body, Encoding.UTF8, "application/x-www-form-urlencoded");
            HttpResponseMessage response = await httpClient.SendAsync(request);
            string responseBody = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
                throw new Exception(responseBody);
            return responseBody;
        }

        public async Task<string> HttpDelete(string uri, string endpoint = null)
        {
            return await CallGraph(HttpMethod.Delete, uri, endpoint: endpoint);
        }

        public class HttpResponse
        {
            public string Body;
            public HttpResponseHeaders Headers;
        }

        public async Task<string> CallGraph(HttpMethod method, string uri, object body = null, string endpoint = null, int retries = 0, int retryDelay = 30)
        {
            return (await CallGraphWithHeaders(method, uri, body, endpoint, retries, retryDelay: retryDelay)).Body;
        }

        public async Task<HttpResponse> CallGraphWithHeaders(HttpMethod method, string uri, object body = null, string endpoint = null, int retries = 0, int retryDelay = 30)
        {
            if (endpoint == null)
                endpoint = graphV1Endpoint;

            string bodyString;
            if (body == null)
                bodyString = null;
            else if (body is string)
                bodyString = body as string;
            else
                bodyString = JsonConvert.SerializeObject(body, jsonSettings);

            var request = new HttpRequestMessage(method, endpoint + uri);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            if (method != HttpMethod.Get && method != HttpMethod.Delete)
                request.Content = new StringContent(bodyString, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await httpClient.SendAsync(request);
            string responseBody = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                if (retries > 0)
                {
                    Thread.Sleep(retryDelay * 1000);
                    return await CallGraphWithHeaders(method, uri, body, endpoint: endpoint, retries: retries - 1, retryDelay: retryDelay);
                }
                else
                    throw new Exception(responseBody);
            }
            return new HttpResponse() { Body = responseBody, Headers = response.Headers };
        }

        // HACK: Only for clone -- needs headers
        public async Task<HttpResponseMessage> HttpRequest(HttpMethod method, string uri, object body = null, string endpoint = null, int retries = 0)
        {
            // BUG: Implement retries

            if (endpoint == null)
                endpoint = graphV1Endpoint;
            string bodyString = (body == null) ? null : JsonConvert.SerializeObject(body, jsonSettings);

            Debug.Assert(accessToken != null);
            var request = new HttpRequestMessage(method, endpoint + uri);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            if (method != HttpMethod.Get)
                request.Content = new StringContent(bodyString, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await httpClient.SendAsync(request);
            string responseBody = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
                throw new Exception(responseBody);
            return response;
        }
    }
}