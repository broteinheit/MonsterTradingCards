using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTradingCards.Server.Managers
{
    public interface IDeckManager
    {
        Models.Deck GetDeckByUsername(string username);
        void SetDeckForUser(Models.Deck deck);
    }
}
