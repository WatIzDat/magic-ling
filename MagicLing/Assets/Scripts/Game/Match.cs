using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

public class Match
{
    private readonly List<Word> words = new();
    public ReadOnlyCollection<Word> Words => words.AsReadOnly();

    private Player player;

    private readonly List<Opponent> opponents = new();
    public ReadOnlyCollection<Opponent> Opponents => opponents.AsReadOnly();

    public event Action<int, Word> OnWordUpdated;

    public Match(Player player, List<Opponent> opponents)
    {
        this.player = player;
        this.opponents = opponents;

        foreach (Opponent battler in opponents)
        {
            battler.OnDeath += OnOpponentDeath;
        }

        words = player.Words;
    }

    private void OnOpponentDeath(Battler battler)
    {
        opponents.Remove((Opponent)battler);
    }

    public void UpdateWord(int index, Word word)
    {
        words[index] = word;

        OnWordUpdated?.Invoke(index, word);
    }

    public void EndTurn(List<Spell> playerSpells)
    {
        player.EndTurn();

        // clone opponents list to prevent modification during iteration
        foreach (Battler battler in opponents.ToList())
        {
            battler.EndTurn();
        }

        CastSpellsOnOpponents(playerSpells);

        foreach (Opponent opponent in opponents)
        {
            EnemyAction action = opponent.Behavior.GetNextAction();

            CastSpellOnPlayer(action.Spell, opponent);
        }
    }

    private void CastSpellsOnOpponents(List<Spell> spells)
    {
        // clone opponents list to prevent modification during iteration
        foreach (Battler opponent in opponents.ToList())
        {
            foreach (Spell spell in spells)
            {
                spell.CastSpell(opponent, player);
            }
        }
    }

    private void CastSpellOnPlayer(Spell spell, Opponent opponent)
    {
        spell.CastSpell(player, opponent);
    }
}
