using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Roulette.Redis.Common
{
    public class BetType
    {
        public string? color { get; set; }
        public string? number { get; set; }
        public string? valorBet { get; set; }
        public string idUser { get; set; }
        public string roulette { get; set; }
    }
}
