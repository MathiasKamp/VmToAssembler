using System.Collections.Generic;
using System.Linq;

namespace VmToAssembler.Utils;

public static class ListUtil
{
    public static List<string> MergeLists(this List<string> list1, List<string> list2)
    {
        list1.AddRange(list2);

        return list1;
    }

    public static List<string> UpdateReplacedWithNewValue(this List<string> list, string newValue)
    {
        return !list.Any()
            ? new List<string>()
            : list.Select(cmd => cmd.Replace("replaced", newValue)).ToList();
    }
}