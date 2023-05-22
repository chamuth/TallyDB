namespace TallyDB.Core.Aggregation
{
  public static class AggregatorHelper
  {
    static SumAggregator sumAggregator = new SumAggregator();
    static AverageAggregator averageAggregator = new AverageAggregator();

    public static Aggregator Get(AggregateFunction type)
    {
      switch(type)
      {
        case AggregateFunction.SUM:
          return sumAggregator;
        case AggregateFunction.AVG:
          return averageAggregator;
        default:
          return sumAggregator;
      }
    }
  }
}
