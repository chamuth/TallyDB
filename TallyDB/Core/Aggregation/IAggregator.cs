namespace TallyDB.Core.Aggregation
{
  public interface IAggregator
  {
    public SliceRecordData Aggregate(SliceRecordData a, SliceRecordData b);
  }
}
