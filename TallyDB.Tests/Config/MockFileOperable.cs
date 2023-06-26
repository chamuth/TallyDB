using TallyDB.Config;

namespace TallyDB.Tests.Config
{
  public class MockFileOperable : IFileOperable
  {
    private Dictionary<string, string> fileContents = new Dictionary<string, string>();

    public string ReadAllText(string path)
    {
      if (!fileContents.ContainsKey(path))
      {
        fileContents.Add(path, "");
      }

      return fileContents[path];
    }

    public void WriteAllText(string path, string contents)
    {
      fileContents[path] = contents;
    }
  }
}
