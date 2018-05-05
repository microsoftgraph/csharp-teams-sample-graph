

using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft_Teams_Graph_RESTAPIs_Connect.Models;

namespace Microsoft_Teams_Graph_RESTAPIs_Connect.ImportantFiles
{
    public class GraphService
    {
        /// <summary>
        /// Create new channel.
        /// </summary>
        /// <param name="accessToken">Access token to validate user</param>
        /// <param name="teamId">Id of the team in which new channel needs to be created</param>
        /// <param name="channelName">New channel name</param>
        /// <param name="channelDescription">New channel description</param>
        /// <returns></returns>
        public async Task<HttpResponseMessage> CreateChannel(string accessToken, string teamId, string channelName, string channelDescription)
        {
            string endpoint = ServiceHelper.GraphRootUri + "groups/" + teamId + "/channels";

            Channel content = new Channel()
            {
                Description = channelDescription,
                Name = channelName
            };

            HttpResponseMessage response = await ServiceHelper.SendRequest(HttpMethod.Get, endpoint, accessToken, content);

            return response;//.ReasonPhrase;
        }

        /// <summary>
        /// Get all channels of the given.
        /// </summary>
        /// <param name="accessToken">Access token to validate user</param>
        /// <param name="teamId">Id of the team to get all associated channels</param>
        /// <returns></returns>
        public async Task<IEnumerable<ResultsItem>> GetChannels(string accessToken, string teamId, string resourcePropId)
        {
            string endpoint = ServiceHelper.GraphRootUri + "groups/" + teamId + "/channels";
            string idPropertyName = "id";
            string displayPropertyName = "displayName";

            List<ResultsItem> items = new List<ResultsItem>();
            HttpResponseMessage response = await ServiceHelper.SendRequest(HttpMethod.Get, endpoint, accessToken);
            if (response != null && response.IsSuccessStatusCode)
            {
                items = await ServiceHelper.GetResultsItem(response, idPropertyName, displayPropertyName, resourcePropId);

            }
            return items;
        }

        /// <summary>
        /// Get the current user's id from their profile.
        /// </summary>
        /// <param name="accessToken">Access token to validate user</param>
        /// <returns></returns>
        public async Task<string> GetMyId(String accessToken)
        {
            string endpoint = "https://graph.microsoft.com/v1.0/me";
            string queryParameter = "?$select=id";
            String userId = "";
            HttpResponseMessage response = await ServiceHelper.SendRequest(HttpMethod.Get, endpoint + queryParameter, accessToken);
            if (response != null && response.IsSuccessStatusCode)
            {
                var json = JObject.Parse(await response.Content.ReadAsStringAsync());
                userId = json.GetValue("id").ToString();
            }
            return userId?.Trim();

        }

        /// <summary>
        /// Get all teams which user is the member of.
        /// </summary>
        /// <param name="accessToken">Access token to validate user</param>
        /// <returns></returns>
        public async Task<IEnumerable<ResultsItem>> GetMyTeams(string accessToken, string resourcePropId)
        {

            string endpoint = ServiceHelper.GraphRootUri + "me/joinedTeams";
            string idPropertyName = "id";
            string displayPropertyName = "displayName";

            List<ResultsItem> items = new List<ResultsItem>();
            HttpResponseMessage response = await ServiceHelper.SendRequest(HttpMethod.Get, endpoint, accessToken);
            if (response != null && response.IsSuccessStatusCode)
            {
                items = await ServiceHelper.GetResultsItem(response, idPropertyName, displayPropertyName, resourcePropId);

            }
            return items;
        }

        public async Task<HttpResponseMessage> PostMessage(string accessToken, string teamId, string channelId, string message)
        {
            string endpoint = ServiceHelper.GraphRootUri + "groups/" + teamId + "/channels/" + channelId + "/chatThreads";

            PostMessage content = new PostMessage()
            {
                RootMessage = new RootMessage()
                {
                    Body = new Message()
                    {
                        Content = message
                    }
                }
            };
            List<ResultsItem> items = new List<ResultsItem>();
            HttpResponseMessage response = await ServiceHelper.SendRequest(HttpMethod.Get, endpoint, accessToken, content);

            return response;//response.ReasonPhrase;
        }
    }
}