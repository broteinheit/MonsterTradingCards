using MonsterTradingCards.Server.BattleLogic.Cards.CardType;
using MonsterTradingCards.Server.BattleLogic.Cards.ElementType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTradingCards.Server.Utility
{
    internal class StringToCardType
    {
        public static IElementType convertStringToElementType(string elementTypeString)
        {
            if (elementTypeString == "Regular")
            {
                elementTypeString = "Normal";
            }

            var elementType = typeof(IElementType).Assembly.GetTypes().Where(t => t.Name == elementTypeString + "ElementType").FirstOrDefault();
            return (IElementType)Activator.CreateInstance(elementType);
        }

        public static ICardType convertStringToCardType(string cardTypeString)
        {
            if (cardTypeString == "Spell")
            {
                return new SpellType();
            }

            var elementType = typeof(ICardType).Assembly.GetTypes().Where(t => t.Name == cardTypeString + "MonsterType").FirstOrDefault();
            return (ICardType)Activator.CreateInstance(elementType);
        }
    }
}
