namespace TallyDB.Core.ByteConverters
{
  public static class ByteConverter
  {
    private static TextConverter textConverter = new TextConverter();
    private static IntConverter intConverter = new IntConverter();
    private static FloatConverter floatConverter = new FloatConverter();
    private static DateTimeConverter dateTimeConverter = new DateTimeConverter();

    public static IByteConverter<T> GetForType<T>(T? test = default)
    {
      if (typeof(T) == typeof(string))
      {
        return (IByteConverter<T>)textConverter;
      }

      if (typeof(T) == typeof(float))
      {
        return (IByteConverter<T>)floatConverter;
      }

      if (typeof(T) == typeof(int))
      {
        return (IByteConverter<T>)intConverter;
      }

      if (typeof(T) == typeof(DateTime))
      {
        return (IByteConverter<T>)dateTimeConverter;
      }

      throw new Exception();
    }

    public static int GetFixedLengthForType(DataType type)
    {
      if (type == DataType.TEXT)
      {
        return textConverter.GetFixedLength();
      }
      else if (type == DataType.INT)
      {
        return intConverter.GetFixedLength();
      }
      else if (type == DataType.FLOAT)
      {
        return floatConverter.GetFixedLength();
      }

      throw new Exception("Unknown DataType input for GetFixedLengthForType");
    }

    public static Type TypeForDataType(DataType type)
    {
      if (type == DataType.TEXT)
      {
        return typeof(string);
      }
      else if (type == DataType.INT)
      {
        return typeof(int);
      }
      else if (type == DataType.FLOAT)
      {
        return typeof(float);
      }

      return typeof(string);
    }
  }
}
