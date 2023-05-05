namespace TallyDB.Core.ByteConverters
{
  public interface IByteConverter<T>
  {
    public byte[] Encode(T value);
    public int GetFixedLength();
    public T Decode(byte[] bytes);
  }
}
