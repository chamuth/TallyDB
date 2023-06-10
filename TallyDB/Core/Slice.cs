namespace TallyDB.Core
{
  public class Slice
  {
    SliceStorage storage;
    public string Name { get; }

    public Slice(string filename)
    {
      Name = Path.GetFileNameWithoutExtension(filename);
      storage = new SliceStorage(filename);
    }

    /// <summary>
    /// Create new Slice using provided slice definition
    /// </summary>
    /// <param name="newDefinition">New Slice definition</param>
    public void Create(SliceDefinition newDefinition)
    {
      storage.SaveSliceDefinition(newDefinition);
    }

    /// <summary>
    /// Load slice definition from the disk
    /// </summary>
    public void Load()
    {
      storage.LoadSliceDefinition();
    }

    /// <summary>
    /// Query slice data based on period
    /// </summary>
    /// <param name="start">Start of the render period</param>
    /// <param name="end">End of the render period</param>
    /// <param name="resolution">Minimum resolution as a multiply of period time of the slice. If period time of the slice is 1 hour and resolution is set as 2, the length between two points in results is 2 hours.</param>
    public void Query(DateTime start, DateTime end, float resolution)
    {

    }

    public void Insert(SliceRecordData[] data)
    {
      storage.Insert(data);
    }
  }
}
