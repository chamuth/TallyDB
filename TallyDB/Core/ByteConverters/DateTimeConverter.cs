namespace TallyDB.Core.ByteConverters
{
  public class DateTimeConverter : IByteConverter<DateTime>
  {
    public DateTime Decode(byte[] bytes)
    {
      return new DateTime(BitConverter.ToInt64(bytes));
    }

    public byte[] Encode(DateTime value)
    {
      return BitConverter.GetBytes(value.Ticks);
    }

    public int GetFixedLength()
    {
      return 8;
    }
  }
}
