using System.Collections.Generic;

namespace VmToAssembler.Utils;

public static class ListUtil
{
    public static List<string> MergeLists(this List<string> list1, List<string> list2)
    {
        list1.AddRange(list2);

        return list1;
    }
}