namespace TallyDB.Core.ByteConverters
{
  public class FloatConverter : IByteConverter<float>
  {
    public float Decode(byte[] bytes)
    {
      return BitConverter.ToSingle(bytes);
    }

    public byte[] Encode(float value)
    {
      return BitConverter.GetBytes(value);
    }

    public int GetFixedLength()
    {
      return 4;
    }
  }
}
