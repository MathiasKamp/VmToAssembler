using System.Collections.Generic;
using VmToAssembler.Utils;

namespace VmToAssembler.Operations;

public class GotoOperations
{
    private Dictionary<string,List<string>> GotoOperation { get; }

    public GotoOperations()
    {
        GotoOperation = new Dictionary<string, List<string>>();
        FillJumps();
    }

    private void FillJumps()
    {
        GotoOperation.Add("if-goto", new List<string>
        {
            "@SP",
            "AM=M-1",
            "D=M",
            "A=A-1",
            "@replaced",
            "D;JNE"
        });
        
        GotoOperation.Add("goto", new List<string>
        {
            "@replaced",
            "0;JMP"
        });
    }

    public List<string> GetIfGoto(string label)
    {
        var ifGotoCommand = GotoOperation.GetValueFromDictionary("if-goto");

        var replacedCommand = ifGotoCommand.UpdateReplacedWithNewValue(label);

        return replacedCommand;
    }
    
    public List<string> GetGoto(string label)
    {
       var gotoCommand = GotoOperation.GetValueFromDictionary("goto");

       var replacedCommand = gotoCommand.UpdateReplacedWithNewValue(label);

       return replacedCommand;
    }
    
}