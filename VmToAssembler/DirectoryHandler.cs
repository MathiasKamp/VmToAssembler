using System;
using System.IO;
using VmToAssembler.Utils;

namespace VmToAssembler;

public class DirectoryHandler
{
    private string VmFileWorkingDirectory { get; }

    public string DirectoryVmFile { get; private set; }

    public DirectoryHandler(string vmFileWorkingDirectory)
    {
        VmFileWorkingDirectory = vmFileWorkingDirectory;
    }

    public void CopySourceVmFileToWorkingDirectory(string file)
    {
        DirectoryVmFile = CreateDestinationDirectoryFromFileName(file);

        var fileName = Path.GetFileName(file);

        var destinationAsmFile = DirectoryVmFile.CheckTrailingSlash() + fileName;

        Console.WriteLine($"copying asm source file to {destinationAsmFile}");

        FileUtil.CopyFileToDestination(file, destinationAsmFile, true);
    }

    private string CreateDestinationDirectoryFromFileName(string fileName)
    {
        var name = FileUtil.GetFileNameFromFullPath(fileName);

        var workingDirectory = VmFileWorkingDirectory.CheckTrailingSlash();

        var directory = workingDirectory.CheckTrailingSlash() + name.CheckTrailingSlash();

        DirUtil.CreateDirIfNotExists(directory);

        return directory;
    }
}