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

    public class GraphService : HttpHelpers
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
        public async Task CreateChannel(string accessToken, string teamId, string channelName, string channelDescription)
        {
            await HttpPost($"/teams/{teamId}/channels",
                new Channel()
                {
                    description = channelDescription,
                    displayName = channelName
                });
        }

        public async Task<IEnumerable<Channel>> GetChannels(string accessToken, string teamId)
        {
            string endpoint = $"{GraphRootUri}/teams/{teamId}/channels";
            HttpResponseMessage response = await ServiceHelper.SendRequest(HttpMethod.Get, endpoint, accessToken);
            return await ParseList<Channel>(response);
        }

        public async Task<IEnumerable<TeamsApp>> GetApps(string accessToken, string teamId)
        {
            // to do: switch to the V1 installedApps API
            return await HttpGetList<TeamsApp>($"/teams/{teamId}/apps", endpoint: graphBetaEndpoint);
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

        public async Task<IEnumerable<Team>> GetMyTeams(string accessToken)
        {
            return await HttpGetList<Team>($"/me/joinedTeams");
        }

        public async Task<IEnumerable<Group>> GetMyGroups(string accessToken)
        {
            return await HttpGetList<Group>($"/me/joinedGroups", endpoint: graphBetaEndpoint);
        }

        public async Task PostMessage(string accessToken, string teamId, string channelId, string message)
        {
            await HttpPost($"/teams/{teamId}/channels/{channelId}/chatThreads",
                new PostMessage()
                {
                    rootMessage = new RootMessage()
                    {
                        body = new Message()
                        {
                            content = message
                        }
                    }
                },
                endpoint: graphBetaEndpoint);
        }

        public async Task<Group> CreateNewTeamAndGroup(string accessToken, String displayName, String mailNickname, String description)
        {
            // create group
            Group groupParams = new Group()
            {
                displayName = displayName,
                mailNickname = mailNickname,
                description = description,

                groupTypes = new string[] { "Unified" },
                mailEnabled = true,
                securityEnabled = false,
                visibility = "Private",
            };

            Group createdGroup = (await HttpPost($"/groups", groupParams))
                            .Deserialize<Group>();
            string groupId = createdGroup.id;

            // add me as member
            string me = await GetMyId(accessToken);
            string payload = $"{{ '@odata.id': '{GraphRootUri}/users/{me}' }}";
            HttpResponseMessage responseRef = await ServiceHelper.SendRequest(HttpMethod.Post,
                $"{GraphRootUri}/groups/{groupId}/members/$ref",
                accessToken, payload);

            // create team
            await AddTeamToGroup(groupId, accessToken);
            return createdGroup;
        }

        public async Task AddTeamToGroup(string groupId, string accessToken)
        {
            await HttpPut($"/groups/{groupId}/team",
                new Team()
                {
                    guestSettings = new TeamGuestSettings()
                    {
                        allowCreateUpdateChannels = false,
                        allowDeleteChannels = false
                    }
                });
        }

        public async Task UpdateTeam(string teamId, string accessToken)
        {
            await HttpPatch($"/teams/{teamId}",
                new Team()
                {
                    guestSettings = new TeamGuestSettings() { allowCreateUpdateChannels = true, allowDeleteChannels = false }
                });
        }

        public async Task AddMember(string teamId, string upn, bool isOwner = false)
        {
            // If you have a user's UPN, you can add it directly to a group, but then there will be a 
            // significant delay before Microsoft Teams reflects the change. Instead, we find the user 
            // object's id, and add the ID to the group through the Graph beta endpoint, which is 
            // recognized by Microsoft Teams much more quickly. See 
            // https://developer.microsoft.com/en-us/graph/docs/api-reference/beta/resources/teams_api_overview 
            // for more about delays with adding members.

            // Step 1 -- Look up the user's id from their UPN
            String userId = (await HttpGet<User>($"/users/{upn}")).id;

            // Step 2 -- add that id to the group
            string payload = $"{{ '@odata.id': '{graphBetaEndpoint}/users/{userId}' }}";
            await HttpPost($"/groups/{teamId}/members/$ref", payload);

            if (isOwner)
                await HttpPost($"/groups/{teamId}/owners/$ref", payload);
        }
    }
}