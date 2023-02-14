using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using VmToAssembler.Utils;

namespace VmToAssembler
{
    public class VmFileReader
    {
        private string[] VmFilesToRead { get; }
        
        public Dictionary<string, List<string>> VmFileContents { get; }

        private readonly Regex commentRegex =
            new(@"(@(?:""[^""]*"")+|""(?:[^""\n\\]+|\\.)*""|'(?:[^'\n\\]+|\\.)*')|//.*|/\*(?s:.*?)\*/");

        public VmFileReader(string[] vmFilesToRead)
        {
            VmFilesToRead = vmFilesToRead;
            VmFileContents = new Dictionary<string, List<string>>();
            ReadVmFiles();
        }

        private void ReadVmFiles()
        {
            foreach (var file in VmFilesToRead)
            {
                var vmFile = File.ReadAllLines(file)
                    .Where(arg => !string.IsNullOrWhiteSpace(arg) || !string.IsNullOrEmpty(arg)).ToList();

                var fileName = file.GetFileNameFromFullPath();

                if (!vmFile.Any())
                {
                    return;
                }

                vmFile = TrimFileContent(vmFile);

                VmFileContents.Add(fileName, vmFile);
            }

        }

        private List<string> TrimFileContent(List<string> vmFile)
        {
            vmFile = RemoveCommentsFromAsm(vmFile);
            vmFile = TrimWhiteSpaces(vmFile);
            vmFile = RemoveEmptyLines(vmFile);

            return vmFile;
        }

        private static List<string> TrimWhiteSpaces(List<string> content)
        {
            return content.Select(str => str?.Trim()).ToList();
        }

        private static List<string> RemoveEmptyLines(List<string> content)
        {
            content.RemoveAll(s => string.IsNullOrEmpty(s) || string.IsNullOrWhiteSpace(s));

            return content;
        }


        private List<string> RemoveCommentsFromAsm(IEnumerable<string> fileContent)

        {
            return fileContent.Select(str => commentRegex.Replace(str, "")).ToList();
        }
        
    }
}