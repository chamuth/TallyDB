namespace TallyDB.Core.ByteConverters
{
  public class FloatConverter : IByteConverter<double>
  {
    public double Decode(byte[] bytes)
    {
      return BitConverter.ToDouble(bytes);
    }

    public byte[] Encode(double value)
    {
      return BitConverter.GetBytes(value);
    }

    public int GetFixedLength()
    {
      return 4;
    }
  }
}
