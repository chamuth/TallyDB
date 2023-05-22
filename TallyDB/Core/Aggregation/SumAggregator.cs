using TallyDB.Core.Exceptions;

namespace TallyDB.Core.Aggregation
{
  public class SumAggregator : Aggregator
  {
    public override SliceRecordData Aggregate(SliceRecordData a, SliceRecordData b)
    {
      VerifyCompatibility(a, b);

      try
      {
        var aValue = float.Parse(a.StringValue);
        var bValue = float.Parse(b.StringValue);
        var final = (aValue + bValue).ToString();

        a.StringValue = final;

        return a;
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
