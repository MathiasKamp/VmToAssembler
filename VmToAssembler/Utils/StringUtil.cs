using System.Text.RegularExpressions;

namespace VmToAssembler.Utils;

public static class StringUtil
{
    private static readonly Regex OnlyCharacters = new(@"^[a-zA-Z]+$");

    private static readonly Regex OnlyNumbers = new(@"^-?[0-9][0-9,\.]+$");

    private static readonly Regex OnlyLowerCase = new(@"^[a-z]+$");

    public static bool IsOnlyCharacters(this string str)
    {
        return OnlyCharacters.IsMatch(str);
    }

    public static bool IsOnlyNumbers(this string str)
    {
        return OnlyNumbers.IsMatch(str);
    }

    public static bool IsOnlyLowerCase(this string str)
    {
        return OnlyLowerCase.IsMatch(str);
    }
    
    public static string[] SplitByWhitespace(this string str)
    {
        return str.Split(' ');
    }
}