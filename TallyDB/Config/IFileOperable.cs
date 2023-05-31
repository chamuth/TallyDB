namespace TallyDB.Config
{
  public interface IFileOperable
  {
    public string ReadAllText(string path);

    public void WriteAllText(string path, string contents);
  }
}
