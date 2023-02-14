using System.Collections.Generic;
using System.Reflection.Emit;
using VmToAssembler.Utils;

namespace VmToAssembler.Operations;

public class LabelOperations
{
    private Dictionary<string, string> Labels { get; }

    public LabelOperations()
    {
        Labels = new Dictionary<string, string>();
    }

    public List<string> GetLabelValueList(string key, bool doWrapInParenthesis)
    {
        var label = GetLabelValue(key, doWrapInParenthesis);
        
        return new List<string>
        {
            label
        };
    }

    public string GetLabelValue(string key, bool shouldWrapWithParenthesis)
    {
        if (!shouldWrapWithParenthesis) return Labels.GetValueFromDict(key);
        
        var label = Labels.GetValueFromDict(key);

        return "(" + label + ")";
    }

    // might return boolean if added
    public void AddToLabels(string key, string value)
    {
        Labels.AddToDictIfNotExists(key, value);
    }
}