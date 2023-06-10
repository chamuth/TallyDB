
using static System.Environment;

namespace TallyDB.Core
{
  public static class Storage
  {
    public static string RootDirectory = Path.Combine(GetFolderPath(SpecialFolder.CommonApplicationData), "TallyDB\\Source\\");

    static Storage()
    {
      if (!Directory.Exists(RootDirectory))
      {
        Directory.CreateDirectory(RootDirectory);
      }
    }

    public static string Join(string sub)
    {
      return Path.Combine(RootDirectory, sub);
    }

    public static void CreateDirectory(string name)
    {
      var directory = Join(name);
      Directory.CreateDirectory(directory);
      Console.WriteLine("Created Directory: {0}", directory);
    }

    public static void CreateFile(string name)
    {
      var fs = File.Create(Join(name).AppendExtension());
      fs.Close();
    }

    public static string[] GetDirectoriesInDirectory()
    {
      return Directory.GetDirectories(Join(""));
    }
    
    public static string[] GetFilesInDirectory(string name)
    {
      return Directory.GetFiles(Join(name));
    }
  }
}