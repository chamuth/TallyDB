
using static System.Environment;

namespace TallyDB.Core
{
  internal static class Storage
  {
    public static string RootDirectory = Path.Combine(GetFolderPath(SpecialFolder.CommonApplicationData), "TallyDB\\Source\\");

    static Storage()
    {
      if (!Directory.Exists(RootDirectory))
      {
        Directory.CreateDirectory(RootDirectory);
      }
    }

    static string Join(string sub)
    {
      return Path.Combine(RootDirectory, sub)
        .SanitizeFilename();
    }

    public static void CreateDirectory(string name)
    {
      Directory.CreateDirectory(Join(name));
    }

    public static void CreateFile(string name)
    {
      File.Create(Join(name));
    }

    public static void GetDirectoriesInDirectory()
    {
      Directory.GetDirectories(Join(""));
    }
    
    public static string[] GetFilesInDirectory(string name)
    {
      return Directory.GetFiles(Join(name));
    }
  }
}