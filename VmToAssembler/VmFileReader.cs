using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace VmToAssembler
{
    public class VmFileReader
    {
        private string VmFileToRead { get; }
        public List<string> VmFileContent { get; private set; }

        private readonly Regex commentRegex =
            new(@"(@(?:""[^""]*"")+|""(?:[^""\n\\]+|\\.)*""|'(?:[^'\n\\]+|\\.)*')|//.*|/\*(?s:.*?)\*/");

        public VmFileReader(string vmFileToRead)
        {
            VmFileToRead = vmFileToRead;
            VmFileContent = new List<string>();
            ReadVmFile();
        }

        private void ReadVmFile()
        {
            var fileContent = File.ReadAllLines(VmFileToRead)
                .Where(arg => !string.IsNullOrWhiteSpace(arg) || !string.IsNullOrEmpty(arg)).ToList();

            if (!fileContent.Any()) return;

            VmFileContent = fileContent;

            TrimFileContent();
        }

        private void TrimFileContent()
        {
            VmFileContent = RemoveCommentsFromAsm(VmFileContent);
            VmFileContent = TrimWhiteSpaces(VmFileContent);
            VmFileContent = RemoveEmptyLines(VmFileContent);
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

        public bool HasFunction()
        {
            return VmFileContent.Any(str => str.StartsWith("function"));
        }
    }
}