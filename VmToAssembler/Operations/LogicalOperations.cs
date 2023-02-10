using System.Collections.Generic;
using System.Linq;
using VmToAssembler.Utils;

namespace VmToAssembler.Operations;

public class LogicalOperations
{
    private Dictionary<string, List<string>> LogicOperations { get; }

    public LogicalOperations()
    {
        LogicOperations = new Dictionary<string, List<string>>();

        FillLogicOperations();
    }


    private void FillLogicOperations()
    {
        LogicOperations.Add("add", new List<string>
        {
            "@SP",
            "AM=M-1",
            "D=M",
            "A=A-1",
            "M=M+D"
        });

        LogicOperations.Add("sub", new List<string>
        {
            "@SP",
            "AM=M-1",
            "D=M",
            "A=A-1",
            "M=M-D"
        });

        LogicOperations.Add("neg", new List<string>
        {
            "D=0",
            "@SP",
            "A=M-1",
            "M=D-M",
        });

        LogicOperations.Add("eq", new List<string>
        {
            "@falseReplaced",
            "D;JNE",
            "@SP",
            "A=M-1",
            "M=-1",
            "@continueReplaced",
            "0;JMP",
            "(falseReplaced)",
            "@SP",
            "A=M-1",
            "M=0",
            "(continueReplaced)"
            
        });

        LogicOperations.Add("gt", new List<string>
        {
            "@falseReplaced",
            "D;JLE",
            "@SP",
            "A=M-1",
            "M=-1",
            "@continueReplaced",
            "0;JMP",
            "(falseReplaced)",
            "@SP",
            "A=M-1",
            "M=0",
            "(continueReplaced)"
        });

        LogicOperations.Add("lt", new List<string>
        {
            "@falseReplaced",
            "D;JGE",
            "@SP",
            "A=M-1",
            "M=-1",
            "@continueReplaced",
            "0;JMP",
            "(falseReplaced)",
            "@SP",
            "A=M-1",
            "M=0",
            "(continueReplaced)"
        });
        LogicOperations.Add("and", new List<string>
        {
            "@SP",
            "AM=M-1",
            "D=M",
            "A=A-1",
            "M=M&D",
        });

        LogicOperations.Add("or", new List<string>
        {
            "@SP",
            "AM=M-1",
            "D=M",
            "A=A-1",
            "M=M|D",
        });

        LogicOperations.Add("not", new List<string>
        {
            "@SP",
            "A=M-1",
            "M=!M"
        });
    }

    private List<string> GetLogicOperation(string operation)
    {
        return LogicOperations.GetValueFromDictionary(operation);
    }

    public List<string> GetAdd()
    {
        return GetLogicOperation("add");
    }

    public List<string> GetSub()
    {
        return GetLogicOperation("sub");
    }

    public List<string> GetNeg()
    {
        return GetLogicOperation("neg");
    }

    public List<string> GetEq(List<string> counters)
    {
        var eqCommands = GetLogicOperation("eq");

        return UpdateReplaced(eqCommands, counters);
    }


    public List<string> GetGt(List<string> counters)
    {
        var gtCommands = GetLogicOperation("gt");

        return UpdateReplaced(gtCommands, counters);
    }

    public List<string> GetLt(List<string> counters)
    {
        var ltCommands = GetLogicOperation("lt");

        return UpdateReplaced(ltCommands, counters);
    }

    private List<string> UpdateReplaced(List<string> commands, List<string> counters)
    {
        var falseCounter = counters.FirstOrDefault(counter => counter.Contains("FALSE"));

        var continueCounter = counters.FirstOrDefault(counter => counter.Contains("CONTINUE"));

        var falseReplaced = commands.Select(cmd => cmd.Replace("falseReplaced", falseCounter)).ToList();

        var replaced = falseReplaced.Select(cmd => cmd.Replace("continueReplaced", continueCounter)).ToList();

        return replaced;
    }

    public List<string> GetAnd()
    {
        return GetLogicOperation("and");
    }

    public List<string> GetOr()
    {
        return GetLogicOperation("or");
    }


    public List<string> GetNot()
    {
        return GetLogicOperation("not");
    }
}