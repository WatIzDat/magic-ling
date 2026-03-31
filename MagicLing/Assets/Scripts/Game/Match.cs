using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEditor.Rendering;
using UnityEngine;

public class Match
{
    private readonly List<Word> words = new();
    public ReadOnlyCollection<Word> Words => words.AsReadOnly();

    private Player player;

    private readonly List<Opponent> opponents = new();
    public ReadOnlyCollection<Opponent> Opponents => opponents.AsReadOnly();

    private readonly List<GameCard> hand = new();
    public ReadOnlyCollection<GameCard> Hand => hand.AsReadOnly();

    public event Action<int, Word> OnWordUpdated;
    public event Action<GameCard> OnCardDrawn;

    public Match(Player player, List<Opponent> opponents, List<GameCard> hand)
    {
        this.player = player;
        this.opponents = opponents;
        this.hand = hand;

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

    public void RemoveCardFromHand(GameCard card)
    {
        hand.Remove(card);
    }

    public void EndTurn(List<Spell> playerSpells, Opponent targetOpponent)
    {
        player.TickEffects();

        // clone opponents list to prevent modification during iteration
        foreach (Battler battler in opponents.ToList())
        {
            battler.TickEffects();
        }

        ApplyBlocksAndHeals(playerSpells, player);

        foreach (Opponent opponent in opponents)
        {
            EnemyAction action = opponent.Behavior.GetCurrentAction();

            ApplyBlocksAndHeals(action.Spells, opponent);
        }

        CastSpellsOnOpponent(playerSpells, targetOpponent);

        foreach (Opponent opponent in opponents)
        {
            EnemyAction action = opponent.Behavior.GetCurrentAction();

            CastSpellsOnPlayer(action.Spells, opponent);

            opponent.Behavior.NextAction();
        }

        for (int i = 0; i < words.Count; i++)
        {
            UpdateWord(i, new Word(words[i].Proto));
        }

        int cardsToDraw = RunInfo.MaxHandSize - hand.Count;

        for (int i = 0; i < cardsToDraw; i++)
        {
            var possibleCards = RunInfo.Cards.Where(card => !hand.Contains(card));

            GameCard card = possibleCards.ElementAt(UnityEngine.Random.Range(0, possibleCards.Count()));

            hand.Add(card);

            OnCardDrawn?.Invoke(card);
        }
    }

    private void CastSpellsOnOpponent(List<Spell> spells, Opponent opponent)
    {
        foreach (Spell spell in spells)
        {
            spell.CastSpell(opponent, player);
        }
    }

    private void CastSpellsOnPlayer(List<Spell> spells, Opponent opponent)
    {
        foreach (Spell spell in spells)
        {
            spell.CastSpell(player, opponent);
        }
    }

    private void ApplyBlocksAndHeals(List<Spell> spells, Battler battler)
    {
        foreach (Spell spell in spells)
        {
            battler.AddBlock(spell.Block);
            battler.Heal(spell.Heal);
        }
    }
}
