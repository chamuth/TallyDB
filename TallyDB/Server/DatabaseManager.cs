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
  }
}
