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

    List<SliceRecord> cachedData = new List<SliceRecord>();

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
        var first = Last();
        var last = First();

        if (first == null || last == null)
        {
          return;
        }

        // Initialize cache storage for data entry
        cachedData = new List<SliceRecord>(_timer.PeriodsBetween(first.Time, last.Time));
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
    /// Get number of slice records saved in disk
    /// </summary>
    /// <returns>No of slice records saved</returns>
    private int GetSliceRecordCountInDisk()
    {
      if (_definition == null || _converter == null || !IsSliceDataAvailable())
      {
        return 0;
      }

      return (int)(_stream.Length - SliceHeaderConverter.GetLengthByAxisCount(_definition.Axes.Length)) / _converter.GetFixedLength();
    }

    /// <summary>
    /// Search disk and find slice record by period
    /// </summary>
    /// <param name="period"></param>
    /// <returns></returns>
    private int FindIndexInDisk(DateTime period)
    {
      if (_definition == null|| !IsSliceDataAvailable())
      {
        return -1;
      }

      // Hard load index if first or last
      if (period == cachedData.First().Time)
      {
        return 0;
      }
      if (period == cachedData.Last().Time)
      {
        return cachedData.Count - 1;
      }

      // Search disk binary
      var count = GetSliceRecordCountInDisk();

      //TODO: Implement search here
      return 0;
    }

    private SliceRecord[] GetWithinRange(DateTime startPeriod, DateTime endPeriod)
    {
      var firstRecord = First();
      if (firstRecord == null || _timer == null)
      {
        return new SliceRecord[] { };
      }

      var periodsCount = _timer.PeriodsBetween(startPeriod, endPeriod);
      var offsetIndex = _timer.PeriodsBetween(firstRecord.Time, startPeriod);

      // Check cache to see if we have the values within period
      if (cachedData[offsetIndex].Time == startPeriod)
      {

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
    public SliceRecord? Last()
    {
      if (cachedData.Last() != null)
      {
        // Return from cache
        return cachedData.Last();
      }

      if (_converter == null)
      {
        return null;
      }

      var sliceRecordLength = _converter.GetFixedLength();
      var sliceRecord = ReadRecordByIndexFromDisk(GetSliceRecordCountInDisk() - 1);

      if (sliceRecord == null)
      {
        return null;
      }

      cachedData[cachedData.Count - 1] = sliceRecord;
      return sliceRecord;
    }

    /// <summary>
    /// Get first written slice data from the disk
    /// </summary>
    /// <returns></returns>
    public SliceRecord? First()
    {
      if (cachedData[0] != null )
      {
        // Return from cache
        return cachedData[0];
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
      
      cachedData[0] = sliceRecord;
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

        return GetWithinRange(startPeriod, endPeriod);
      }
    }
  }
}
