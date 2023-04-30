namespace TallyDB.Core
{
  public class SliceRecord
  {
    public DateTime Time { get; set; }
    public SliceRecordData[] Data { get; set; }

    public SliceRecord(SliceRecordData[] data)
    {
      Data = data;
      Time = DateTime.Now;
    }
  }
}
