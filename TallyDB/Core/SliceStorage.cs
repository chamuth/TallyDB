using System.Security.Cryptography.X509Certificates;
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

    /// <summary>
    /// Cache container datasource
    /// </summary>
    List<SliceRecord> cachedData = new List<SliceRecord>();
    int cachedCount = Constants.DefaultCacheExtensionCount;

    SliceRecord? firstRecord;
    SliceRecord? lastRecord;

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

    #region Private Methods
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
        var first = First();
        var last = Last();

        if (first == null || last == null)
        {
          return;
        }

        // Fetch cache data and initialize cache data storage
        FetchCacheRecords();
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
    /// <returns>Null if slice data not available</returns>
    private SliceRecord? ReadRecordByIndexFromDisk(int index)
    {
      if (_converter == null || _definition == null)
      {
        return null;
      }

      if (IsSliceDataAvailable())
      {
        // Load last written key
        var headerSize = SliceHeaderConverter.GetLengthByAxisCount(_definition.Axes.Length);
        var sliceRecordLength = _converter.GetFixedLength();
        _stream.Seek(headerSize + index * sliceRecordLength, SeekOrigin.Begin);
        return _converter.Decode(_reader.ReadBytes(sliceRecordLength));
      }

      return null;
    }

    /// <summary>
    /// Read array of records from the disk with how many to read from the end.
    /// </summary>
    /// <param name="countFromLast">How many slice records to be read from the end</param>
    /// <param name="offset">How many to skip from last</param>
    /// <returns>Slice record array of count from end</returns>
    private SliceRecord[] ReadRecordArrayByIndexFromDisk(int countFromLast, int offset = 0)
    {
      var result = new SliceRecord[] { };

      if (_converter == null || _definition == null)
      {
        return result;
      }

      if (IsSliceDataAvailable())
      {
        // Select number of bytes to read if count is greater or less
        var count = GetSliceRecordCountInDisk();
        int sliceSize = _converter.GetFixedLength();
        var bytesToRead = ((count > countFromLast) ? countFromLast : count) * sliceSize;
        var skipFromLast = offset * sliceSize;

        // Read bytes and transfer into chunks
        _stream.Seek(-(bytesToRead + skipFromLast), SeekOrigin.End);
        var bytes = _reader.ReadBytes(bytesToRead);
        var byteChunks = bytes.SplitArray(_converter.GetFixedLength());

        result = byteChunks
          .Select((byteArray) => _converter.Decode(byteArray))
          .ToArray();
      }

      return result;
    }

    /// <summary>
    /// Get number of slice records saved in disk
    /// </summary>
    /// <returns>No of slice records saved</returns>
    private int GetSliceRecordCountInDisk()
    {
      if (_definition == null || _converter == null || !IsSliceDataAvailable())
      {
        return 0;
      }

      var size = _converter.GetFixedLength();
      var remainingLength = (_stream.Length - SliceHeaderConverter.GetLengthByAxisCount(_definition.Axes.Length));
      var value = remainingLength / size;

      return (int)value;
    }

    /// <summary>
    /// Read records from cache or prefetch cache if required
    /// </summary>
    /// <param name="startPeriod"></param>
    /// <param name="endPeriod"></param>
    /// <returns></returns>
    private SliceRecord[] GetWithinRange(DateTime startPeriod, DateTime endPeriod)
    {
      var result = new SliceRecord[] { };
      if (_timer == null)
      {
        return result;
      }

      if (cachedData.First().Time <= startPeriod)
      {
        // Required range overlaps cached range
        result = cachedData
          .Where((record) => record.Time >= startPeriod && record.Time <= endPeriod)
          .OrderBy(x => x.Time)
          .ToArray();
      }
      else
      {
        // Increase cache size and retry
        IncreaseCacheSize();
        result = GetWithinRange(startPeriod, endPeriod);
      }
      
      return result;
    }

    /// <summary>
    /// Increases cache size based on a predefined amount
    /// </summary>
    private void IncreaseCacheSize()
    {
      int previous = cachedCount;
      cachedCount += cachedCount;

      // Read only increased count skipping already loaded ones
      FetchCacheRecords(previous);
    }

    /// <summary>
    /// Fetch cache records from the disk and extend cache size (requires memory management implementation)
    /// <param name="skip"></param>
    /// </summary>
    private void FetchCacheRecords(int skip = 0)
    {
      if (_timer == null)
      {
        return;
      }

      cachedData = new List<SliceRecord>(ReadRecordArrayByIndexFromDisk(cachedCount, skip));
    }

    /// <summary>
    /// Get gap filled records for all periods within given timeframe
    /// </summary>
    /// <param name="from">Timeframe start</param>
    /// <param name="to">Timeframe end</param>
    /// <param name="dataset">Array of records</param>
    /// <returns>Gap filled slice records</returns>
    private SliceRecord[] GetGapFilledRecords(DateTime from, DateTime to, SliceRecord[] dataset)
    {
      var result = new List<SliceRecord>();
      
      if (_definition == null)
      {
        return result.ToArray();
      }

      while(from == to)
      {
        var record = dataset.Where(r => r.Time == from);

        if (record.Count() == 0) {
          // Add default slice record
          result.Add(new SliceRecord(new SliceRecordData[] { }, from));
          continue;
        }

        // Insert found record
        result.Add(record.First());
      }

      return result.ToArray();
    }
    #endregion

    /// <summary>
    /// Loads up and decodes slice definitoin from slice file 
    /// </summary>
    public SliceDefinition LoadSliceDefinition()
    {
      // Read axis size byte
      const int AXIS_SIZE_BYTE_INDEX = 32;
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
    public SliceRecord? Last()
    {
      if (lastRecord != null)
      {
        // Return from cache
        return lastRecord;
      }

      if (_converter == null)
      {
        return null;
      }

      var sliceRecordCount = GetSliceRecordCountInDisk() - 1;
      var sliceRecord = ReadRecordByIndexFromDisk(sliceRecordCount);

      if (sliceRecord == null)
      {
        return null;
      }

      lastRecord = sliceRecord;
      return sliceRecord;
    }

    /// <summary>
    /// Get first written slice data from the disk
    /// </summary>
    /// <returns></returns>
    public SliceRecord? First()
    {
      if (firstRecord != null )
      {
        // Return from cache
        return firstRecord;
      }

      if (_definition == null || _converter == null)
      {
        return null;
      }

      // Load first index slice form disk
      var sliceRecord = ReadRecordByIndexFromDisk(0);

      if (sliceRecord == null)
      {
        return null;
      }

      firstRecord = sliceRecord;
      return sliceRecord;
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

      var lastRecord = Last();

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
        if (_timer == null)
        {
          return new SliceRecord[] { };
        }

        var startPeriod = _timer.GetPeriodFor(start);
        var endPeriod = _timer.GetPeriodFor(end);

        if (startPeriod == endPeriod)
        {
          return new SliceRecord[] { };
        }

        var records = GetWithinRange(startPeriod, endPeriod);
        var gapFilledRecords = GetGapFilledRecords(startPeriod, endPeriod, records);

        return gapFilledRecords;
      }
    }
  }
}
