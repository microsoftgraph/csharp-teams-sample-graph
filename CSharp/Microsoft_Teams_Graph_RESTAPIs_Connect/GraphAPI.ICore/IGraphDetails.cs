using GraphAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace GraphAPI.ICore
{
    public interface IGraphDetails
    {
        Task<String> GetMyId(String accessToken);

        Task<IEnumerable<ResultsItem>> GetMyTeams(String accessToken, string resourcePropId);
        Task<IEnumerable<ResultsItem>> GetChannels(String accessToken, String teamId, string resourcePropId);

        Task<HttpResponseMessage> CreateChannel(String accessToken, String teamId, String channelName, String channelDescription);
        Task<HttpResponseMessage> PostMessage(String accessToken, String teamId, String channelId, String message);
    }
}
