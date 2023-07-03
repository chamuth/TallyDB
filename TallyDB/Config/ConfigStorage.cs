using static System.Environment;
using Newtonsoft.Json;

namespace TallyDB.Config
{
  public abstract class ConfigStorage<T>
  {
    public abstract string Path { get; set; }
    public static string RootDirectory = System.IO.Path.Combine(GetFolderPath(SpecialFolder.ApplicationData), "TallyDB/Config/");

    private IFileOperable file;

    /// <summary>
    /// Initialize config storage
    /// </summary>
    /// <param name="file">Injectable File Operation dependency</param>
    public ConfigStorage(IFileOperable file)
    {
      this.file = file;
    }

    private string GetPath()
    {
      return System.IO.Path.Combine(RootDirectory, Path);
    }

    public T[] GetAll()
    {
      var data = JsonConvert.DeserializeObject<T[]>(file.ReadAllText(GetPath()));

      if (data != null)
      {
        return data;
      }

      return new T[] { };
    }

    public void Insert(T data)
    {
      var current = GetAll();
      var amended = current.Append(data);
      var newContents = JsonConvert.SerializeObject(amended);

      file.WriteAllText(GetPath(), newContents);
    }

    public void Save(T data, Func<T, bool> predicate)
    {
      var currentData = GetAll();
      currentData = currentData.Select((each) =>
      {
        if (predicate(each))
        {
          return data;
        }

        return each;
      }).ToArray();

      var newContents = JsonConvert.SerializeObject(currentData);
      file.WriteAllText(GetPath(), newContents);
    }
  }
}
