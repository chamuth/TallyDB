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
        int? runnerValue = null;

        switch (axis.Type)
        {
          case DataType.TEXT:
            stringValue = bytes.DecodeByteSlice(ref skip, ByteConverter.GetForType<string>()).ToString();
            break;
          case DataType.INT:
            stringValue = bytes.DecodeByteSlice(ref skip, ByteConverter.GetForType<int>()).ToString();
            if (axis.Function == AggregateFunction.AVG)
            {
              runnerValue = bytes.DecodeByteSlice(ref skip, ByteConverter.GetForType<int>());
            }
            break;
          case DataType.FLOAT:
            stringValue = bytes.DecodeByteSlice(ref skip, new FloatConverter()).ToString();
            if (axis.Function == AggregateFunction.AVG)
            {
              runnerValue = bytes.DecodeByteSlice(ref skip, ByteConverter.GetForType<int>());
            }
            break;
        }

        var sliceRecord = new SliceRecordData(axis.Type, stringValue);
        
        if (runnerValue != null)
        {
          sliceRecord.RunnerValue = (int)runnerValue;
        }

        records.Add(sliceRecord);
      }

      return new SliceRecord(records.ToArray(), time);
    }

    public byte[] Encode(SliceRecord value)
    {
      List<byte> values = new List<byte>();

      var dateTimeConverter = ByteConverter.GetForType<DateTime>();
      byte[] dateTime = dateTimeConverter.Encode(value.Time);
      values.AddRange(dateTime);

      for (var i = 0; i < value.Data.Length; i++)
      {
        var datum = value.Data[i];
        var axis = _definition.Axes[i];

        if (datum.Type == DataType.TEXT)
        {
          var converter = ByteConverter.GetForType<string>();
          values.AddRange(converter.Encode(datum.StringValue));
        }
        else if (datum.Type == DataType.FLOAT)
        {
          var converter = ByteConverter.GetForType<float>();
          values.AddRange(converter.Encode(float.Parse(datum.StringValue)));

          if (axis.Function == AggregateFunction.AVG)
          {
            values.AddRange(ByteConverter.GetForType<int>().Encode(datum.RunnerValue));
          }
        }
        else if (datum.Type == DataType.INT)
        {
          var converter = ByteConverter.GetForType<int>();
          values.AddRange(converter.Encode(int.Parse(datum.StringValue)));

          if (axis.Function == AggregateFunction.AVG)
          {
            values.AddRange(ByteConverter.GetForType<int>().Encode(datum.RunnerValue));
          }
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

      return new DateTimeConverter().GetFixedLength() + _definition.Axes.Select((x) =>
      {
        var type = ByteConverter.TypeForDataType(x.Type);
        return ByteConverter.GetForType(type).GetFixedLength();
      }).Sum();
    }
  }
}
