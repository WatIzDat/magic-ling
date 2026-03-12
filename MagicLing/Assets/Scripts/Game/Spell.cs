using UnityEngine;

public record Spell
{
    public int StartIndex { get; }
    public int EndIndex { get; }
    public Color Color { get; }

    public Spell(int startIndex, int endIndex, Color color)
    {
        StartIndex = startIndex;
        EndIndex = endIndex;
        Color = color;
    }
}
