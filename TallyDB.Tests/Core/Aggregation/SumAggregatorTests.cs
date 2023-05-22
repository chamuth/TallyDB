
using TallyDB.Core.Aggregation;
using TallyDB.Core.Exceptions;

namespace TallyDB.Tests.Core.Aggregation
{
  [TestClass]
  public class SumAggregatorTests
  {
    private SumAggregator aggregator;

    public SumAggregatorTests()
    {
      aggregator = new SumAggregator();
    }

    [TestMethod("Should sum integers")]
    public void Sum_Integers()
    {
      int a = 2;
      int b = 4;

      var final = aggregator.Aggregate(
        new SliceRecordData(DataType.INT, a.ToString()), 
        new SliceRecordData(DataType.INT, b.ToString())
      );
      Assert.AreEqual(final.StringValue, (a + b).ToString());
    }

    [TestMethod("Should throw on incompatible types")]
    [ExpectedException(typeof(IncompatibleDataTypesInAggregator))]
    public void Sum_IncompatibleTypes()
    {
      int a = 2;
      float b = 4.5f;

      aggregator.Aggregate(
        new SliceRecordData(DataType.INT, a.ToString()),
        new SliceRecordData(DataType.FLOAT, b.ToString())
      );
    }

    [TestMethod("Should throw on incompatible types")]
    [ExpectedException(typeof(IncompatibleDataTypesInAggregator))]
    public void Sum_NonNumericalType()
    {
      int a = 2;
      string b = "Test";

      aggregator.Aggregate(
        new SliceRecordData(DataType.INT, a.ToString()),
        new SliceRecordData(DataType.INT, b.ToString())
      );
    }
  }
}
