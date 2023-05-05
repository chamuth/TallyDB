namespace TallyDB.Core.ByteConverters
{
  public class IntConverter : IByteConverter<int>
  {
    public int Decode(byte[] bytes)
    {
      return BitConverter.ToInt32(bytes);
    }

    public byte[] Encode(int value)
    {
      return BitConverter.GetBytes(value);
    }

    public int GetFixedLength()
    {
      return 4;
    }
  }
}
