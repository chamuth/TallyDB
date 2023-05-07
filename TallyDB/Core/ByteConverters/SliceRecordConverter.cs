using TallyDB.Core.ByteConverters.Util;

namespace TallyDB.Core.ByteConverters
{
  public class SliceRecordConverter : IByteConverter<SliceRecord>
  {
    private SliceDefinition _definition;

    public SliceRecordConverter(SliceDefinition definition)
    {
      _definition = definition;
    }

    public SliceRecord Decode(byte[] bytes)
    {
      int skip = 0;

      // load time
      var dateTime = new DateTimeConverter();
      var time = bytes.DecodeByteSlice(ref skip, dateTime);

      // load records using slice definition
      List<SliceRecordData> records = new List<SliceRecordData>();
      foreach (var axis in _definition.Axes)
      {
        string stringValue = "";
        switch (axis.Type)
        {
          case DataType.TEXT:
            stringValue = bytes.DecodeByteSlice(ref skip, new TextConverter()).ToString();
            break;
          case DataType.INT:
            stringValue = bytes.DecodeByteSlice(ref skip, new IntConverter()).ToString();
            break;
          case DataType.FLOAT:
            stringValue = bytes.DecodeByteSlice(ref skip, new FloatConverter()).ToString();
            break;
        }

        records.Add(new SliceRecordData(axis.Type, stringValue));
      }

      return new SliceRecord(records.ToArray(), time);
    }

    public byte[] Encode(SliceRecord value)
    {
      List<byte> values = new List<byte>();

      var dateTimeConverter = new DateTimeConverter();
      byte[] dateTime = dateTimeConverter.Encode(value.Time);
      values.AddRange(dateTime);

      foreach(var datum in value.Data)
      {
        if (datum.Type == DataType.TEXT)
        {
          var converter = new TextConverter();
          values.AddRange(converter.Encode(datum.StringValue));
        }
        else if (datum.Type == DataType.FLOAT)
        {
          var converter = new FloatConverter();
          values.AddRange(converter.Encode(float.Parse(datum.StringValue)));
        }
        else if (datum.Type == DataType.INT)
        {
          var converter = new IntConverter();
          values.AddRange(converter.Encode(int.Parse(datum.StringValue)));
        }
      }

      return values.ToArray();
    }

    public int GetFixedLength()
    {
      if (_definition == null)
      {
        return 0;
      }

      // TODO 
      return 0;
    }
  }
}
