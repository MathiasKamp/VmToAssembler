using System;
using System.Collections.Generic;
using System.IO;
using VmToAssembler.Utils;

namespace VmToAssembler
{
    public class FileWriter
    {
        private string VmFileName { get; }

        public DirectoryHandler DirectoryHandler { get; }

        private List<string> VmFileContent { get; }
        
        private string VmFileWorkingDirectory { get; }

        public FileWriter(List<string> vmFileContent, string vmFileName)

        {
            VmFileName = vmFileName;
            VmFileWorkingDirectory = SysUtil.GetRootPath.CheckTrailingSlash() + "vmFiles".CheckTrailingSlash();
            DirectoryHandler = new DirectoryHandler(VmFileWorkingDirectory);
            VmFileContent = vmFileContent;
        }

        public void WriteFile()
        {
            try
            {
                if (string.IsNullOrEmpty(VmFileName) && string.IsNullOrWhiteSpace(VmFileName))
                {
                    Console.WriteLine("file content is empty");
                    throw new ArgumentException("fileName is empty");
                }

                var fileName = Path.GetFileNameWithoutExtension(VmFileName);

                var file = CreateAsmFile(fileName);

                File.WriteAllLines(file, VmFileContent);

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
            string file = DirectoryHandler.DirectoryVmFile + fileName + ".asm";

            file.CreateFileIfNotExists();

            return file;
        }
    }
}