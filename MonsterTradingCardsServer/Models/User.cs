using MonsterTradingCards.Server.Core.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTradingCards.Server.Models
{
    public class User : IIdentity
    {
        public string Username { get; set; }
        public string Password { get; set; }

        public string Token => $"{Username}-mtcgToken";

        public int Elo { get; set; }
        public int Gold { get; set; }
    }
}
