namespace TallyDB.Core.ByteConverters
{
  internal interface IByteConverter<T>
  {
    public byte[] Convert(T value);
  }
}
