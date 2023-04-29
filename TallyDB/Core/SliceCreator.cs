namespace TallyDB.Core
{
  internal class SliceCreator
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
      var filename = Storage.Join(string.Format("{0}\\{1}", _database.Name, definition.Name));
      Storage.CreateFile(filename);
      return new Slice(filename);
    }
  }
}
