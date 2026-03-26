using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

public class Match
{
    private readonly List<Word> words = new();
    public ReadOnlyCollection<Word> Words => words.AsReadOnly();

    private Player player;

    private readonly List<Battler> opponents = new();
    public ReadOnlyCollection<Battler> Battlers => opponents.AsReadOnly();

    public event Action<int, Word> OnWordUpdated;

    public Match(Player player, List<Battler> opponents)
    {
        this.player = player;
        this.opponents = opponents;

        words = player.Words;
    }

    public void UpdateWord(int index, Word word)
    {
        words[index] = word;

        OnWordUpdated?.Invoke(index, word);
    }

    public void EndTurn(List<Spell> playerSpells)
    {
        foreach (Battler battler in opponents)
        {
            battler.EndTurn();
        }

        //CastSpellsOnOpponents(playerSpells);
    }

    public void CastSpellsOnOpponents(List<Spell> spells)
    {
        foreach (Battler opponent in opponents)
        {
            foreach (Spell spell in spells)
            {
                opponent.TakeDamage(player, spell.Damage);
            }
        }
    }
}
