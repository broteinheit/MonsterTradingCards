using MonsterTradingCards.Battle;
using MonsterTradingCards.Cards;
using MonsterTradingCards.Cards.CardType;
using MonsterTradingCards.Cards.ElementType;

public class Program
{
    public static int Main()
    {
        List<Card> deckP1 = new List<Card>()
        {
            new Card("a", 1, 15, new FireElementType(), new ElveMonsterType()),
            new Card("b", 1, 20, new WaterElementType(), new GoblinMonsterType()),
            new Card("c", 1, 25, new NormalElementType(), new KnightMonsterType()),
            new Card("d", 1, 20, new WaterElementType(), new SpellType())
        };

        List<Card> deckP2 = new List<Card>()
        {
            new Card("e", 1, 20, new WaterElementType(), new OrkMonsterType()),
            new Card("f", 1, 20, new FireElementType(), new DragonMonsterType()),
            new Card("g", 1, 20, new WaterElementType(), new WizardMonsterType()),
            new Card("h", 1, 20, new NormalElementType(), new SpellType())
        };

        BattleHandler battle = new BattleHandler(123, 456);
        battle.DisconnectPlayer(456);
        battle.ConnectPlayer(456);

        battle.cardsP1 = deckP1;
        battle.cardsP2 = deckP2;
        Console.WriteLine($"winner: {battle.StartBattle()}");

        return 0;
    }
}