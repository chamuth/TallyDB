using TallyDB.Core.Exceptions;

namespace TallyDB.Core
{
  /// <summary>
  /// Represents a Database instance
  /// </summary>
  internal class Database
  {
    string _name;
    List<Slice> _slices = new List<Slice>();

    public Database(string name)
    {
      _name = name;
    }


    public static Slice[] GetSlices(string databaseName)
    {
      var returner = new List<Slice>();

      return returner.ToArray();
    }

    /// <summary>
    /// Create Database on storage
    /// </summary>
    /// <exception cref="DatabaseCreateFailedException"></exception>
    public void Create()
    {
      try
      {
        Storage.CreateDirectory(_name);
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
      var files = Storage.GetFilesInDirectory(_name);
      
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
