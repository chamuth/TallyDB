namespace TallyDB.Core
{
  public class SliceRecord
  {
    public DateTime Time { get; set; }
    public SliceRecordData[] Data { get; set; }

    public SliceRecord(SliceRecordData[] data, DateTime time)
    {
      Data = data;
      Time = time;
    }

    public override bool Equals(object? obj)
    {
      if (obj == null || GetType() != obj.GetType())
      {
        return false;
      }

      var sliceRec = (SliceRecord)obj;
      
      if (sliceRec.Time != this.Time)
      {
        return false;
      }

      if (sliceRec.Data.Length != Data.Length)
      {
        return false;
      }

      for (var i = 0; i < sliceRec.Data.Length; i++)
      {
        if (sliceRec.Data[i].Type != Data[i].Type
          || sliceRec.Data[i].StringValue != Data[i].StringValue)
        {
          return false;
        }
      }

      return true;
    }

    public override int GetHashCode()
    {
      return base.GetHashCode();
    }
  }
}
