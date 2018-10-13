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
                graphService.accessToken = accessToken;
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
        public async Task<ActionResult> PostChannelsForm()
        {
            return await WithExceptionHandling(
                token =>
                {
                    return new FormOutput()
                    {
                        ShowTeamDropdown = true,
                        ShowNameInput = true,
                        ShowDescriptionInput = true,
                        ButtonLabel = "Create channel",
                    };
                }
                );
        }

        [Authorize]
        public async Task<ActionResult> PostChannelsAction(FormOutput data)
        {
            return await WithExceptionHandlingAsync(
                async token =>
                {
                    await graphService.CreateChannel(token,
                        data.SelectedTeam, data.NameInput, data.DescriptionInput);
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
        public async Task<ActionResult> PostMessageForm()
        {
            return await WithExceptionHandling(
                token =>
                {
                    return new FormOutput()
                    {
                        ShowTeamDropdown = true,
                        ShowChannelDropdown = true,
                        ShowMessageBodyInput = true,
                        ButtonLabel = "Post Message",
                    };
                }
                );
        }

        [Authorize]
        public async Task<ActionResult> PostMessageAction(FormOutput data)
        {
            return await WithExceptionHandlingAsync(
                async token =>
                {
                    await graphService.PostMessage(token,
                        data.SelectedTeam, data.SelectedChannel, data.MessageBodyInput);
                    return new FormOutput()
                    {
                        SuccessMessage = "Done",
                    };
                }
                );
        }


        [Authorize]
        public async Task<ActionResult> PostGroupForm()
        {
            return await WithExceptionHandling(
                token =>
                {
                    return new FormOutput()
                    {
                        ShowDescriptionInput = true,
                        ShowDisplayNameInput = true,
                        ShowMailNicknameInput = true,
                        ButtonLabel = "Create team",
                    };
                }
                );
        }

        [Authorize]
        public async Task<ActionResult> PostGroupAction(FormOutput data)
        {
            return await WithExceptionHandlingAsync(
                async token =>
                {
                    Group group = await graphService.NewCreateNewTeamAndGroup(token, data.DisplayNameInput, data.MailNicknameInput, data.DescriptionInput);
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
        public async Task<ActionResult> Index()
        {
            return await WithExceptionHandling(
                token =>
                {
                    return new FormOutput()
                    {
                    };
                }
                );
        }


        //[Authorize]
        //public async Task<ActionResult> GetMyTeamsLoad()
        //{
        //    await GetMyId();

        //    ViewBag.GetMyTeamsLoad = "Enable";
        //    return View("Graph");
        //}

        //[Authorize]
        //public async Task<ActionResult> GetChannelsLoad()
        //{
        //    await GetMyId();
            
        //    ViewBag.GetChannelsLoad = "Enable";
        //    return View("Graph");
        //}

        //[Authorize]
        //public async Task<ActionResult> GetGroupLoad()
        //{
        //    await GetMyId();
        //    ViewBag.GetGroupLoad = "Enable";
        //    return View("Graph");
        //}

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


        [Authorize]
        public async Task<ActionResult> AddTeamToGroup()
        {
            try
            {
                string accessToken = await AuthProvider.Instance.GetUserAccessTokenAsync();
                string groupId = Request.Form["group-id"];

                await graphService.AddTeamToGroup(groupId, accessToken);
                ViewBag.CreateTeamMessage = "Successfully created/updated a team";

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

                await graphService.UpdateTeam(groupId, accessToken);
                ViewBag.CreateTeamMessage = "Successfully updated a team";

                return Content(ViewBag.CreateTeamMessage);
            }
            catch (Exception e)
            {
                if (e.Message == Resource.Error_AuthChallengeNeeded) return new EmptyResult();
                return RedirectToAction("Index", "Error", new { message = Resource.Error_Message + Request.RawUrl + ": " + e.Message });
            }
        }


        [Authorize]
        public async Task<String> AddMember(Member member)
        {
            try
            {
                string groupId = member.groupId;
                string accessToken = await AuthProvider.Instance.GetUserAccessTokenAsync();
                await graphService.AddMember(groupId, member.id, isOwner: false);
                return "Success";
            }
            catch (Exception e)
            {

                return e.Message;
            }
        }


        public ActionResult About()
        {
            return View();
        }
    }
}