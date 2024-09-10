namespace aoc.helpers;

public static class Permutations
{
    public static IList<IList<T>> Permute<T>(T[] cities)
    {
        var list = new List<IList<T>>();
        return DoPermute(cities, 0, cities.Length - 1, list);
    }

    private static IList<IList<T>> DoPermute<T>(T[] cities, int start, int end, IList<IList<T>> list)
    {
        if (start == end)
        {
            // We have one of our possible n! solutions,
            // add it to the list.
            list.Add(new List<T>(cities));
        }
        else
        {
            for (var i = start; i <= end; i++)
            {
                Swap(ref cities[start], ref cities[i]);
                DoPermute(cities, start + 1, end, list);
                Swap(ref cities[start], ref cities[i]);
            }
        }

        return list;
    }

    private static void Swap<T>(ref T a, ref T b)
    {
        (a, b) = (b, a);
    }
}