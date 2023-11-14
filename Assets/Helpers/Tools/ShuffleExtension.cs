using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using Random = System.Random;


public static class ThreadSafeRandom
{
    [ThreadStatic] private static Random _local;

    public static Random ThisThreadsRandom
    {
        get { return _local ??= new Random(unchecked(Environment.TickCount * 31 + Thread.CurrentThread.ManagedThreadId)); }
    }
}

static class MyExtensions
{
    public static void Shuffle<T>(this IList<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = ThreadSafeRandom.ThisThreadsRandom.Next(n + 1);
            (list[k], list[n]) = (list[n], list[k]);
        }
    }
}


