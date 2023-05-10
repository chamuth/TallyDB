namespace TallyDB.Core.Aggregation
{
  public class ComplexAggregator
  {
    private SliceDefinition definition;

    public ComplexAggregator(SliceDefinition definition)
    {
      this.definition = definition;
    }

    /// <summary>
    /// Aggregate each data in the slice record consulting the definition
    /// </summary>
    /// <param name="a">Initial slice record</param>
    public SliceRecord Aggregate(SliceRecord a, SliceRecordData[] b)
    {
      for (var i = 0; i < a.Data.Length; i++)
      {
        var aData = a.Data[i];
        var bData = b[i];

        var function = definition.Axes[i].Function;

        Aggregator aggregator = AggregatorHelper.Get(function);
        a.Data[i] = aggregator.Aggregate(aData, bData);
      }

      return a;
    }
  }
}
