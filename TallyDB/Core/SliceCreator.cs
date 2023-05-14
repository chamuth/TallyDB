namespace TallyDB.Core
{
  public class SliceCreator
  {
    Database _database;

    public SliceCreator(Database database)
    {
      _database = database;
    }

    /// <summary>
    /// Create a slice on the database
    /// </summary>
    /// <param name="definition"></param>
    public Slice Create(SliceDefinition definition)
    {
      // Create file for slice
      var filename = Storage.Join(string.Format("{0}\\{1}", _database.Name, definition.Name));
      if (!File.Exists(filename))
      {
        Storage.CreateFile(filename);
      }

      // Return created storage
      var slice = new Slice(filename);
      // Store definition
      slice.Create(definition);

      return slice;
    }
  }
}
