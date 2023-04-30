using System.Text;

namespace TallyDB.Core
{
  internal class Slice
  {
    string _name;
    FileStream _stream;
    BinaryReader _reader;
    BinaryWriter _writer;

    ~Slice()
    {
      _reader.Close();
      _writer.Close();
      _stream.Dispose();
    }

    public Slice(string filename)
    {
      _name = Path.GetFileNameWithoutExtension(filename);

      // Initialize IO readers and writers
      _stream = new FileStream(string.Format("{0}.{1}", filename, Constants.TallyExtension), FileMode.Open);
      _reader = new BinaryReader(_stream, Encoding.UTF8);
      _writer = new BinaryWriter(_stream, Encoding.UTF8);
    }

    public BinaryWriter GetWriter()
    {
      return _writer;
    }
  }
}
