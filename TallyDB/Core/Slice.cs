using System.IO;
using System.Text;
using TallyDB.Core.ByteConverters;

namespace TallyDB.Core
{
  public class Slice
  {
    string _name;
    FileStream _stream;
    BinaryReader _reader;
    BinaryWriter _writer;

    SliceDefinition? _definition;
    SliceRecordConverter? converter;

    ~Slice()
    {
      _reader.Close();
      _writer.Close();
      _stream.Dispose();
    }

    public Slice(string filename, SliceDefinition? definition)
    {
      _definition = definition;

      _name = Path.GetFileNameWithoutExtension(filename);

      // Initialize IO readers and writers
      _stream = new FileStream(string.Format("{0}.{1}", filename, Constants.TallyExtension), FileMode.Open);
      _reader = new BinaryReader(_stream, Encoding.UTF8);
      _writer = new BinaryWriter(_stream, Encoding.UTF8);

      // Instantiate converter
      if (definition != null)
      {
        converter = new SliceRecordConverter(definition);
      }
    }

    /// <summary>
    /// Stores and updates slice definitions into slice file
    /// </summary>
    public void UpdateSliceDefinition()
    {
      if (_definition == null)
      {
        return;
      }

      var converter = new SliceHeaderConverter();
      byte[] buffer = converter.Encode(_definition);
      _writer.Write(buffer, 0, buffer.Length);
      _writer.Flush();
    }

    public void Report(SliceRecord record)
    {
      if (converter == null)
      {
        return;
      }

      // Write to end of slice, TODO: Implement timing functions and etc.
      var buffer = converter.Encode(record);
      _stream.Seek(0, SeekOrigin.End);
      _writer.Write(buffer);
    }
  }
}
