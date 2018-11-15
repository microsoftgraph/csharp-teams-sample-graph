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
using System.Configuration;

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
                if (ConfigurationManager.AppSettings["ida:AppId"] == null
                    || ConfigurationManager.AppSettings["ida:AppSecret"] == null)
                {
                    return RedirectToAction("Index", "Error", new {
                        message = "You need to put your appid and appsecret in Web.config.secrets. See CSharp\\README.md for details."
                    });
                }

                // Get an access token.
                string accessToken = await AuthProvider.Instance.GetUserAccessTokenAsync();
                graphService.accessToken = accessToken;
                FormOutput output = await f(accessToken);

                output.Action = callerName.Replace("Form", "Action");

                output.UserUpn = await graphService.GetMyId(accessToken); // todo: cache

                if (output.ShowTeamDropdown)
                    output.Teams = (await graphService.GetMyTeams(accessToken)).ToArray();
                if (output.ShowGroupDropdown)
                    output.Groups = (await graphService.GetMyGroups(accessToken)).ToArray();

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
                    var teams = (await graphService.GetMyTeams(token)).ToArray();
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
                    var channels = (await graphService.GetChannels(token, data.SelectedTeam)).ToArray();
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
                    var apps = (await graphService.GetApps(token, data.SelectedTeam)).ToArray();
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
                    var channels = (await graphService.GetChannels(token, data.SelectedTeam)).ToArray();
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
                    Group group = await graphService.CreateNewTeamAndGroup(token, data.DisplayNameInput, data.MailNicknameInput, data.DescriptionInput);
                    var teams = (await graphService.GetMyTeams(token)).ToArray();
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



        [Authorize]
        public async Task<ActionResult> AddTeamToGroupForm()
        {
            return await WithExceptionHandling(
                token =>
                {
                    return new FormOutput()
                    {
                        ShowGroupDropdown = true,
                        ButtonLabel = "Create team",
                    };
                }
                );
        }

        [Authorize]
        public async Task<ActionResult> AddTeamToGroupAction(FormOutput data)
        {
            return await WithExceptionHandlingAsync(
                async token =>
                {
                    await graphService.AddTeamToGroup(data.SelectedGroup, token);
                    var teams = (await graphService.GetMyTeams(token)).ToArray();
                    return new FormOutput()
                    {
                        Teams = teams,
                        ShowTeamOutput = true
                    };
                }
                );
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
        public async Task<ActionResult> UpdateTeamForm()
        {
            return await WithExceptionHandling(
                token =>
                {
                    return new FormOutput()
                    {
                        ShowTeamDropdown = true,
                        ButtonLabel = "Change guest settings",
                    };
                }
                );
        }

        [Authorize]
        public async Task<ActionResult> UpdateTeamAction(FormOutput data)
        {
            return await WithExceptionHandlingAsync(
                async token =>
                {
                    await graphService.UpdateTeam(data.SelectedTeam, token);
                    return new FormOutput()
                    {
                        SuccessMessage = "Done",
                    };
                }
                );
        }

        [Authorize]
        public async Task<ActionResult> AddMemberForm()
        {
            return await WithExceptionHandling(
                token =>
                {
                    return new FormOutput()
                    {
                        ShowTeamDropdown = true,
                        ShowUpnInput = true,
                        ButtonLabel = "Add member",
                    };
                }
                );
        }

        [Authorize]
        public async Task<ActionResult> AddMemberAction(FormOutput data)
        {
            return await WithExceptionHandlingAsync(
                async token =>
                {
                    await graphService.AddMember(data.SelectedTeam, data.UpnInput, isOwner: false);
                    return new FormOutput()
                    {
                        SuccessMessage = "Done",
                    };
                }
                );
        }

        public ActionResult About()
        {
            return View();
        }
    }
}