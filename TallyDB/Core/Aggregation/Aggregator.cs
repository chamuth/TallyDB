using TallyDB.Core.Exceptions;

namespace TallyDB.Core.Aggregation
{
  public abstract class Aggregator
  {
    public void VerifyCompatibility(SliceRecordData a, SliceRecordData b)
    {
      if (a.Type != b.Type)
      {
        throw new IncompatibleDataTypesInAggregator(
          string.Format("Type {0} is not compatible with type {1} to aggregate", a.Type.ToString(), b.Type.ToString()));
      }
    }

    public abstract SliceRecordData Aggregate(SliceRecordData a, SliceRecordData b);
  }
}
