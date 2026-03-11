using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

public class Match
{
    private readonly List<Word> words = new();
    public ReadOnlyCollection<Word> Words => words.AsReadOnly();

    public event Action<int, Word> OnWordUpdated;

    public Match(List<Word> words)
    {
        this.words = words;
    }

    public void UpdateWord(int index, Word word)
    {
        words[index] = word;

        OnWordUpdated?.Invoke(index, word);
    }
}
