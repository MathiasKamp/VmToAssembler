using System.Collections.Generic;

namespace VmToAssembler.Utils;

public static class DictUtils
{
    public static bool ExistsInDictionary <T>(this Dictionary<string,T> dict, string key)
    {
        dict.TryGetValue(key, out var val);

        return val != null;
    }
    
    public static bool ExistsInDictionary <T>(this Dictionary<int,T> dict, int key)
    {
        dict.TryGetValue(key, out var val);

        return val != null;
    }

    public static List<string> GetValueFromDictionary(this Dictionary<string, List<string>> dict, string key)
    {
        dict.TryGetValue(key, out var val);

        return val;
    }

    public static List<string> GetValueFromDict(this Dictionary<int,List<string>> dict, int key)
    {
        dict.TryGetValue(key, out var val);

        return val;
    }
}