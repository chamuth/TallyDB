using TallyDB.Core.ByteConverters;

namespace TallyDB.Tests.Core.ByteConverters
{
  [TestClass]
  public class SliceRecordConverterTests
  {
    private readonly SliceRecordConverter sliceRecordConverter;

    public SliceRecordConverterTests()
    {
      var definition = new SliceDefinition(
        "example", new Axis[]
        {
          new Axis("reports", DataType.INT, AggregateFunction.SUM)
        },
        1 / 10f
      );

      sliceRecordConverter = new SliceRecordConverter(definition);
    }

    [TestMethod("Should convert best case scenario")]
    public void EncodeDecode_ShouldConvertBestCase()
    {
      var sliceRecord = new SliceRecord(new SliceRecordData[]
      {
        new SliceRecordData(DataType.INT, "22")
      }, DateTime.Now);

      var input = sliceRecordConverter.Encode(sliceRecord);
      var output = sliceRecordConverter.Decode(input);

      Assert.AreEqual(sliceRecord, output);
    }

    [TestMethod("Should throw for slice definition mismatch")]
    [ExpectedException(typeof(ArgumentOutOfRangeException))]
    public void EncodeDecode_ShouldThrowForDefinitionMismatch()
    {
      var sliceRecord = new SliceRecord(
        new SliceRecordData[] { }
      , DateTime.Now);

      var input = sliceRecordConverter.Encode(sliceRecord);
      sliceRecordConverter.Decode(input);
    }

    [TestMethod("Should consider averaging count dataset")]
    public void EncodeDecode_ShouldConsiderAveragingCount()
    {
      var definition = new SliceRecordConverter(new SliceDefinition("NEW", new Axis[]
      {
        new Axis("temperature", DataType.FLOAT, AggregateFunction.AVG)
      }, 1));

      var input = new SliceRecord(new SliceRecordData[] { new SliceRecordData(DataType.FLOAT, "1.24") }, DateTime.Now);
      var bytes = definition.Encode(input);
      var output = definition.Decode(bytes);

      Assert.AreEqual(input, output);
    }
  }
}
