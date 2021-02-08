using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Roulette.Redis.Common
{
    public class WinnersType
    {
        public string idUser { get; set; }
        public string valorBet { get; set; }
        public string valorWinner { get; set; }
        public string color { get; set; }
        public string number { get; set; }
        public string colorWinner { get; set; }
        public string numberWinner { get; set; }
        public string state { get; set; }


    }
}
