using System.Text;
using TallyDB.Core.Aggregation;
using TallyDB.Core.ByteConverters;
using TallyDB.Core.Timing;

namespace TallyDB.Core
{
  /// <summary>
  /// Act as a proxy between Slice and Disk Storage
  /// </summary>
  public class SliceStorage
  {
    FileStream _stream;
    BinaryReader _reader;
    BinaryWriter _writer;

    SliceRecordConverter? _converter;
    KeyTimer? _timer;
    ComplexAggregator? _aggregator;

    SliceDefinition? _definition;

    SliceRecord? lastRecord;
    SliceRecord? firstRecord;

    List<SliceRecord> cachedData;

    ~SliceStorage()
    {
      _reader.Close();
      _writer.Close();
      _stream.Dispose();
    }

    public SliceStorage(string filename)
    {
      // Initialize IO readers and writers
      _stream = new FileStream(string.Format("{0}.{1}", filename, Constants.TallyExtension), FileMode.Open);
      _reader = new BinaryReader(_stream, Encoding.UTF8);
      _writer = new BinaryWriter(_stream, Encoding.UTF8);

    }

    #region PRIVATE METHODS
    /// <summary>
    /// Initialize slice definition dependent services
    /// </summary>
    private void InitializeDefinitionDependents()
    {
      if (_definition != null)
      {
        _converter = new SliceRecordConverter(_definition);
        _timer = new KeyTimer(_definition);
        _aggregator = new ComplexAggregator(_definition);

        // Load last and first records
        Last();
        First();

        // Initialize cache storage for data entry
        cachedData = new List<SliceRecord>(_timer.PeriodsBetween(First().Time, Last().Time));
      }
    }

    /// <summary>
    /// Returns true if slice data available
    /// </summary>
    /// <returns>true if Slice data available</returns>
    private bool IsSliceDataAvailable()
    {
      if (_definition == null)
      {
        return false;
      }

      return _stream.Length > SliceHeaderConverter.GetLengthByAxisCount(_definition.Axes.Length);
    }

    /// <summary>
    /// Read slice from disk using the start offset
    /// </summary>
    /// <param name="start">Offset in bytes</param>
    /// <returns></returns>
    private SliceRecord? ReadRecordFromDisk(long start)
    {
      if (_converter == null || _definition == null)
      {
        return null;
      }

      if (IsSliceDataAvailable())
      {
        // Load last written key
        _stream.Seek(start, SeekOrigin.Begin);
        var sliceRecordLength = _converter.GetFixedLength();
        return _converter.Decode(_reader.ReadBytes(sliceRecordLength));
      }

      return null;
    }
    #endregion

    /// <summary>
    /// Loads up and decodes slice definitoin from slice file 
    /// </summary>
    public SliceDefinition LoadSliceDefinition()
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
      InitializeDefinitionDependents();

      return sliceDef;
    }

    /// <summary>
    /// Stores and updates slice definitions into slice file
    /// </summary>
    public void SaveSliceDefinition(SliceDefinition _definition)
    {
      this._definition = _definition;

      var converter = new SliceHeaderConverter();
      byte[] buffer = converter.Encode(_definition);
      _writer.Write(buffer, 0, buffer.Length);
      _writer.Flush();

      InitializeDefinitionDependents();
    }

    /// <summary>
    /// Loads last written slice data from disk
    /// </summary>
    public SliceRecord Last()
    {
      if (lastRecord != null)
      {
        return lastRecord;
      }

      if (_converter == null)
      {
        return null;
      }

      var sliceRecordLength = _converter.GetFixedLength();
      lastRecord = ReadRecordFromDisk(_stream.Length - sliceRecordLength);
      return lastRecord;
    }

    /// <summary>
    /// Get first written slice data from the disk
    /// </summary>
    /// <returns></returns>
    public SliceRecord First()
    {
      if (firstRecord != null )
      {
        return firstRecord;
      }

      if (_definition == null || _converter == null)
      {
        return null;
      }

      var skip = SliceHeaderConverter.GetLengthByAxisCount(_definition.Axes.Length);
      firstRecord = ReadRecordFromDisk(skip);
      return firstRecord;
    }

    /// <summary>
    /// Insert slice record to the slice
    /// </summary>
    /// <param name="data">slice record data arary</param>
    public void Insert(SliceRecordData[] data)
    {
      if (_converter == null || _timer == null || _aggregator == null)
      {
        return;
      }

      var period = _timer.GetCurrent();

      SliceRecord record;

      if (lastRecord == null)
      {
        lastRecord = Last();
      }

      if (period == lastRecord?.Time)
      {
        // Aggregation
        record = _aggregator.Aggregate(lastRecord, data);
      }
      else
      {
        // Brand new record
        record = new SliceRecord(data, _timer.GetCurrent());
      }

      // Set last record
      lastRecord = record;

      // Write to buffer
      var buffer = _converter.Encode(record);
      _stream.Seek(0, SeekOrigin.End);
      _writer.Write(buffer);
      _writer.Flush();
    }

    /// <summary>
    /// Get slice record range between start and end time
    /// </summary>
    /// <param name="start">Start time of the time period</param>
    /// <param name="end">End time of the time period</param>
    /// <returns>Slice records between this time period</returns>
    public SliceRecord[] this[DateTime start, DateTime end]
    {
      get
      {
        List<SliceRecord> sliceRecords = new List<SliceRecord>();

        if (_timer == null)
        {
          return sliceRecords.ToArray();
        }

        var startPeriod = _timer.GetPeriodFor(start);
        var endPeriod = _timer.GetPeriodFor(end);

        if (startPeriod == endPeriod)
        {
          // Empty period given
          return sliceRecords.ToArray();
        }

        return sliceRecords.ToArray();
      }
    }
  }
}
