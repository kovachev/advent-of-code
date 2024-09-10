﻿namespace aoc.helpers;

public static class Combinations
{
    public static IEnumerable<List<T>> Subsets<T>(List<T>? objects, int maxLength)
    {
        if (objects == null || maxLength <= 0)
        {
            yield break;
        }

        var stack = new Stack<int>(maxLength);

        var i = 0;
        while (stack.Count > 0 || i < objects.Count)
        {
            if (i < objects.Count)
            {
                if (stack.Count == maxLength)
                {
                    i = stack.Pop() + 1;
                }

                stack.Push(i++);

                yield return (from index in stack.Reverse() select objects[index]).ToList();
            }
            else
            {
                i = stack.Pop() + 1;
                if (stack.Count > 0)
                {
                    i = stack.Pop() + 1;
                }
            }
        }
    }
}