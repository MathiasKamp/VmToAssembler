using System.Collections.Generic;
using System.Linq;
using VmToAssembler.Utils;

namespace VmToAssembler.Operations;

public class MemoryAccessOperations
{
    public Dictionary<string, List<string>> MemoryAccessOperation { get; }

    private readonly int staticSegment = 16;

    public MemoryAccessOperations()
    {
        MemoryAccessOperation = new Dictionary<string, List<string>>();
        FillMemoryAccessOperation();
    }

    private void FillMemoryAccessOperation()
    {
        MemoryAccessOperation.Add("push constant", new List<string>
        {
            "@replaced",
            "D=A"
        });

        MemoryAccessOperation.Add("push local", new List<string>
        {
            "@LCL",
            "D=M",
            "@replaced",
            "A=D+A",
            "D=M"
        });

        MemoryAccessOperation.Add("push that", new List<string>
        {
            "@THAT",
            "D=M",
            "@replaced",
            "A=D+A",
            "D=M"
        });

        MemoryAccessOperation.Add("push argument", new List<string>
        {
            "@ARG",
            "D=M",
            "@replaced",
            "A=D+A",
            "D=M"
        });

        MemoryAccessOperation.Add("push temp", new List<string>
        {
            "@R5",
            "D=M",
            "@11",
            "A=D+A",
            "D=M"
        });

        MemoryAccessOperation.Add("push this", new List<string>
        {
            "@THIS",
            "D=M",
            "@replaced",
            "A=D+A",
            "D=M"
        });

        MemoryAccessOperation.Add("pop local", new List<string>
        {
            "@LCL",
            "D=M",
            "@replaced",
            "D=D+A",
            "@R13",
            "M=D",
            "@SP",
            "AM=M-1",
            "D=M",
            "@R13",
            "A=M",
            "M=D"
        });

        MemoryAccessOperation.Add("pop argument", new List<string>
        {
            "@ARG",
            "D=M",
            "@replaced",
            "D=D+A",
            "@R13",
            "M=D",
            "@SP",
            "AM=M-1",
            "D=M",
            "@R13",
            "A=M",
            "M=D"
        });

        MemoryAccessOperation.Add("push pointer 0", new List<string>
        {
            "@THIS",
            "D=M"
        });
        MemoryAccessOperation.Add("push pointer 1", new List<string>
        {
            "@THAT",
            "D=M"
        });

        MemoryAccessOperation.Add("pop static", new List<string>
        {
            "@replaced",
            "D=A",
            "@R13",
            "M=D",
            "@SP",
            "AM=M-1",
            "D=M",
            "@R13",
            "A=M",
            "M=D"
        });

        MemoryAccessOperation.Add("push static", new List<string>
        {
            "@replaced",
            "D=M"
        });

        MemoryAccessOperation.Add("pop this", new List<string>
        {
            "@THIS",
            "D=M",
            "@replaced",
            "D=D+A",
            "@R13",
            "M=D",
            "@SP",
            "AM=M-1",
            "D=M",
            "@R13",
            "A=M",
            "M=D"
        });

        MemoryAccessOperation.Add("pop that", new List<string>
        {
            "@THAT",
            "D=M",
            "@replaced",
            "D=D+A",
            "@R13",
            "M=D",
            "@SP",
            "AM=M-1",
            "D=M",
            "@R13",
            "A=M",
            "M=D"
        });

        MemoryAccessOperation.Add("pop temp", new List<string>
        {
            "@R5",
            "D=M",
            "@11",
            "D=D+A",
            "@R13",
            "M=D",
            "@SP",
            "AM=M-1",
            "D=M",
            "@R13",
            "A=M",
            "M=D"
        });

        MemoryAccessOperation.Add("pop pointer 0", new List<string>
        {
            "@THIS",
            "D=A",
            "@R13",
            "M=D",
            "@SP",
            "AM=M-1",
            "D=M",
            "@R13",
            "A=M",
            "M=D"
        });

        MemoryAccessOperation.Add("pop pointer 1", new List<string>
        {
            "@THAT",
            "D=A",
            "@R13",
            "M=D",
            "@SP",
            "AM=M-1",
            "D=M",
            "@R13",
            "A=M",
            "M=D"
        });
    }
    

    public List<string> GetPush(string command)
    {
        var splitCommand = command.SplitByWhitespace();

        if (splitCommand[1].Contains("pointer"))
        {
            return GetCommandsBasedOnWholeCommand(splitCommand);
        }

        if (splitCommand[1].Contains("static"))
        {
            // calculate static value
            var staticVal = int.Parse(splitCommand[2]) + staticSegment;

            var commands = MemoryAccessOperation.GetValueFromDictionary(splitCommand[0] + " " + splitCommand[1]);

            return UpdateReplacedWithCorrectAddressValue(commands, staticVal.ToString());
        }

        return GetCommandsBasedOnFirstTwoWords(splitCommand);
    }

    private List<string> GetCommandsBasedOnFirstTwoWords(string[] command)
    {
        var commandList = MemoryAccessOperation.GetValueFromDictionary(command[0] + " " + command[1]);

        bool containsReplaced = false;

        foreach (var cmd in commandList.Where(cmd => cmd.Contains("replaced")))
        {
            containsReplaced = true;
        }

        return !containsReplaced ? commandList : UpdateReplacedWithCorrectAddressValue(commandList, command[2]);
    }

    private List<string> GetCommandsBasedOnWholeCommand(string[] command)
    {
        var commandList =
            MemoryAccessOperation.GetValueFromDictionary(command[0] + " " + command[1] + " " + command[2]);

        bool containsReplaced = false;

        foreach (var cmd in commandList.Where(cmd => cmd.Contains("replaced")))
        {
            containsReplaced = true;
        }

        return !containsReplaced ? commandList : UpdateReplacedWithCorrectAddressValue(commandList, command[2]);
    }

    private List<string> UpdateReplacedWithCorrectAddressValue(List<string> commandList, string valToReplace)
    {
        return !commandList.Any()
            ? new List<string>()
            : commandList.Select(cmd => cmd.Replace("replaced", valToReplace)).ToList();
    }

    public List<string> GetPop(string vmCommand)
    {
        var splitCommand = vmCommand.SplitByWhitespace();

        if (splitCommand[1].Contains("pointer"))
        {
            return GetCommandsBasedOnWholeCommand(splitCommand);
        }

        if (splitCommand[1].Contains("static"))
        {
            // calculate static value
            var staticVal = int.Parse(splitCommand[2]) + staticSegment;

            var commands = MemoryAccessOperation.GetValueFromDictionary(splitCommand[0] + " " + splitCommand[1]);

            return UpdateReplacedWithCorrectAddressValue(commands, staticVal.ToString());
        }

        return GetCommandsBasedOnFirstTwoWords(splitCommand);
    }
}