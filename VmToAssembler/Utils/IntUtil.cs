using System;
using System.Text.RegularExpressions;

namespace VmToAssembler.Utils;

public static class IntUtil
{
    private static readonly Regex OnlyNumbers = new (@"\d+");
    
    
    // convert decimal to 16 bit binary
    public static string ConvertIntTo16BitBinary(string command)
    {
        var resultStr = OnlyNumbers.Match(command).Value;

        return Convert.ToString(int.Parse(resultStr), 2).PadLeft(16, '0');
    }
}