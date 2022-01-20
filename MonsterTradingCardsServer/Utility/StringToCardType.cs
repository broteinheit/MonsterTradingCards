using MonsterTradingCards.Server.BattleLogic.Cards.CardType;
using MonsterTradingCards.Server.BattleLogic.Cards.ElementType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MonsterTradingCards.Server.Utility
{
    public class StringToCardType
    {
        public static (IElementType, ICardType) GetCardAndElementTypeFromString(string cardName)
        {
            string[] nameSplit = Regex.Split(cardName, @"(?<!^)(?=[A-Z])");
            IElementType elemType = nameSplit.Length == 2 ? convertStringToElementType(nameSplit[0]) : new NormalElementType();
            ICardType cardType = nameSplit.Length == 2
                ? convertStringToCardType(nameSplit[1])
                : convertStringToCardType(nameSplit[0]);
            return (elemType, cardType);
        }

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
