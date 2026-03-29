using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

public class Match
{
    private readonly List<Word> words = new();
    public ReadOnlyCollection<Word> Words => words.AsReadOnly();

    private Player player;

    private readonly List<Battler> opponents = new();
    public ReadOnlyCollection<Battler> Opponents => opponents.AsReadOnly();

    public event Action<int, Word> OnWordUpdated;

    public Match(Player player, List<Battler> opponents)
    {
        this.player = player;
        this.opponents = opponents;

        foreach (Battler battler in opponents)
        {
            battler.OnDeath += OnOpponentDeath;
        }

        words = player.Words;
    }

    private void OnOpponentDeath(Battler battler)
    {
        opponents.Remove(battler);
    }

    public void UpdateWord(int index, Word word)
    {
        words[index] = word;

        OnWordUpdated?.Invoke(index, word);
    }

    public void EndTurn(List<Spell> playerSpells)
    {
        // clone opponents list to prevent modification during iteration
        foreach (Battler battler in opponents.ToList())
        {
            battler.EndTurn();
        }

        CastSpellsOnOpponents(playerSpells);
    }

    public void CastSpellsOnOpponents(List<Spell> spells)
    {
        // clone opponents list to prevent modification during iteration
        foreach (Battler opponent in opponents.ToList())
        {
            foreach (Spell spell in spells)
            {
                opponent.AddEffect(new Effect(spell.Effect));
                opponent.TakeDamage(player, spell.Damage);
            }
        }
    }
}
