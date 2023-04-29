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
      _stream.Dispose();
    }

    public Slice(string filename)
    {
      _name = Path.GetFileNameWithoutExtension(filename);

      // Initialize IO readers
      _stream = new FileStream(filename, FileMode.Open);
    }
  }
}
