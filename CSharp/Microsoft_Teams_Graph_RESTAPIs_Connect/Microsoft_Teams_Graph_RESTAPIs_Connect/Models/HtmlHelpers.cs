using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
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
            return JsonConvert.DeserializeObject<T>(result);
        }
    }

    public class HttpHelpers
    {
        public HttpClient httpClient = new HttpClient();
        public string accessToken;
        public string graphBetaEndpoint = "https://graph.microsoft.com/beta";
        public string graphTestEndpoint = "https://graph.microsoft.com/testTeams1";

        public static async Task<IEnumerable<T>> ParseList<T>(HttpResponseMessage response)
        {
            if (response != null && response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                var t = JsonConvert.DeserializeObject<ResultList<T>>(content);
                return t.value;
            }
            return new T[0];
        }

        public static IEnumerable<T> ParseList<T>(string content)
        {
            if (content != null)
            {
                var t = JsonConvert.DeserializeObject<ResultList<T>>(content);
                return t.value;
            }
            return new T[0];
        }


        public async Task<string> HttpGet(string uri, string endpoint = null)
        {
            return await CallGraph(HttpMethod.Get, uri, endpoint: endpoint);
        }

        public async Task<string> HttpPatch(string uri, object body, string endpoint = null)
        {
            return await CallGraph(new HttpMethod("PATCH"), uri, body, endpoint: endpoint);
        }

        public async Task<string> HttpPost(string uri, object body, string endpoint = null)
        {
            return await CallGraph(HttpMethod.Post, uri, body, endpoint: endpoint);
        }
        public async Task<HttpResponse> HttpPostWithHeaders(string uri, object body)
        {
            return await CallGraphWithHeaders(HttpMethod.Post, uri, body);
        }

        public async Task<string> HttpPut(string uri, object body, string endpoint = null)
        {
            return await CallGraph(HttpMethod.Put, uri, body, endpoint: endpoint);
        }

        public class HttpResponse
        {
            public string Body;
            public HttpResponseHeaders Headers;
        }

        public async Task<string> CallGraph(HttpMethod method, string uri, object body = null, string endpoint = null)
        {
            return (await CallGraphWithHeaders(method, uri, body, endpoint)).Body;
        }

        public async Task<HttpResponse> CallGraphWithHeaders(HttpMethod method, string uri, object body = null, string endpoint = null)
        {
            if (endpoint == null)
                endpoint = graphBetaEndpoint;

            string bodyString;
            if (body == null)
                bodyString = null;
            else if (body is string)
                bodyString = body as string;
            else
                bodyString = JsonConvert.SerializeObject(body);

            var request = new HttpRequestMessage(method, endpoint + uri);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            if (method != HttpMethod.Get)
                request.Content = new StringContent(bodyString, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await httpClient.SendAsync(request);
            string responseBody = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
                throw new Exception(responseBody);
            return new HttpResponse() { Body = responseBody, Headers = response.Headers };
        }

        public async Task<HttpResponseMessage> HttpRequest(HttpMethod method, string uri, object body = null, string endpoint = null)
        {
            if (endpoint == null)
                endpoint = graphBetaEndpoint;
            string bodyString = (body == null) ? null : JsonConvert.SerializeObject(body);

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