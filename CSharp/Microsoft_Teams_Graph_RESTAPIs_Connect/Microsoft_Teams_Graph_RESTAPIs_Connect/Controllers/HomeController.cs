/* 
 *  Copyright (c) Microsoft. All rights reserved. Licensed under the MIT license. 
 *  See LICENSE in the source repository root for complete license information. 
 */

using System.Threading.Tasks;
using System.Web.Mvc;
using Microsoft_Teams_Graph_RESTAPIs_Connect.Auth;
using Microsoft_Teams_Graph_RESTAPIs_Connect.Models;
using Resources;
using System;

using System.Net.Http;
using Microsoft_Teams_Graph_RESTAPIs_Connect.ImportantFiles;
using System.Linq;
using System.Runtime.CompilerServices;

namespace GraphAPI.Web.Controllers
{
    public class HomeController : Controller
    {
        public static bool hasAppId = ServiceHelper.AppId != "Enter AppId of your application";

        readonly GraphService graphService ;

        public HomeController()
        {
            graphService = new GraphService();

        }

        private async Task<ActionResult> WithExceptionHandling(Func<string, FormOutput> f, [CallerMemberName] string callerName = "")
        {
            return await WithExceptionHandlingAsync(
                async s => f(s),
                callerName);
        }

        private async Task<ActionResult> WithExceptionHandlingAsync(Func<string, Task<FormOutput>> f, [CallerMemberName] string callerName = "")
        {
            try
            {
                // Get an access token.
                string accessToken = await AuthProvider.Instance.GetUserAccessTokenAsync();
                FormOutput output = await f(accessToken);

                output.Action = callerName.Replace("Form", "Action");

                output.UserUpn = await graphService.GetMyId(accessToken); // todo: cache

                if (output.ShowTeamDropdown)
                    output.Teams = (await graphService.NewGetMyTeams(accessToken)).ToArray();

                //results.Items = await graphService.GetMyTeams(accessToken, Convert.ToString(Resource.Prop_ID));
                return View("Graph", output);
            }
            catch (Exception e)
            {
                if (e.Message == Resource.Error_AuthChallengeNeeded) return new EmptyResult();
                return RedirectToAction("Index", "Error", new { message = Resource.Error_Message + Request.RawUrl + ": " + e.Message });
            }

        }

        [Authorize]
        public async Task<ActionResult> GetTeamsForm()
        {
            return await WithExceptionHandling(
                token =>
                {
                    return new FormOutput()
                    {
                        ShowTeamDropdown=true
                    };
                }
                );
        }

        [Authorize]
        public async Task<ActionResult> GetTeamsAction(FormOutput data)
        {
            return await WithExceptionHandlingAsync(
                async token =>
                {
                    var teams = (await graphService.NewGetMyTeams(token)).ToArray();
                    return new FormOutput()
                    {
                        Teams = teams,
                        ShowTeamOutput = true
                    };
                }
                );
        }

        [Authorize]
        public async Task<ActionResult> GetChannelsForm()
        {
            return await WithExceptionHandling(
                token =>
                {
                    return new FormOutput()
                    {
                        ShowTeamDropdown = true,
                        ButtonLabel="Get channels",
                    };
                }
                );
        }

        [Authorize]
        public async Task<ActionResult> GetChannelsAction(FormOutput data)
        {
            return await WithExceptionHandlingAsync(
                async token =>
                {
                    var channels = (await graphService.NewGetChannels(token, data.SelectedTeam)).ToArray();
                    return new FormOutput()
                    {
                        Channels = channels,
                        ShowChannelOutput = true
                    };
                }
                );
        }

        [Authorize]
        public async Task<ActionResult> GetAppsForm()
        {
            return await WithExceptionHandling(
                token =>
                {
                    return new FormOutput()
                    {
                        ShowTeamDropdown = true,
                        ButtonLabel = "Get Apps",
                    };
                }
                );
        }

        [Authorize]
        public async Task<ActionResult> GetAppsAction(FormOutput data)
        {
            return await WithExceptionHandlingAsync(
                async token =>
                {
                    var apps = (await graphService.NewGetApps(token, data.SelectedTeam)).ToArray();
                    return new FormOutput()
                    {
                        Apps = apps,
                        ShowAppOutput = true
                    };
                }
                );
        }






        [Authorize]
        public async Task<ActionResult> Index()
        {
            return await WithExceptionHandlingAsync(
                async token =>
                {
                    return new FormOutput()
                    {
                    };
                }
                );
        }


        [Authorize]
        public async Task<ActionResult> GetMyTeamsLoad()
        {
            await GetMyId();

            ViewBag.GetMyTeamsLoad = "Enable";
            return View("Graph");
        }

        [Authorize]
        public async Task<ActionResult> GetChannelsLoad()
        {
            await GetMyId();
            
            ViewBag.GetChannelsLoad = "Enable";
            return View("Graph");
        }

        [Authorize]
        public async Task<ActionResult> GetGroupLoad()
        {
            await GetMyId();
            ViewBag.GetGroupLoad = "Enable";
            return View("Graph");
        }

        [Authorize]
        public async Task<ActionResult> GetAddTeamToGroupLoad()
        {
            await GetMyId();
            ViewBag.GetAddTeamToGroupLoad = "Enable";
            return View("Graph");
        }

        [Authorize]
        public async Task<ActionResult> GetTeamLoadUpdate()
        {
            await GetMyId();
            ViewBag.GetTeamLoadUpdate = "Enable";
            return View("Graph");
        }


        // [Authorize]
        public async Task<ActionResult> GetMemberLoad()
        {
            await GetMyId();
            ViewBag.GetMemberLoad = "Enable";
            return View("Graph");
        }


        [Authorize]
        public async Task<ActionResult> CreateChannelLoad()
        {
            await GetMyId();

            ViewBag.CreateChannelLoad = "Enable";
            return View("Graph");
        }

        [Authorize]
        public async Task<ActionResult> PostMessageLoad()
        {
            await GetMyId();

            ViewBag.PostMessageLoad = "Enable";
            return View("Graph");
        }

        [Authorize]
        public async Task<ActionResult> GetAppsLoad()
        {
            await GetMyId();

            ViewBag.GetAppsLoad = "Enable";
            return View("Graph");
        }


        
        /// <summary>
        /// Get the current user's id from their profile.
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public async Task<ActionResult> GetMyId()
        {
            try
            {
                // Get an access token.
                string accessToken = await AuthProvider.Instance.GetUserAccessTokenAsync();

                // Get the current user's id.
                ViewBag.UserId = await graphService.GetMyId(accessToken);
                return View("Graph");
            }
            catch (Exception e)
            {
                if (e.Message == Resource.Error_AuthChallengeNeeded) return new EmptyResult();
                return RedirectToAction("Index", "Error", new { message = Resource.Error_Message + Request.RawUrl + ": " + e.Message });
            }
        }

        /// <summary>
        /// Get all teams which user is the member of.
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public async Task<ActionResult> GetMyTeams()
        {
            ResultsViewModel results = new ResultsViewModel();

            try
            {
                string accessToken = await AuthProvider.Instance.GetUserAccessTokenAsync();
                results.Items = await graphService.GetMyTeams(accessToken, Convert.ToString(Resource.Prop_ID));

                // Reset the status to display when the page reloads.
                ViewBag.UserId = Request.Form["user-id"];
                ViewBag.GetMyTeamsLoad = "Enable";
                ViewBag.GetMyTeamsResult = "Enable";

                return View("Graph", results);
            }
            catch (Exception e)
            {
                if (e.Message == Resource.Error_AuthChallengeNeeded) return new EmptyResult();
                return RedirectToAction("Index", "Error", new { message = Resource.Error_Message + Request.RawUrl + ": " + e.Message });
            }
        }

        /// <summary>
        /// Get all channels of the given.
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public async Task<ActionResult> GetChannels()
        {
            ResultsViewModel results = new ResultsViewModel();

            try
            {
                string accessToken = await AuthProvider.Instance.GetUserAccessTokenAsync();
                results.Items = await graphService.GetChannels(accessToken, Request.Form["group-id"], Resource.Prop_ID);

                // Reset the status to display when the page reloads.
                ViewBag.UserId = Request.Form["user-id"];
                ViewBag.GroupId = Request.Form["group-id"];
                ViewBag.GetChannelsLoad = "Enable";
                ViewBag.GetChannelsResult = "Enable";

                return View("Graph", results);
            }
            catch (Exception e)
            {
                if (e.Message == Resource.Error_AuthChallengeNeeded) return new EmptyResult();
                return RedirectToAction("Index", "Error", new { message = Resource.Error_Message + Request.RawUrl + ": " + e.Message });
            }
        }

        /// <summary>
        /// Create new channel.
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public async Task<ActionResult> CreateChannel()
        {
            try
            {
                string accessToken = await AuthProvider.Instance.GetUserAccessTokenAsync();
                HttpResponseMessage response = await graphService.CreateChannel(accessToken,
                    Request.Form["group-id"], Request.Form["channel-name"], Request.Form["channel-description"]);
                if (response != null && response.IsSuccessStatusCode)
                    ViewBag.CreateChannelMessage = Resource.TeamsGraph_CreateGroup_Success_Result;
                else
                    ViewBag.CreateChannelMessage = response.ReasonPhrase;

                // Reset the status to display when the page reloads.
                ViewBag.UserId = Request.Form["user-id"];
                ViewBag.GroupId = Request.Form["group-id"];
                ViewBag.ChannelName = Request.Form["channel-name"];
                ViewBag.ChannelDescription = Request.Form["channel-description"];
                ViewBag.CreateChannelLoad = "Enable";

                return View("Graph");
            }
            catch (Exception e)
            {
                if (e.Message == Resource.Error_AuthChallengeNeeded) return new EmptyResult();
                return RedirectToAction("Index", "Error", new { message = Resource.Error_Message + Request.RawUrl + ": " + e.Message });
            }
        }

        [Authorize]
        public async Task<ActionResult> AddTeamToGroup()
        {
            try
            {
                string accessToken = await AuthProvider.Instance.GetUserAccessTokenAsync();
                string groupId = Request.Form["group-id"];

                String response = await graphService.AddTeamToGroup(groupId, accessToken);
                    
                if (response != null )
                    ViewBag.CreateTeamMessage = "Successfully created/updated a team";
                else
                    ViewBag.CreateTeamMessage = "Fail";

                return Content(ViewBag.CreateTeamMessage);
            }
            catch (Exception e)
            {
                if (e.Message == Resource.Error_AuthChallengeNeeded) return new EmptyResult();
                return RedirectToAction("Index", "Error", new { message = Resource.Error_Message + Request.RawUrl + ": " + e.Message });
            }
        }

        public async Task<ActionResult> UpdateTeam()
        {
            try
            {
                string accessToken = await AuthProvider.Instance.GetUserAccessTokenAsync();
                string groupId = Request.Form["group-id"];

                String response = await graphService.UpdateTeam(groupId, accessToken);

                if (response != null)
                    ViewBag.CreateTeamMessage = "Successfully updated a team";
                else
                    ViewBag.CreateTeamMessage = "Fail";

                return Content(ViewBag.CreateTeamMessage);
            }
            catch (Exception e)
            {
                if (e.Message == Resource.Error_AuthChallengeNeeded) return new EmptyResult();
                return RedirectToAction("Index", "Error", new { message = Resource.Error_Message + Request.RawUrl + ": " + e.Message });
            }
        }


        /// <summary>
        /// Start a new chat thread in channel
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public async Task<ActionResult> PostMessage()
        {
            try
            {
                string accessToken = await AuthProvider.Instance.GetUserAccessTokenAsync();
                HttpResponseMessage response = await graphService.PostMessage(accessToken,
                    Request.Form["group-id"], Request.Form["channel-id"], Request.Form["message"]);

                if (response != null && response.IsSuccessStatusCode)
                    ViewBag.PostMessage = Resource.TeamsGraph_PostMessage_Success_Result;
                else
                    ViewBag.PostMessage = response.ReasonPhrase;

                // Reset the status to display when the page reloads.
                ViewBag.UserId = Request.Form["user-id"];
                ViewBag.groupId = Request.Form["group-id"];
                ViewBag.ChannelId = Request.Form["channel-id"];
                ViewBag.PostMessageLoad = "Enable";

                return View("Graph");
            }
            catch (Exception e)
            {
                if (e.Message == Resource.Error_AuthChallengeNeeded) return new EmptyResult();
                return RedirectToAction("Index", "Error", new { message = Resource.Error_Message + Request.RawUrl + ": " + e.Message });
            }
        }

        [Authorize]
        public async Task<ActionResult> Foo(FormOutput data)
        {
            return View("Graph");
        }

        [Authorize]
        public async Task<String> CreateNewTeamAndGroup(Group group)
        {
            try
            {
                string accessToken = await AuthProvider.Instance.GetUserAccessTokenAsync();
                String message = await graphService.CreateNewTeamAndGroup(accessToken, group);
                return message;
            }
            catch (Exception e)
            {
                if (e.Message == Resource.Error_AuthChallengeNeeded)
                    return "Fail";
                return e.Message;
            }
        }

        [Authorize]
        public async Task<String> AddMember(Member member)
        {
            try
            {
                string groupId = member.groupId;
                string accessToken = await AuthProvider.Instance.GetUserAccessTokenAsync();
                await graphService.AddMember(groupId, member, accessToken);
                return "Success";
            }
            catch (Exception e)
            {

                return e.Message;
            }
        }


        public async Task<ActionResult> ListApps()
        {
            ResultsViewModel results = new ResultsViewModel();

            try
            {
                string accessToken = await AuthProvider.Instance.GetUserAccessTokenAsync();
                results.Items = await graphService.ListApps(
                    accessToken, Request.Form["teamId"], Resource.Prop_ID);

                // Reset the status to display when the page reloads.
                ViewBag.UserId = Request.Form["user-id"];
                ViewBag.GroupId = Request.Form["group-id"];
                ViewBag.GetAppsLoad = "Enable";
                ViewBag.GetAppsLoadResult = "Enable";

                return View("Graph", results);
            }
            catch (Exception e)
            {
                if (e.Message == Resource.Error_AuthChallengeNeeded) return new EmptyResult();
                return RedirectToAction("Index", "Error", new { message = Resource.Error_Message + Request.RawUrl + ": " + e.Message });
            }
        }

        public ActionResult About()
        {
            return View();
        }
    }
}