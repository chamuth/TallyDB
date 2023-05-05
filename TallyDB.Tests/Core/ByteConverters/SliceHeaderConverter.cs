

namespace TallyDB.Tests.Core.ByteConverters
{
  [TestClass]
  public class SliceHeaderConverter
  {
    private readonly TallyDB.Core.ByteConverters.SliceHeaderConverter _sliceHeaderConverter;

    public SliceHeaderConverter()
    {
      _sliceHeaderConverter = new TallyDB.Core.ByteConverters.SliceHeaderConverter();
    }

    [TestMethod]
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

    [TestMethod]
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
