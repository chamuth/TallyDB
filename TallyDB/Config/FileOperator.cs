namespace TallyDB.Config
{
  public class FileOperator : IFileOperable
  {
    public string ReadAllText(string path)
    {
      return File.ReadAllText(path);
    }

    public void WriteAllText(string path, string contents)
    {
      File.WriteAllText(path, contents);
    }
  }
}
