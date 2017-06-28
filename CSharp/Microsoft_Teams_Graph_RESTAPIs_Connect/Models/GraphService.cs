/* 
*  Copyright (c) Microsoft. All rights reserved. Licensed under the MIT license. 
*  See LICENSE in the source repository root for complete license information. 
*/

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Resources;

namespace Microsoft_Teams_Graph_RESTAPIs_Connect.Models
{
    public class GraphService
    {
        private static string graphRootUri = ConfigurationManager.AppSettings["ida:GraphRootUri"];

        /// <summary>
        /// Get the current user's id from their profile.
        /// </summary>
        /// <param name="accessToken">Access token to validate user</param>
        /// <returns></returns>
        public async Task<string> GetMyId(string accessToken)
        {
            string endpoint = "https://graph.microsoft.com/v1.0/me";
            string queryParameter = "?$select=id";

            using (var client = new HttpClient())
            {
                using (var request = new HttpRequestMessage(HttpMethod.Get, endpoint + queryParameter))
                {
                    request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                    string userId = "";
                    using (HttpResponseMessage response = await client.SendAsync(request))
                    {
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            var json = JObject.Parse(await response.Content.ReadAsStringAsync());
                            userId = json.GetValue("id").ToString();
                        }
                        return userId?.Trim();
                    }
                }
            }
        }

        /// <summary>
        /// Get all teams which user is the member of.
        /// </summary>
        /// <param name="accessToken">Access token to validate user</param>
        /// <returns></returns>
        public async Task<List<ResultsItem>> GetMyTeams(string accessToken)
        {
            string endpoint = graphRootUri + "me/joinedTeams";
            string idPropertyName = "id";
            string displayPropertyName = "displayName";

            List<ResultsItem> items = new List<ResultsItem>();

            using (var client = new HttpClient())
            {
                using (var request = new HttpRequestMessage(HttpMethod.Get, endpoint))
                {
                    request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                    using (HttpResponseMessage response = await client.SendAsync(request))
                    {
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            items = await GetResultsItem(response, idPropertyName, displayPropertyName);
                            return items;
                        }

                        throw new Exception(response.ReasonPhrase);
                    }
                }
            }
        }

        /// <summary>
        /// Get all channels of the given.
        /// </summary>
        /// <param name="accessToken">Access token to validate user</param>
        /// <param name="teamId">Id of the team to get all associated channels</param>
        /// <returns></returns>
        public async Task<List<ResultsItem>> GetChannels(string accessToken, string teamId)
        {
            string endpoint = graphRootUri + "groups/" + teamId + "/channels";
            string idPropertyName = "id";
            string displayPropertyName = "displayName";

            List<ResultsItem> items = new List<ResultsItem>();

            using (var client = new HttpClient())
            {
                using (var request = new HttpRequestMessage(HttpMethod.Get, endpoint))
                {
                    request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                    using (HttpResponseMessage response = await client.SendAsync(request))
                    {
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            items = await GetResultsItem(response, idPropertyName, displayPropertyName);
                            return items;
                        }

                        throw new Exception(response.ReasonPhrase);
                    }
                }
            }
        }

        /// <summary>
        /// Create new channel.
        /// </summary>
        /// <param name="accessToken">Access token to validate user</param>
        /// <param name="teamId">Id of the team in which new channel needs to be created</param>
        /// <param name="channelName">New channel name</param>
        /// <param name="channelDescription">New channel description</param>
        /// <returns></returns>
        public async Task<string> CreateChannel(
            string accessToken, string teamId, string channelName, string channelDescription)
        {
            string endpoint = graphRootUri + "groups/" + teamId + "/channels";

            CreateChannel content = new CreateChannel()
            {
                description = channelDescription,
                displayName = channelName
            };

            using (var client = new HttpClient())
            {
                using (var request = new HttpRequestMessage(HttpMethod.Post, endpoint))
                {
                    request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                    request.Content = new StringContent(JsonConvert.SerializeObject(content), Encoding.UTF8, "application/json");

                    using (HttpResponseMessage response = await client.SendAsync(request))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            return Resource.TeamsGraph_CreateChannel_Success_Result;
                        }
                        return response.ReasonPhrase;
                    }
                }
            }
        }

        /// <summary>
        /// Start a new chat thread in channel
        /// </summary>
        /// <param name="accessToken">Access token to validate user</param>
        /// <param name="teamId">Id of the team in which chat thread needs to be started</param>
        /// <param name="channelId">Id of the channel associated with provided team Id in which chat thread needs to be started</param>
        /// <param name="message">Root message of the chat thread</param>
        /// <returns></returns>
        public async Task<string> PostMessage(string accessToken, string teamId, string channelId, string message)
        {
            string endpoint = graphRootUri + "groups/" + teamId + "/channels/" + channelId + "/chatThreads";

            PostMessage content = new PostMessage()
            {
                rootMessage = new RootMessage()
                {
                    body = new MessageBody()
                    {
                        content = message
                     }
                }
            };

            using (var client = new HttpClient())
            {
                using (var request = new HttpRequestMessage(HttpMethod.Post, endpoint))
                {
                    request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                    request.Content = new StringContent(JsonConvert.SerializeObject(content), Encoding.UTF8, "application/json");

                    using (HttpResponseMessage response = await client.SendAsync(request))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            return Resource.TeamsGraph_PostMessage_Success_Result;
                        }
                        return response.ReasonPhrase;
                    }
                }
            }
        }

        /// <summary>
        /// Helper function to prepare the ResultsItem list from request response.
        /// </summary>
        /// <param name="response">Request response</param>
        /// <param name="idPropertyName">Property name of the item Id</param>
        /// <param name="displayPropertyName">Property name of the item display name</param>
        /// <returns></returns>
        private async Task<List<ResultsItem>> GetResultsItem(
            HttpResponseMessage response, string idPropertyName, string displayPropertyName)
        {
            List<ResultsItem> items = new List<ResultsItem>();

            JObject json = JObject.Parse(await response.Content.ReadAsStringAsync());
            foreach (JProperty content in json.Children<JProperty>())
            {
                if (content.Name.Equals("value"))
                {
                    var res = content.Value.AsJEnumerable().GetEnumerator();
                    res.MoveNext();

                    while (res.Current != null)
                    {
                        string display = "";
                        string id = "";

                        foreach (JProperty prop in res.Current.Children<JProperty>())
                        {
                            if (prop.Name.Equals(idPropertyName))
                            {
                                id = prop.Value.ToString();
                            }

                            if (prop.Name.Equals(displayPropertyName))
                            {
                                display = prop.Value.ToString();
                            }
                        }

                        items.Add(new ResultsItem
                        {
                            Display = display,
                            Id = id,
                            Properties = new Dictionary<string, object>
                                            {
                                                { Resource.Prop_ID, id }
                                            }
                        });

                        res.MoveNext();
                    }
                }
            }

            return items;
        }
    }
}