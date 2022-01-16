using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTradingCards.Server.BattleLogic.Cards.ElementType
{
    interface IElementType : IType
    {
        //returns a damage multiplier based on the element type comparison
        public double Compare(IElementType otherElementType);
    }
}
