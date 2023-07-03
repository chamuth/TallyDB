namespace TallyDB.Config
{
  public class FileOperator : IFileOperable
  {
    public string ReadAllText(string path)
    {
      if (!File.Exists(path))
      {
        return "";
      }

      return File.ReadAllText(path);
    }

    public void WriteAllText(string path, string contents)
    {
      string? directory = Path.GetDirectoryName(path);
      if (directory != null && !Directory.Exists(directory))
      {
        Directory.CreateDirectory(directory);
      }

      File.WriteAllText(path, contents);
    }
  }
}
