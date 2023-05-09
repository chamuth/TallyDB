using System.Text;
using TallyDB.Core.ByteConverters;
using TallyDB.Core.ByteConverters.Util;
using TallyDB.Core.Timing;

namespace TallyDB.Core
{
  public class Slice
  {
    FileStream _stream;
    BinaryReader _reader;
    BinaryWriter _writer;

    SliceDefinition? _definition;
    SliceRecordConverter? _converter;
    KeyTimer? _timer;

    DateTime? lastWrittenKey;

    ~Slice()
    {
      _reader.Close();
      _writer.Close();
      _stream.Dispose();
    }

    public Slice(string filename, SliceDefinition? definition)
    {
      _definition = definition;

      // Initialize IO readers and writers
      _stream = new FileStream(string.Format("{0}.{1}", filename, Constants.TallyExtension), FileMode.Open);
      _reader = new BinaryReader(_stream, Encoding.UTF8);
      _writer = new BinaryWriter(_stream, Encoding.UTF8);

      // Instantiate converter
      if (definition != null)
      {
        _converter = new SliceRecordConverter(definition);
        _timer = new KeyTimer(definition);
      }

      // Load up last written key
      LoadLastWrittenKey();
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

    /// <summary>
    /// Loads up and decodes slice definitoin from slice file 
    /// </summary>
    public void LoadSliceDefinition()
    {
      // Read axis size byte
      const int AXIS_SIZE_BYTE_INDEX = 33;
      _stream.Seek(AXIS_SIZE_BYTE_INDEX, SeekOrigin.Begin);
      var axisCount = _reader.ReadByte();
      var length = SliceHeaderConverter.GetLengthByAxisCount(axisCount);

      // Read and decode slice definition length
      _stream.Seek(0, SeekOrigin.Begin);
      var bytes = _reader.ReadBytes(length);
      var converter = new SliceHeaderConverter();
      var sliceDef = converter.Decode(bytes);

      _definition = sliceDef;
    }

    /// <summary>
    /// Loads last written key from disk
    /// </summary>
    public void LoadLastWrittenKey()
    {
      if (_converter == null)
      {
        return;
      }

      // Load last written key
      var sliceRecordLength = _converter.GetFixedLength();
      _stream.Seek(sliceRecordLength, SeekOrigin.End);
      var converter = ByteConverter.GetForType<DateTime>();
      lastWrittenKey = converter.Decode(_reader.ReadBytes(converter.GetFixedLength()));
    }

    /// <summary>
    /// Report slice record to the slice
    /// </summary>
    /// <param name="data">slice record data arary</param>
    public void Report(SliceRecordData[] data)
    {
      if (_converter == null || _timer == null)
      {
        return;
      }

      var period = _timer.GetCurrent();

      SliceRecord record;
      if (period == lastWrittenKey)
      {
        record = new SliceRecord(data, (DateTime)lastWrittenKey);
      }
      else
      {
        record = new SliceRecord(data, _timer.GetCurrent());
      }

      var buffer = _converter.Encode(record);
      _stream.Seek(0, SeekOrigin.End);
      _writer.Write(buffer);
    }
  }
}
