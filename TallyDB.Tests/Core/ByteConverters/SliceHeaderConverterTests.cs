

using TallyDB.Core.ByteConverters;

namespace TallyDB.Tests.Core.ByteConverters
{
  [TestClass]
  public class SliceHeaderConverterTests
  {
    private readonly SliceHeaderConverter _sliceHeaderConverter;

    public SliceHeaderConverterTests()
    {
      _sliceHeaderConverter = new SliceHeaderConverter();
    }

    [TestMethod("Should convert for best case scenario")]
    public void EncodeDecode_ShouldConvertBestCaseSlice()
    {
      var name = "ExampleSlice";
      var axes = new Axis[]
      {
        new Axis("Axis1", DataType.INT, AggregateFunction.SUM)
      };
      double frequency = 0.25;

      var input = new SliceDefinition(
        name, axes, frequency
      );

      var bytes = _sliceHeaderConverter.Encode(input);
      var output = _sliceHeaderConverter.Decode(bytes);

      Assert.AreEqual(input, output);
    }

    [TestMethod("Should convert empty slice definitions")]
    public void EncodeDecode_ShouldConvertEmptySliceDefinition()
    {
      var name = "ExampleSlice";
      var axes = new Axis[] { };
      double frequency = 0.25;

      var input = new SliceDefinition(
        name, axes, frequency
      );

      var bytes = _sliceHeaderConverter.Encode(input);
      var output = _sliceHeaderConverter.Decode(bytes);

      Assert.AreEqual(input, output);
    }
  }
}
