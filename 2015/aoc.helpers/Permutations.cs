namespace aoc.helpers;

public static class Permutations
{
    public static IList<IList<T>> Permute<T>(T[] items)
    {
        var list = new List<IList<T>>();
        return DoPermute(items, 0, items.Length - 1, list);
    }

    private static IList<IList<T>> DoPermute<T>(T[] items, int start, int end, IList<IList<T>> list)
    {
        if (start == end)
        {
            // We have one of our possible n! solutions,
            // add it to the list.
            list.Add(new List<T>(items));
        }
        else
        {
            for (var i = start; i <= end; i++)
            {
                Swap(ref items[start], ref items[i]);
                DoPermute(items, start + 1, end, list);
                Swap(ref items[start], ref items[i]);
            }
        }

        return list;
    }

    private static void Swap<T>(ref T a, ref T b)
    {
        (a, b) = (b, a);
    }
}