using TallyDB.Core.Exceptions;
using TallyDB.Server.Errors;

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
        throw DatabaseErrors.DatabaseCreationFailedError;
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
        var slice = new Slice(file);
        slice.Load();
        _slices.Add(slice);
      }
    }

    /// <summary>
    /// Get slice by name
    /// </summary>
    /// <param name="name">Slice name</param>
    /// <returns>Slice instance</returns>
    public Slice GetSlice(string name)
    {
      var slice = _slices.FirstOrDefault(sl => sl.Name == name);

      if (slice == null)
      {
        throw DatabaseErrors.SliceNotFoundError;
      }

      return slice;
    }

    /// <summary>
    /// Get all loaded slices
    /// </summary>
    /// <returns>All slices</returns>
    public Slice[] GetAllSlices()
    {
      return _slices.ToArray();
    }

    /// <summary>
    /// Delete database self
    /// </summary>
    public void DeleteSelf()
    {
      Storage.DeleteDirectory(Name);
    }
  }
}
