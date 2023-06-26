using TallyDB.Core;
using TallyDB.Server.Errors;

namespace TallyDB.Server
{
  /// <summary>
  /// Responsible for managing Database instances
  /// </summary>
  public static class DatabaseManager
  {
    private static List<Database> Databases = new List<Database>();

    /// <summary>
    /// Loads databases from the disk to memory
    /// </summary>
    public static void LoadDatabases()
    {
      Databases.Clear();

      foreach(var dir in Storage.GetDirectoriesInDirectory())
      {
        var name = Path.GetRelativePath(Storage.RootDirectory, dir);
        if (name == null)
        {
          continue;
        }

        var db = new Database(name);
        db.Load();

        Databases.Add(db);
      }
    }

    /// <summary>
    /// Find and return loaded database by name
    /// </summary>
    /// <param name="name">Name of the database</param>
    /// <returns>Database entity</returns>
    public static Database GetDatabase(string name)
    {
      var db = Databases.FirstOrDefault((db) => db.Name == name);

      if (db == null)
      {
        throw DatabaseErrors.DatabaseCreationFailedError;
      }

      return db;
    }

    /// <summary>
    /// Get all loaded databases
    /// </summary>
    /// <returns>List of databases</returns>
    public static Database[] GetDatabases()
    {
      return Databases.ToArray();
    }

    /// <summary>
    /// Create database and add to memory
    /// </summary>
    /// <param name="name">Name of the database</param>
    public static void CreateDatabase(string name)
    {
      var db = new Database(name);
      db.Create();
      Databases.Add(db);
    }
    
    /// <summary>
    /// Delete a database from storage and unload it from memory
    /// </summary>
    /// <param name="name">Name of the database</param>
    public static void DeleteDatabase(string name)
    {
      GetDatabase(name).DeleteSelf();
      UnloadDatabase(name);
    }

    /// <summary>
    /// Unload database and slice data from memory
    /// </summary>
    /// <param name="name">Name of the database</param>
    private static void UnloadDatabase(string name)
    {
      Databases.RemoveAt(Databases.FindIndex(x => x.Name == name));
    }
  }
}
