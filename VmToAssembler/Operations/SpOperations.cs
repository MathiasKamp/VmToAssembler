using System.Collections.Generic;
using VmToAssembler.Utils;

namespace VmToAssembler.Operations;

public class SpOperations
{
    private Dictionary<string, List<string>> Operations { get; }

    public SpOperations()
    {
        Operations = new Dictionary<string, List<string>>();
        FillOperations();
    }

    private void FillOperations()
    {
        Operations.Add("plus", new List<string>
        {
            "@SP",
            "A=M",
            "M=D",
            "@SP",
            "M=M+1"
        });
        
        Operations.Add("minus", new List<string>
        {
            "@SP",
            "AM=M-1",
            "D=M",
            "A=A-1",
            "D=M-D"
        });
    }

    private List<string> GetSpOperation(string operation)
    {
        return Operations.GetValueFromDictionary(operation);
    }

    public List<string> GetPlus()
    {
        return GetSpOperation("plus");
    }
    
    
    public List<string> GetMinus()
    {
        return GetSpOperation("minus");
    }
}