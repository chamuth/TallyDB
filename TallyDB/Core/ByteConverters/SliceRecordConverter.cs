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
      }

      return dateTime;
    }
  }
}
