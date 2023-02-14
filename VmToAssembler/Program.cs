using System;
using System.IO;
using System.Linq;
using VmToAssembler.Utils;

namespace VmToAssembler
{
    internal static class Program
    {
        public static void Main(string[] args)
        {
            if (args.Length > 1)
            {
                Console.WriteLine("only use 1 argument. ie : full path to .asm file or a directory of vm files");
                return;
            }
            
            var fileToProcess = args[0].PathIsDirectory() ? Directory.GetFiles(args[0], "*.vm") : args;

            var parser = new Parser(fileToProcess);
            
            parser.ParseVmFilesToAsm();

            var asm = parser.VmTranslatedToAsm;

            if (!asm.Any())
            {
                Console.WriteLine("no commands in file");
                return;
            }

            var fileWriter = new FileWriter(args[0], asm);


            fileWriter.WriteFile();
        }
    }
}