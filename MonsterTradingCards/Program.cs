using MonsterTradingCards.Card;
using MonsterTradingCards.Card.CardType;
using MonsterTradingCards.Card.ElementType;

public class Program
{
    public static int Main()
    {
        Card monsterCard = new Card("a", 1, 20, new WaterElementType(), new GoblinMonsterType());
        Card spellCard = new Card("b", 1, 25, new WaterElementType(), new SpellType());

        Console.WriteLine("monsterCard: " + monsterCard.GetCardName());
        Console.WriteLine("spellCard: " + spellCard.GetCardName());

        return 0;
    }
}