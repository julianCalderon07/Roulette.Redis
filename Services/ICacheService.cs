using Roulette.Redis.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Roulette.Redis.Services
{
    public interface ICacheService
    {
        Task<string> SetListCacheAsync();
        Task<string> SetActivateRouletteCacheAsync(string roulette);
        Task<string> SetBetCacheAsync(BetType bet);
        Task<string> SetCloseRouletteCacheAsync(String roulette);
    }
}
;