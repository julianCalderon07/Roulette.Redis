using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Roulette.Redis.Services
{
    public interface IRouletteBL
    {
        Task<string> GetCacheValueAsync(string key);
        Task<bool>SetCacheValueAsync([System.Web.Http.FromBody] KeyValuePair<string, string> keyValue);
        Task<string> SetCreateRoulette([System.Web.Http.FromBody] KeyValuePair<string, string> keyValue);
    }
}
