using System.Text;

namespace TallyDB.Core.ByteConverters
{
  public class TextConverter : IByteConverter<string>
  {
    public string Decode(byte[] bytes)
    {
      // Find the index of the last non-zero byte
      int lastIndex = bytes.Length - 1;
      while (lastIndex >= 0 && bytes[lastIndex] == 0)
      {
        lastIndex--;
      }

      // Create a new byte array with the non-zero bytes
      byte[] trimmedArray = new byte[lastIndex + 1];
      Array.Copy(bytes, trimmedArray, lastIndex + 1);

      return Encoding.ASCII.GetString(trimmedArray);
    }

    public byte[] Encode(string value)
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
