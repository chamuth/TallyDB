using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TallyDB.Core.ByteConverters;

namespace TallyDB.Tests.Core.ByteConverters
{
  [TestClass]
  public class SliceRecordConverter
  {
    private readonly TallyDB.Core.ByteConverters.SliceRecordConverter sliceRecordConverter;

    public SliceRecordConverter()
    {
      var definition = new SliceDefinition(
        "example", new Axis[]
        {
          new Axis("reports", DataType.INT, AggregateFunction.SUM)
        },
        1 / 10f
      );

      sliceRecordConverter = new TallyDB.Core.ByteConverters.SliceRecordConverter(definition);
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
  }
}
