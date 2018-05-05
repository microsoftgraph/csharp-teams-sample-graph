/* 
*  Copyright (c) Microsoft. All rights reserved. Licensed under the MIT license. 
*  See LICENSE in the source repository root for complete license information. 
*/

using Microsoft.Identity.Client;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OpenIdConnect;
using Microsoft_Teams_Graph_RESTAPIs_Connect.SessionToken;
using System;
using System.Configuration;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using Resources;
using Microsoft_Teams_Graph_RESTAPIs_Connect.ImportantFiles;

namespace Microsoft_Teams_Graph_RESTAPIs_Connect.Auth
{
    public sealed class AuthProvider : IAuthProvider
    {

        // Properties used to get and manage an access token.
        private string redirectUri = ServiceHelper.RedirectUri;
        private string appId = ServiceHelper.AppId;
        private string appSecret = ServiceHelper.AppSecret;
        private string scopes = ServiceHelper.Scopes;
        private SessionTokenCache tokenCache { get; set; }

        private static readonly AuthProvider instance = new AuthProvider();
        private AuthProvider() { }

        public static AuthProvider Instance
        {
            get
            {
                return instance;
            }
        }

        // Get an access token. First tries to get the token from the token cache.
        public async Task<string> GetUserAccessTokenAsync()
        {
            string signedInUserID = ClaimsPrincipal.Current.FindFirst(ClaimTypes.NameIdentifier).Value;
            tokenCache = new SessionTokenCache(
                signedInUserID,
                HttpContext.Current.GetOwinContext().Environment["System.Web.HttpContextBase"] as HttpContextBase);
            //var cachedItems = tokenCache.ReadItems(appId); // see what's in the cache

            ConfidentialClientApplication cca = new ConfidentialClientApplication(
                appId,
                redirectUri,
                new ClientCredential(appSecret),
                tokenCache);

            try
            {
                AuthenticationResult result = await cca.AcquireTokenSilentAsync(scopes.Split(new char[] { ' ' }));
                return result.Token;
            }

            // Unable to retrieve the access token silently.
            catch (MsalSilentTokenAcquisitionException)
            {
                HttpContext.Current.Request.GetOwinContext().Authentication.Challenge(
                    new AuthenticationProperties() { RedirectUri = "/" },
                    OpenIdConnectAuthenticationDefaults.AuthenticationType);

                throw new Exception(Resource.Error_AuthChallengeNeeded);
            }
        }

       
    }
}
