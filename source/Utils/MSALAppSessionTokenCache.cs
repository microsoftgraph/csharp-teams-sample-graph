/************************************************************************************************
The MIT License (MIT)

Copyright (c) 2015 Microsoft Corporation

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
***********************************************************************************************/

using Microsoft.Identity.Client;
using System;
using System.Threading;
using System.Web;

namespace Microsoft_Teams_Graph_RESTAPIs_Connect
{
    /// <summary>
    /// An implementation of token cache for Confidential clients backed by Http session.
    /// </summary>
    /// <seealso cref="https://github.com/AzureAD/microsoft-authentication-library-for-dotnet/wiki/token-cache-serialization"/>
    public class MSALAppSessionTokenCache
    {
        /// <summary>
        /// The application cache key
        /// </summary>
        internal readonly string AppCacheId;

        /// <summary>
        /// The HTTP context being used by this app
        /// </summary>
        internal HttpContextBase HttpContextInstance = null;

        /// <summary>
        /// The internal handle to the client's instance of the Cache
        /// </summary>
        private ITokenCache ApptokenCache;

        private static ReaderWriterLockSlim SessionLock = new ReaderWriterLockSlim(LockRecursionPolicy.NoRecursion);

        /// <summary>
        /// Initializes a new instance of the <see cref="MSALAppSessionTokenCache"/> class.
        /// </summary>
        /// <param name="tokenCache">The client's instance of the token cache.</param>
        /// <param name="clientId">The application's id (Client ID).</param>
        public MSALAppSessionTokenCache(ITokenCache tokenCache, string clientId, HttpContextBase httpcontext)
        {
            this.HttpContextInstance = httpcontext;
            this.AppCacheId = clientId + "_AppTokenCache";

            if (this.ApptokenCache == null)
            {
                this.ApptokenCache = tokenCache;
                this.ApptokenCache.SetBeforeAccess(this.AppTokenCacheBeforeAccessNotification);
                this.ApptokenCache.SetAfterAccess(this.AppTokenCacheAfterAccessNotification);
                this.ApptokenCache.SetBeforeWrite(this.AppTokenCacheBeforeWriteNotification);
            }

            this.LoadAppTokenCacheFromSession();
        }

        /// <summary>
        /// if you want to ensure that no concurrent write take place, use this notification to place a lock on the entry
        /// </summary>
        /// <param name="args">Contains parameters used by the MSAL call accessing the cache.</param>
        private void AppTokenCacheBeforeWriteNotification(TokenCacheNotificationArgs args)
        {
            // Since we are using a SessionCache ,whose methods are threads safe, we need not to do anything in this handler.
        }

        /// <summary>
        /// Loads the application's tokens from session cache.
        /// </summary>
        private void LoadAppTokenCacheFromSession()
        {
            SessionLock.EnterReadLock();

            this.ApptokenCache.DeserializeMsalV3((byte[])HttpContextInstance.Session[this.AppCacheId]);

            SessionLock.ExitReadLock();
        }

        /// <summary>
        /// Persists the application token's to session cache.
        /// </summary>
        private void PersistAppTokenCache()
        {
            SessionLock.EnterWriteLock();

            // Reflect changes in the persistence store
            HttpContextInstance.Session[this.AppCacheId] = this.ApptokenCache.SerializeMsalV3();

            SessionLock.ExitWriteLock();
        }

        /// <summary>
        /// Clears the TokenCache's copy of this user's cache.
        /// </summary>
        public void Clear()
        {
            SessionLock.EnterWriteLock();

            HttpContextInstance.Session[this.AppCacheId] = null;

            SessionLock.ExitWriteLock();

            // Nulls the currently deserialized instance
            this.LoadAppTokenCacheFromSession();
        }

        /// <summary>
        /// Triggered right before MSAL needs to access the cache. Reload the cache from the persistence store in case it changed since the last access.
        /// </summary>
        /// <param name="args">Contains parameters used by the MSAL call accessing the cache.</param>
        private void AppTokenCacheBeforeAccessNotification(TokenCacheNotificationArgs args)
        {
            this.LoadAppTokenCacheFromSession();
        }

        /// <summary>
        /// Triggered right after MSAL accessed the cache.
        /// </summary>
        /// <param name="args">Contains parameters used by the MSAL call accessing the cache.</param>
        private void AppTokenCacheAfterAccessNotification(TokenCacheNotificationArgs args)
        {
            // if the access operation resulted in a cache update
            if (args.HasStateChanged)
            {
                this.PersistAppTokenCache();
            }
        }
    }
}