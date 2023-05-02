namespace TallyDB.Core.ByteConverters
{
  public interface IByteConverter<T>
  {
    public byte[] Encode(T value);
    public T Decode(byte[] bytes);
  }
}
