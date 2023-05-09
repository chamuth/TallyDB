using TallyDB.Core.Exceptions;

namespace TallyDB.Core.Aggregation
{
  public class SumAggregator : IAggregator
  {
    public SliceRecordData Aggregate(SliceRecordData a, SliceRecordData b)
    {
      if (a.Type != b.Type)
      {
        throw new IncompatibleDataTypesInAggregator(
          string.Format("Type {0} is not compatible with type {1} to aggregate", a.Type.ToString(), b.Type.ToString()));
      }

      try
      {
        var aValue = float.Parse(a.StringValue);
        var bValue = float.Parse(b.StringValue);
        var final = (aValue + bValue).ToString();

        return new SliceRecordData(a.Type, final);
      }
      catch (Exception)
      {
        throw new IncompatibleDataTypesInAggregator(
          string.Format("Non numerical type {0} cannot be used in SumAggregator", a.Type.ToString())
        );
      }
    }
  }
}
