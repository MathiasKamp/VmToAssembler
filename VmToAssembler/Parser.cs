using System.Collections.Generic;
using VmToAssembler.Operations;
using VmToAssembler.Utils;

namespace VmToAssembler
{
    public class Parser
    {
        private VmFileReader VmFileReader { get; }

        private List<string> RawVmFileContent { get; }

        private List<string> VmTranslatedToAsm { get; }

        private LogicalOperations LogicalOperations { get; }


        private SpOperations SpOperations { get; }

        private MemoryAccessOperations MemoryAccessOperations { get; }


        private CounterHandler CounterHandler { get; }


        public Parser(string file)
        {
            VmFileReader = new VmFileReader(file);
            LogicalOperations = new LogicalOperations();
            SpOperations = new SpOperations();
            MemoryAccessOperations = new MemoryAccessOperations();

            CounterHandler = new CounterHandler();
            RawVmFileContent = VmFileReader.VmFileContent;
            VmTranslatedToAsm = new List<string>();
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


        public List<string> ParseVmToAsm()
        {
            foreach (var vmCommand in RawVmFileContent)
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
            }

            return VmTranslatedToAsm;
        }

        private List<string> HandlePop(string vmCommand)
        {
            return MemoryAccessOperations.GetPop(vmCommand);
        }


        // adds two stack addresses together
        private List<string> HandleAdd()
        {
            return LogicalOperations.GetAdd();
        }
    }
}