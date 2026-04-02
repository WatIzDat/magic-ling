using System;
using System.Collections.Generic;

public static class ListUtil
{
    public static List<T> ComposeRandom<T>(int length, params Func<T>[] funcs)
    {
        List<T> list = new();

        for (int i = 0; i < length; i++)
        {
            Func<T> func = funcs[UnityEngine.Random.Range(0, funcs.Length)];
            
            list.Add(func());
        }

        return list;
    }
}
