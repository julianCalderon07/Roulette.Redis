﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Roulette.Redis.Common
{
    public class ResponseType
    {
        public int Code { get; set; }
        public string Message { get; set; }
        public string OutPut { get; set; }
    }
}
