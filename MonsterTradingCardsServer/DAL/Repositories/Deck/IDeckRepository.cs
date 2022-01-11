using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonsterTradingCards.Server.Models;

namespace MonsterTradingCards.Server.DAL.Repositories.Deck
{
    public interface IDeckRepository
    {
        bool SetDeck(Models.Deck deck);
        Models.Deck GetDeck(string username);
    }
}
