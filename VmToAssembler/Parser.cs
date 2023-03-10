using System.Collections.Generic;
using System.Linq;
using VmToAssembler.Operations;
using VmToAssembler.Utils;

namespace VmToAssembler
{
    public class Parser
    {
        private VmFileReader VmFileReader { get; }


        public List<string> VmTranslatedToAsm { get; }

        private LogicalOperations LogicalOperations { get; }


        private SpOperations SpOperations { get; }

        private MemoryAccessOperations MemoryAccessOperations { get; }

        private CounterHandler CounterHandler { get; }

        private LabelOperations LabelOperations { get; }

        private GotoOperations GotoOperations { get; }

        private bool FirstFile { get; set; }

        private SysOperations SysOperations { get; }

        private FunctionOperations FunctionOperations { get; }

        private Dictionary<string, List<string>> rawFileContent { get; }

        public Parser(string[] files)
        {
            VmFileReader = new VmFileReader(files);
            LogicalOperations = new LogicalOperations();
            SpOperations = new SpOperations();
            SysOperations = new SysOperations();
            FunctionOperations = new FunctionOperations();
            MemoryAccessOperations = new MemoryAccessOperations();
            LabelOperations = new LabelOperations();
            GotoOperations = new GotoOperations();
            CounterHandler = new CounterHandler();
            rawFileContent = VmFileReader.VmFileContents;
            VmTranslatedToAsm = new List<string>();
            FirstFile = false;
        }


        private List<string> HandleLt()
        {
            List<string> ltCommands = new List<string>();

            ltCommands.MergeLists(SpOperations.GetMinus());

            var counterVal = CounterHandler.CreateNewCounters();

            var counters = CounterHandler.GetCounters(counterVal);

            var commands = LogicalOperations.GetLt(counters);

            return ltCommands.MergeLists(commands);
        }

        private List<string> HandleEq()
        {
            List<string> eqCommands = new List<string>();

            eqCommands.MergeLists(SpOperations.GetMinus());

            var counterVal = CounterHandler.CreateNewCounters();

            var counters = CounterHandler.GetCounters(counterVal);

            var commands = LogicalOperations.GetEq(counters);

            return eqCommands.MergeLists(commands);
        }

        private List<string> HandleGt()
        {
            List<string> gtCommands = new List<string>();

            gtCommands.MergeLists(SpOperations.GetMinus());

            var counterVal = CounterHandler.CreateNewCounters();

            var counters = CounterHandler.GetCounters(counterVal);

            var commands = LogicalOperations.GetGt(counters);

            return gtCommands.MergeLists(commands);
        }

        private List<string> HandleSub()
        {
            return LogicalOperations.GetSub();
        }

        private List<string> HandleNeg()
        {
            return LogicalOperations.GetNeg();
        }

        private List<string> HandleAnd()
        {
            return LogicalOperations.GetAnd();
        }

        private List<string> HandleOr()
        {
            return LogicalOperations.GetOr();
        }

        private List<string> HandleNot()
        {
            return LogicalOperations.GetNot();
        }

        private List<string> HandlePush(string command)
        {
            return MemoryAccessOperations.GetPush(command);
        }

        private List<string> HandleReturn()
        {
            return SysOperations.GetReturn();
        }

        public void ParseVmFilesToAsm()
        {
            if (!FirstFile)
            {
                VmTranslatedToAsm.MergeLists(SysOperations.GetStartWithLabel());
                FirstFile = true;
            }

            foreach (var fileContent in rawFileContent.Select(file => file.Value))
            {
                ParseVmToAsm(fileContent);
            }
        }

        private void ParseVmToAsm(List<string> fileContent)

        {
            foreach (var vmCommand in fileContent)
            {
                if (vmCommand.StartsWith("push"))
                {
                    VmTranslatedToAsm.MergeLists(HandlePush(vmCommand));
                    VmTranslatedToAsm.MergeLists(SpOperations.GetPlus());
                }

                else if (vmCommand.StartsWith("pop"))
                {
                    VmTranslatedToAsm.MergeLists(HandlePop(vmCommand));
                }

                else if (vmCommand.StartsWith("add"))
                {
                    VmTranslatedToAsm.MergeLists(HandleAdd());
                }

                else if (vmCommand.StartsWith("eq"))
                {
                    VmTranslatedToAsm.MergeLists(HandleEq());
                }

                else if (vmCommand.StartsWith("lt"))
                {
                    VmTranslatedToAsm.MergeLists(HandleLt());
                }

                else if (vmCommand.StartsWith("gt"))
                {
                    VmTranslatedToAsm.MergeLists(HandleGt());
                }

                else if (vmCommand.StartsWith("and"))
                {
                    VmTranslatedToAsm.MergeLists(HandleAnd());
                }

                else if (vmCommand.StartsWith("or"))
                {
                    VmTranslatedToAsm.MergeLists(HandleOr());
                }

                else if (vmCommand.StartsWith("not"))
                {
                    VmTranslatedToAsm.MergeLists(HandleNot());
                }

                else if (vmCommand.StartsWith("sub"))
                {
                    VmTranslatedToAsm.MergeLists(HandleSub());
                }

                else if (vmCommand.StartsWith("neg"))
                {
                    VmTranslatedToAsm.MergeLists(HandleNeg());
                }
                else if (vmCommand.StartsWith("label"))
                {
                    VmTranslatedToAsm.MergeLists(HandleLabel(vmCommand));
                }

                else if (vmCommand.StartsWith("if-goto"))
                {
                    VmTranslatedToAsm.MergeLists(HandleIfGoto(vmCommand));
                }

                else if (vmCommand.StartsWith("goto"))
                {
                    VmTranslatedToAsm.MergeLists(HandleGoto(vmCommand));
                }

                else if (vmCommand.StartsWith("function"))
                {
                    VmTranslatedToAsm.MergeLists(HandleFunction(vmCommand));
                }

                else if (vmCommand.StartsWith("return"))
                {
                    VmTranslatedToAsm.MergeLists(HandleReturn());
                }

                else if (vmCommand.StartsWith("call"))
                {
                    VmTranslatedToAsm.MergeLists(HandleCall(vmCommand));
                }
            }
        }

        private List<string> HandleCall(string vmCommand)
        {
            return SysOperations.GetCall(vmCommand);
        }

        private List<string> HandleFunction(string vmCommand)
        {
            var splitCommand = vmCommand.SplitByWhitespace();

            var label = splitCommand[1];

            LabelOperations.AddToLabels(label, label);

            var labelCommand = LabelOperations.GetLabelValueList(label, true);

            return labelCommand.MergeLists(FunctionOperations.GetFunctionOperations(int.Parse(splitCommand[2])));
        }

        private List<string> HandleGoto(string vmCommand)
        {
            var splitCommand = vmCommand.SplitByWhitespace();

            LabelOperations.AddToLabels(splitCommand[1], splitCommand[1]);

            var label = LabelOperations.GetLabelValue(splitCommand[1], false);

            return GotoOperations.GetGoto(label);
        }

        private List<string> HandleIfGoto(string vmCommand)
        {
            var splitCommand = vmCommand.SplitByWhitespace();

            LabelOperations.AddToLabels(splitCommand[1], splitCommand[1]);

            var label = LabelOperations.GetLabelValue(splitCommand[1], false);

            return GotoOperations.GetIfGoto(label);
        }

        private List<string> HandleLabel(string vmCommand)
        {
            var splitCommand = vmCommand.SplitByWhitespace();
            LabelOperations.AddToLabels(splitCommand[1], splitCommand[1]);

            return LabelOperations.GetLabelValueList(splitCommand[1], true);
        }

        private List<string> HandlePop(string vmCommand)
        {
            return MemoryAccessOperations.GetPop(vmCommand);
        }


        private List<string> HandleAdd()
        {
            return LogicalOperations.GetAdd();
        }
    }
}