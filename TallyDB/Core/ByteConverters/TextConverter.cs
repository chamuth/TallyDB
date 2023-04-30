using System.Text;

namespace TallyDB.Core.ByteConverters
{
  internal class TextConverter : IByteConverter<string>, IFixedLengthConverter
  {
    public byte[] Convert(string value)
    {
      byte[] axisName = Encoding.ASCII.GetBytes(value);
      Array.Resize(ref axisName, 32);
      return axisName;
    }

    public int GetFixedLength()
    {
      return 32;
    }
  }
}
