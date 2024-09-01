namespace aoc.helpers;

public static class Permutations
{
    public static IList<IList<string>> Permute(string[] cities)
    {
        var list = new List<IList<string>>();
        return DoPermute(cities, 0, cities.Length - 1, list);
    }

    private static IList<IList<string>> DoPermute(string[] cities, int start, int end, IList<IList<string>> list)
    {
        if (start == end)
        {
            // We have one of our possible n! solutions,
            // add it to the list.
            list.Add(new List<string>(cities));
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

    private static void Swap(ref string a, ref string b)
    {
        (a, b) = (b, a);
    }
}