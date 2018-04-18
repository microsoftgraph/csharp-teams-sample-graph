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
using GraphAPI.ICore;
using System.Net.Http;


namespace GraphAPI.Web.Controllers
{
    public class HomeController : Controller
    {
        readonly IGraphDetails graphService ;

        public HomeController()
        {
            graphService = new Core.GraphDetails();

        }
        public ActionResult Index()
        {
            return View("Graph");
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
                results.Items = await graphService.GetChannels(accessToken, Request.Form["team-id"], Resource.Prop_ID);

                // Reset the status to display when the page reloads.
                ViewBag.UserId = Request.Form["user-id"];
                ViewBag.TeamId = Request.Form["team-id"];
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
                    Request.Form["team-id"], Request.Form["channel-name"], Request.Form["channel-description"]);
                if (response != null && response.IsSuccessStatusCode)
                    ViewBag.CreateChannelMessage = Resource.TeamsGraph_CreateChannel_Success_Result;
                else
                    ViewBag.CreateChannelMessage = response.ReasonPhrase;

                // Reset the status to display when the page reloads.
                ViewBag.UserId = Request.Form["user-id"];
                ViewBag.TeamId = Request.Form["team-id"];
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
                    Request.Form["team-id"], Request.Form["channel-id"], Request.Form["message"]);

                if (response != null && response.IsSuccessStatusCode)
                    ViewBag.PostMessage = Resource.TeamsGraph_PostMessage_Success_Result;
                else
                    ViewBag.PostMessage = response.ReasonPhrase;

                // Reset the status to display when the page reloads.
                ViewBag.UserId = Request.Form["user-id"];
                ViewBag.TeamId = Request.Form["team-id"];
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

        public ActionResult About()
        {
            return View();
        }
    }
}