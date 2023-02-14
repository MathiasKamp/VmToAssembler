using System.IO;

namespace VmToAssembler.Utils;

public static class FileUtil
{
    public static bool CheckIfFileExists(this string file)
    {
        bool exists = false;

        if (!string.IsNullOrEmpty(file) && !string.IsNullOrWhiteSpace(file))
        {
            exists = File.Exists(file);
        }

        return exists;
    }
    
    public static void CreateFileIfNotExists(this string file)
    {
        if (!string.IsNullOrEmpty(file))
        {
            if (!file.CheckIfFileExists())
            {
                File.Create(file).Close();
            }
        }
    }
    
    public static void WriteToFile(string file, string content)
    {
        if (!string.IsNullOrEmpty(file))
        {
            if (file.CheckIfFileExists())
            {
                using (StreamWriter sw = new StreamWriter(file, true))
                {
                    sw.Write(content);
                    sw.Close();
                }
            }
        }
    }
    
    public static bool CopyFileToDestination(string sourceFile, string destinationFile, bool overwrite)
    {
        if (!sourceFile.CheckIfFileExists()) return false;
            
        File.Copy(sourceFile, destinationFile, overwrite);

        var fileExists = destinationFile.CheckIfFileExists();

        return fileExists;
    }

    public static bool MoveFileToDestination(string sourceFile, string destinationFile, bool overwrite)
    {
        bool fileMoved = false;

        if (!sourceFile.CheckIfFileExists()) return false;

        if (overwrite && destinationFile.CheckIfFileExists())
        {
            File.Delete(destinationFile);
        }
            
        File.Move(sourceFile, destinationFile);

        fileMoved = destinationFile.CheckIfFileExists();

        return fileMoved;
    }

    public static string GetFileNameFromFullPath(this string file)
    {
        if (!CheckIfFileExists(file))
        {
            return "";
        }

        return Path.GetFileNameWithoutExtension(file);
    }
}