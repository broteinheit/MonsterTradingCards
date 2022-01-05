using MonsterTradingCards.Card;

public class Program
{
    public static int Main()
    {
        MonsterCard monsterCard = new MonsterCard("a", "WaterGoblin", 1, 20, ElementType.WATER, MonsterType.GOBLIN);
        SpellCard spellCard = new SpellCard("b", "WaterSpell", 1, 25, ElementType.WATER);

        return 0;
    }
}