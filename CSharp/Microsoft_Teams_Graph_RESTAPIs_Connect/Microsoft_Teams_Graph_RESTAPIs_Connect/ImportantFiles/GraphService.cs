using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft_Teams_Graph_RESTAPIs_Connect.Models;
using Newtonsoft.Json;
using Resources;
using System.Configuration;

namespace Microsoft_Teams_Graph_RESTAPIs_Connect.ImportantFiles
{
    public static class Statics
    {
        public static T Deserialize<T>(this string result)
        {
            return JsonConvert.DeserializeObject<T>(result);
        }
    }

    public class GraphService
    {
        private static string GraphRootUri = ConfigurationManager.AppSettings["ida:GraphRootUri"];

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
            string endpoint = $"{GraphRootUri}/teams/{teamId}/channels";

            Channel content = new Channel()
            {
                description = channelDescription,
                displayName = channelName
            };

            HttpResponseMessage response = await ServiceHelper.SendRequest(HttpMethod.Post, endpoint, accessToken, content);

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
            string endpoint = $"{GraphRootUri}/teams/{teamId}/channels";

            List<ResultsItem> items = new List<ResultsItem>();
            HttpResponseMessage response = await ServiceHelper.SendRequest(HttpMethod.Get, endpoint, accessToken);
            if (response != null && response.IsSuccessStatusCode)
            {
                items = await ServiceHelper.GetResultsItem(response, "id", "displayName", resourcePropId);

            }
            return items;
        }

        public async Task<IEnumerable<Channel>> NewGetChannels(string accessToken, string teamId)
        {
            string endpoint = $"{GraphRootUri}/teams/{teamId}/channels";
            HttpResponseMessage response = await ServiceHelper.SendRequest(HttpMethod.Get, endpoint, accessToken);
            return await ParseList<Channel>(response);
        }

        private static async Task<IEnumerable<T>> ParseList<T>(HttpResponseMessage response)
        {
            if (response != null && response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                var t = JsonConvert.DeserializeObject<ResultList<T>>(content);
                return t.value;
            }
            return new T[0];
        }

        public async Task<IEnumerable<TeamsApp>> NewGetApps(string accessToken, string teamId)
        {
            string endpoint = $"{GraphRootUri}/teams/{teamId}/apps";
            HttpResponseMessage response = await ServiceHelper.SendRequest(HttpMethod.Get, endpoint, accessToken);
            return await ParseList<TeamsApp>(response);
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
            string endpoint = $"{GraphRootUri}/me/joinedTeams";

            List<ResultsItem> items = new List<ResultsItem>();
            HttpResponseMessage response = await ServiceHelper.SendRequest(HttpMethod.Get, endpoint, accessToken);
            if (response != null && response.IsSuccessStatusCode)
            {
                items = await ServiceHelper.GetResultsItem(response, "id", "displayName", resourcePropId);

            }
            return items;
        }

        public async Task<IEnumerable<Team>> NewGetMyTeams(string accessToken)
        {
            string endpoint = $"{GraphRootUri}/me/joinedTeams";

            HttpResponseMessage response = await ServiceHelper.SendRequest(HttpMethod.Get, endpoint, accessToken);
            if (response != null && response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                var t = JsonConvert.DeserializeObject<ResultList<Team>>(content);
                return t.value;
            }
            return new Team[0];
        }

        public async Task<HttpResponseMessage> PostMessage(string accessToken, string teamId, string channelId, string message)
        {
            string endpoint = $"{GraphRootUri}/teams/{teamId}/channels/{channelId}/chatThreads";

            PostMessage content = new PostMessage()
            {
                rootMessage = new RootMessage()
                {
                    body = new Message()
                    {
                        content = message
                    }
                }
            };
            List<ResultsItem> items = new List<ResultsItem>();
            HttpResponseMessage response = await ServiceHelper.SendRequest(HttpMethod.Post, endpoint, accessToken, content);

            return response;//response.ReasonPhrase;
        }

        public async Task<string> CreateNewTeamAndGroup(string accessToken, Group group)
        {
            // create group
            string endpoint = $"{GraphRootUri}/groups";
            if (group != null)
            {
                group.groupTypes = new string[] { "Unified" };
                group.mailEnabled = true;
                group.securityEnabled = false;
                group.visibility = "Private";
            }

            HttpResponseMessage response = await ServiceHelper.SendRequest(HttpMethod.Post, endpoint, accessToken, group);
            if (!response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();
            }

            string responseBody = await response.Content.ReadAsStringAsync(); ;
            string groupId = responseBody.Deserialize<Group>().id; // groupId is the same as teamId

            // add me as member
            string me = await GetMyId(accessToken);
            string payload = $"{{ '@odata.id': '{GraphRootUri}/users/{me}' }}";
            HttpResponseMessage responseRef = await ServiceHelper.SendRequest(HttpMethod.Post,
                $"{GraphRootUri}/groups/{groupId}/members/$ref",
                accessToken, payload);

            // create team
            await AddTeamToGroup(groupId, accessToken);
            return $"Created {groupId}";
        }

        public async Task<String> AddTeamToGroup(string groupId, string accessToken)
        {
            string endpoint = $"{GraphRootUri}/groups/{groupId}/team";
            Team team = new Models.Team();
            team.guestSettings = new Models.TeamGuestSettings() { allowCreateUpdateChannels = false, allowDeleteChannels = false };

            HttpResponseMessage response = await ServiceHelper.SendRequest(HttpMethod.Put, endpoint, accessToken, team);
            if (!response.IsSuccessStatusCode)
                throw new Exception(response.ReasonPhrase);
            return response.ReasonPhrase;
        }

        public async Task<String> UpdateTeam(string teamId, string accessToken)
        {
            string endpoint = $"{GraphRootUri}/teams/{teamId}";

            Team team = new Models.Team();
            team.guestSettings = new Models.TeamGuestSettings() { allowCreateUpdateChannels = true, allowDeleteChannels = false };

            HttpResponseMessage response = await ServiceHelper.SendRequest(new HttpMethod("PATCH"), endpoint, accessToken, team);
            if (!response.IsSuccessStatusCode)
                throw new Exception(response.ReasonPhrase);
            return response.ReasonPhrase;
        }

        public async Task AddMember(string teamId, Member member, string accessToken)
        {
            // If you have a user's UPN, you can add it directly to a group, but then there will be a 
            // significant delay before Microsoft Teams reflects the change. Instead, we find the user 
            // object's id, and add the ID to the group through the Graph beta endpoint, which is 
            // recognized by Microsoft Teams much more quickly. See 
            // https://developer.microsoft.com/en-us/graph/docs/api-reference/beta/resources/teams_api_overview 
            // for more about delays with adding members.

            // Step 1 -- Look up the user's id from their UPN
            string endpoint = $"{GraphRootUri}/users/{member.upn}";
            HttpResponseMessage response = await ServiceHelper.SendRequest(HttpMethod.Get, endpoint, accessToken);
            string responseBody = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
                throw new Exception(response.ReasonPhrase);

            String userId = responseBody.Deserialize<Member>().id;

            // Step 2 -- add that id to the group
            string payload = $"{{ '@odata.id': '{GraphRootUri}/users/{userId}' }}";
            endpoint = $"{GraphRootUri}/groups/{teamId}/members/$ref";

            HttpResponseMessage responseRef = await ServiceHelper.SendRequest(HttpMethod.Post, endpoint, accessToken, payload);
            if (!response.IsSuccessStatusCode)
                throw new Exception(response.ReasonPhrase);

            if (member.owner)
            {
                endpoint = $"{GraphRootUri}/groups/{teamId}/owners/$ref";
                HttpResponseMessage responseOwner = await ServiceHelper.SendRequest(HttpMethod.Post, endpoint, accessToken, payload);
                if (!response.IsSuccessStatusCode)
                    throw new Exception(response.ReasonPhrase);
            }
        }

        public async Task<IEnumerable<ResultsItem>> ListApps(string accessToken, string teamId, string resourcePropId)
        {
            HttpResponseMessage response = await ServiceHelper.SendRequest(
                HttpMethod.Get, 
                $"{GraphRootUri}/teams/{teamId}/apps",
                accessToken);
            string responseBody = await response.Content.ReadAsStringAsync();

            List<ResultsItem> items = new List<ResultsItem>();
            if (response != null && response.IsSuccessStatusCode)
            {
                items = await ServiceHelper.GetResultsItem(response, "id", "displayName", resourcePropId);
            }
            return items;
        }
    }
}