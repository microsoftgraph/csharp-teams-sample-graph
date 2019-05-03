using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft_Teams_Graph_RESTAPIs_Connect
{
    /// <summary>
    /// An MSAL IAccount implementation
    /// </summary>
    /// <seealso cref="Microsoft.Identity.Client.IAccount" />
    internal class MSALAccount : IAccount
    {
        public string Username { get; set; }

        public string Environment { get; set; }

        public AccountId HomeAccountId { get; set; }

        internal MSALAccount()
        {
            this.HomeAccountId = new AccountId(string.Empty, string.Empty, string.Empty);
        }
        public MSALAccount(string identifier, string objectId, string tenantId)
        {
            this.HomeAccountId = new AccountId(identifier, objectId, tenantId);
        }

        public override bool Equals(Object obj)
        {
            //Check for null and compare run-time types.
            if ((obj == null) || !this.GetType().Equals(obj.GetType()))
            {
                return false;
            }
            else
            {
                MSALAccount other = obj as MSALAccount;

                return (this.Environment == other.Environment)
                    && (this.Username == other.Username)
                    && (this.HomeAccountId.Identifier == other.HomeAccountId.Identifier)
                    && (this.HomeAccountId.ObjectId == other.HomeAccountId.ObjectId)
                    && (this.HomeAccountId.TenantId == other.HomeAccountId.TenantId);
            }
        }

        /// <summary>
        /// Serves as a hash function for a particular type.
        /// </summary>
        /// <returns>
        /// A hash code for the current Address.
        /// </returns>
        public override int GetHashCode()
        {
            return (this.GetType().FullName + this.Username.ToString() + this.Environment.ToString()).GetHashCode();
        }
    }
}