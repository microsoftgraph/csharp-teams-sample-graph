using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphAPI.ICore
{
    public interface IAuthProvider
    {
        Task<String> GetUserAccessTokenAsync();
    }
}
