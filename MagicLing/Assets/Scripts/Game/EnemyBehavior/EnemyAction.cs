using System.Collections.Generic;
using System.Linq;

public record EnemyAction
{
    public List<Spell> Spells { get; }
    public string Word { get; }

    public EnemyAction(List<Syllable> syllables)
    {
        Spells = new();

        int wordPos = 0;

        foreach (Syllable syllable in syllables)
        {
            Spell spell = Spell.CreateSpellOfSyllable(wordPos, syllable);

            Spells.Add(spell);

            wordPos += syllable.Full.Length;
        }

        Word = string.Join("", syllables.Select(s => s.Full));
    }
}
