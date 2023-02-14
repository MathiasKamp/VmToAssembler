using System;
using System.Collections.Generic;
using System.IO;
using VmToAssembler.Utils;

namespace VmToAssembler
{
    public class FileWriter
    {
        private string DirectoryPathOfVmFiles { get; }

        private List<string> Commands { get; }

        public FileWriter(string directoryPathOfVmFiles, List<string> commands)

        {
            Commands = commands;
            DirectoryPathOfVmFiles = directoryPathOfVmFiles;
        }

        public void WriteFile()
        {
            try
            {
                var fileName = new DirectoryInfo(DirectoryPathOfVmFiles).Name;

                var file = CreateAsmFile(fileName);

                File.WriteAllLines(file, Commands);

                Console.WriteLine($"file created at : {file}");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private string CreateAsmFile(string fileName)
        {
            string file = DirectoryPathOfVmFiles.CheckTrailingSlash() + fileName + ".asm";

            file.CreateFileIfNotExists();

            return file;
        }
    }
}