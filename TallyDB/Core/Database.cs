using TallyDB.Core.Exceptions;

namespace TallyDB.Core
{
  /// <summary>
  /// Represents a Database instance
  /// </summary>
  public class Database
  {
    public string Name { get; set; }
    List<Slice> _slices = new List<Slice>();

    public Database(string name)
    {
      Name = name;
    }

    /// <summary>
    /// Create Database on storage
    /// </summary>
    /// <exception cref="DatabaseCreateFailedException"></exception>
    public void Create()
    {
      try
      {
        Storage.CreateDirectory(Name);
      }
      catch (Exception)
      {
        throw new DatabaseCreateFailedException();
      }
    }


    /// <summary>
    /// Loads slices from the database directory
    /// </summary>
    public void Load()
    {
      var files = Storage.GetFilesInDirectory(Name);
      
      foreach(var file in files)
      {
        _slices.Add(new Slice(file));
      }
    }


    /// <summary>
    /// Query the database
    /// </summary>
    public void Query()
    {

    }
  }
}
