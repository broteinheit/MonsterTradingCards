using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTradingCardsServer.Core.Response
{
    public class Response
    {
        public StatusCode StatusCode { get; set; }
        public string Payload { get; set; }
    }
}
