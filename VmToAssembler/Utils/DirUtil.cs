using System.IO;

namespace VmToAssembler.Utils;

public static class DirUtil
{
    public static bool CheckIfDirectoryExists(string directory)
    {
        bool exists = false;

        if (!string.IsNullOrEmpty(directory) && !string.IsNullOrWhiteSpace(directory))
        {
            exists = Directory.Exists(directory);
        }

        return exists;
    }

    public static void CreateDirIfNotExists(string directory)
    {
        if (!string.IsNullOrEmpty(directory))
        {
            if (!Directory.Exists(directory)) Directory.CreateDirectory(directory);
        }
    }

    public static string CheckTrailingSlash(this string directory)
    {
        return directory.EndsWith(@"\") ? directory : directory + @"\";
    }
}