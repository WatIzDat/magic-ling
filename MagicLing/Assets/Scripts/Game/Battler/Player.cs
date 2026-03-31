using System.Collections.Generic;

public class Player : Battler
{
    //public List<GameCard> Cards { get; } = new() {
    //    new RuleCard("p", "f"),
    //    new RuleCard("t", "th"),
    //    new RuleCard("a", "e"),
    //    new RuleCard("e", "a"),
    //    new RuleCard("r", "f"),
    //    new RuleCard("t", "l"), 
    //};

    //public int MaxHandSize { get; private set; } = 6;
    public List<Word> Words { get; protected set; }

    public Player(List<Word> words, float health = 20f, float attack = 1f) : base(health, attack)
    {
        Words = words;
    }
}
