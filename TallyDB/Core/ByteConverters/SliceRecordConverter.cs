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
      throw new NotImplementedException();
    }

    public byte[] Encode(SliceRecord value)
    {
      var dateTimeConverter = new DateTimeConverter();
      byte[] dateTime = dateTimeConverter.Encode(value.Time);
      List<byte> values = new List<byte>();

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

      return dateTime;
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
