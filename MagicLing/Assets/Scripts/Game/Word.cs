public record Word
{
    public string Proto { get; }
    public string Current { get; }

    public Word(string protoWord)
    {
        Proto = protoWord;
        Current = protoWord;
    }

    public Word(string protoWord, string currentWord)
    {
        Proto = protoWord;
        Current = currentWord;
    }
}
