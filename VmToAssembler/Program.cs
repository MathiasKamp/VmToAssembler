using System;
using System.IO;
using System.Linq;

namespace VmToAssembler
{
    internal static class Program
    {
        public static void Main(string[] args)
        {
            if (args.Length > 1)
            {
                Console.WriteLine("only use 1 argument. ie : full path to .asm file");
                return;
            }

            var file = args[0];

            if (!File.Exists(file))
            {
                Console.WriteLine($"file {file} does not exists");
                return;
            }
            
            var parser = new Parser(file);

            var asm = parser.ParseVmToAsm();

            if (!asm.Any())
            {
                Console.WriteLine("no commands in file");
                return;
            }

            var fileWriter =  new FileWriter(asm, file);
            
            fileWriter.DirectoryHandler.CopySourceVmFileToWorkingDirectory(file);
            
            fileWriter.WriteFile();
        }
    }
}