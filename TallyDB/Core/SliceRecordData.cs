namespace TallyDB.Core
{
  public class SliceRecordData
  {
    public DataType Type { get; set; }
    public string StringValue { get; set; }

    public SliceRecordData(DataType type, string stringValue)
    {
      Type = type;
      StringValue = stringValue;
    }
  }
}
