using MonsterTradingCards.Battle;
using MonsterTradingCards.Card;
using MonsterTradingCards.Card.CardType;
using MonsterTradingCards.Card.ElementType;

public class Program
{
    public static int Main()
    {
        BattleLogic battleLogic = new BattleLogic();

        Card monsterCard = new Card("a", 1, 20, new FireElementType(), new ElveMonsterType());
        Card spellCard = new Card("b", 1, 25, new WaterElementType(), new DragonMonsterType());

        Console.WriteLine("monsterCard: " + monsterCard.GetCardName());
        Console.WriteLine("spellCard: " + spellCard.GetCardName());

        Console.WriteLine("battleLogic: " + battleLogic.RunRound(monsterCard, spellCard));

        return 0;
    }
}