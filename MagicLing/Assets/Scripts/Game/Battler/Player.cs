using System.Collections.Generic;

public class Player : Battler
{
    public Player(List<Word> words, float health = 100f, float attack = 1f) : base(words, health, attack)
    {
    }
}
