using System.Collections.Generic;
using VmToAssembler.Utils;

namespace VmToAssembler.Operations;

public class SysOperations
{
    private Dictionary<string, List<string>> SysOperation { get; }

    private int callLabelNumber;

    public SysOperations()
    {
        SysOperation = new Dictionary<string, List<string>>();
        FillSysOperations();
        callLabelNumber = 0;
    }

    private void FillSysOperations()
    {
        SysOperation.Add("startWithLabel", new List<string>
        {
            "@256",
            "D=A",
            "@SP",
            "M=D",
            "@RETURNLabel",
            "D=A",
            "@SP",
            "A=M",
            "M=D",
            "@SP",
            "M=M+1",
            "@LCL",
            "D=M",
            "@SP",
            "A=M",
            "M=D",
            "@SP",
            "M=M+1",
            "@ARG",
            "D=M",
            "@SP",
            "A=M",
            "M=D",
            "@SP",
            "M=M+1",
            "@THIS",
            "D=M",
            "@SP",
            "A=M",
            "M=D",
            "@SP",
            "M=M+1",
            "@THAT",
            "D=M",
            "@SP",
            "A=M",
            "M=D",
            "@SP",
            "M=M+1",
            "@SP",
            "D=M",
            "@5",
            "D=D-A",
            "@0",
            "D=D-A",
            "@ARG",
            "M=D",
            "@SP",
            "D=M",
            "@LCL",
            "M=D",
            "@Sys.init",
            "0;JMP",
            "(RETURNLabel)"
        });

        SysOperation.Add("startWithoutLabel", new List<string>
        {
            "@256",
            "D=A",
            "@SP",
            "M=D",
            "D=A",
            "@SP",
            "A=M",
            "M=D",
            "@SP",
            "M=M+1",
            "@LCL",
            "D=M",
            "@SP",
            "A=M",
            "M=D",
            "@SP",
            "M=M+1",
            "@ARG",
            "D=M",
            "@SP",
            "A=M",
            "M=D",
            "@SP",
            "M=M+1",
            "@THIS",
            "D=M",
            "@SP",
            "A=M",
            "M=D",
            "@SP",
            "M=M+1",
            "@THAT",
            "D=M",
            "@SP",
            "A=M",
            "M=D",
            "@SP",
            "M=M+1",
            "@SP",
            "D=M",
            "@5",
            "D=D-A",
            "@0",
            "D=D-A",
            "@ARG",
            "M=D",
            "@SP",
            "D=M",
            "@LCL",
            "M=D",
            "@Sys.init",
            "0;JMP"
        });

        SysOperation.Add("return", new List<string>
        {
            "@LCL",
            "D=M",
            "@R11",
            "M=D",
            "@5",
            "A=D-A",
            "D=M",
            "@R12",
            "M=D",
            "@ARG",
            "D=M",
            "@0",
            "D=D+A",
            "@R13",
            "M=D",
            "@SP",
            "AM=M-1",
            "D=M",
            "@R13",
            "A=M",
            "M=D",
            "@ARG",
            "D=M",
            "@SP",
            "M=D+1",
            "@R11",
            "D=M-1",
            "AM=D",
            "D=M",
            "@THAT",
            "M=D",
            "@R11",
            "D=M-1",
            "AM=D",
            "D=M",
            "@THIS",
            "M=D",
            "@R11",
            "D=M-1",
            "AM=D",
            "D=M",
            "@ARG",
            "M=D",
            "@R11",
            "D=M-1",
            "AM=D",
            "D=M",
            "@LCL",
            "M=D",
            "@R12",
            "A=M",
            "0;JMP"
        });
        
        SysOperation.Add("call", new List<string>
        {
            "@RETURNLabel",
            "D=A",
            "@SP",
            "A=M",
            "M=D",
            "@SP",
            "M=M+1",
            "@LCL",
            "D=M",
            "@SP",
            "A=M",
            "M=D",
            "@SP",
            "M=M+1",
            "@ARG",
            "D=M",
            "@SP",
            "A=M",
            "M=D",
            "@SP",
            "M=M+1",
            "@THIS",
            "D=M",
            "@SP",
            "A=M",
            "M=D",
            "@SP",
            "M=M+1",
            "@THAT",
            "D=M",
            "@SP",
            "A=M",
            "M=D",
            "@SP",
            "M=M+1",
            "@SP",
            "D=M",
            "@5",
            "D=D-A",
            "@argumentCount",
            "D=D-A",
            "@ARG",
            "M=D",
            "@SP",
            "D=M",
            "@LCL",
            "M=D",
            "@functionLabel",
            "0;JMP",
            "(RETURNLabel)"
        });
    }

    private int GetNewLabelNumber()
    {
        return callLabelNumber++;
    }

    public List<string> GetCall(string command)
    {
        var commandSplit = command.SplitByWhitespace();

        var val = SysOperation.GetValueFromDictionary(commandSplit[0])
            .UpdateStrWithNewValue("RETURNLabel", $"RETURN{GetNewLabelNumber()}")
            .UpdateStrWithNewValue("functionLabel", commandSplit[1]).UpdateStrWithNewValue("argumentCount", commandSplit[2]);
        return val;
    }
    

    public List<string> GetReturn()
    {
        return SysOperation.GetValueFromDictionary("return");
    }
    
    public List<string> GetStartWithLabel()
    {
        return SysOperation.GetValueFromDictionary("startWithLabel").UpdateStrWithNewValue("RETURNLabel", $"RETURN{GetNewLabelNumber()}");
    }
    
    public List<string> GetStartWithoutLabel()
    {
        return SysOperation.GetValueFromDictionary("startWithoutLabel");
    }
}