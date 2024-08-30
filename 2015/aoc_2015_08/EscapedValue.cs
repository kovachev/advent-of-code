using System.Text.RegularExpressions;

namespace aoc_2015_08;

internal class EscapedValue
{
    public int Length => _value.Length;

    public int DecodedCharacters { get; private set; }

    public int DoubleEncodedCharacters { get; private set; }

    private readonly string _value;

    internal EscapedValue(string value)
    {
        _value = value;

        CountDecodedCharacters();
        CountDoubleEncodedCharacters();
    }

    private void CountDecodedCharacters()
    {
        var value = _value[1..^1];

        value = value.Replace("\\\"", "\"");

        value = value.Replace("\\\\", "\\");

        value = Regex.Replace(value, @"\\x[0-9a-f]{2}", "X", RegexOptions.Compiled); // just replace with a placeholder
                                                                                     // we only need to count the characters

        DecodedCharacters = value.Length;
    }

    private void CountDoubleEncodedCharacters()
    {
        var value = _value;

        value = Regex.Replace(value, @"\\x[0-9a-f]{2}", "XXXXX", RegexOptions.Compiled); // just replace with a placeholder
                                                                                         // we only need to count the characters

        value = value.Replace("\\", "\\\\");

        value = value.Replace("\"", "\\\"");

        DoubleEncodedCharacters = value.Length + 2; // +2 for the surrounding quotes
    }
}