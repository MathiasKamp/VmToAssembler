using System.Collections.Generic;
using VmToAssembler.Utils;

namespace VmToAssembler.Operations;

public class FunctionOperations
{
    private Dictionary<string, List<string>> FunctionOperation { get; }

    public FunctionOperations()
    {
        FunctionOperation = new Dictionary<string, List<string>>();
        FillFunctionOperations();
    }

    private void FillFunctionOperations()
    {
        FunctionOperation.Add("add", new List<string>
        {
            "@replaced",
            "D=A",
            "@SP",
            "A=M",
            "M=D",
            "@SP",
            "M=M+1"
        });
    }

    private List<string> GetAdd(int num)
    {
        return FunctionOperation.GetValueFromDictionary("add").UpdateReplacedWithNewValue(num.ToString());
    }

    public List<string> GetFunctionOperations(int argNumber)
    {
        List<string> operations = new List<string>();

        for (int i = 0; i < argNumber; i++)
        {
            operations.AddRange(GetAdd(i));
        }

        return operations;
    }
}