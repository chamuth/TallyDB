using TallyDB.Core;
using TallyDB.Core.ByteConverters;
using TallyDB.Core.Timing;

namespace TallyDB.Mock.Slice
{
  public class MockSliceCreator : MockCreator
  {
    public MockSliceCreator(int? seed = null) : base(seed) { }

    private Axis[] GetRandomAxis(int length)
    {
      Axis[] axes = new Axis[length];

      for (var i = 0; i < length; i++)
      {
        DataType type = DataType.FLOAT;
        AggregateFunction function = AggregateFunction.SUM;
        axes[i] = new Axis("axis" + (i + 1).ToString(), type, function);
      }

      return axes;
    }

    private SliceRecord[] GetSliceRecordData(int count, SliceDefinition definition, DateTime startPeriod)
    {
      List<SliceRecord> records = new List<SliceRecord>();

      for (var i = 0; i < count; i ++)
      {
        List<SliceRecordData> data = new List<SliceRecordData>();
        for (var j = 0; j < definition.Axes.Count(); j++)
        {
          data.Add(new SliceRecordData(definition.Axes[j].Type, (random.NextSingle() * 100).ToString()));
        }

        records.Add(new SliceRecord(data.ToArray(), startPeriod + TimeSpan.FromHours(definition.Frequency * i)));
      }

      return records.ToArray();
    }

    public void Create(string name, out SliceDefinition def, out SliceRecord[] records)
    {
      var axesCount = 2;
      var frequency = random.Next(2, 5) * 0.5f;

      def = new SliceDefinition(name, GetRandomAxis(axesCount), frequency);
      var keyTimer = new KeyTimer(def);
      var startPeriod = keyTimer.GetPeriodFor(new DateTime(
        random.Next(2001, 2010), random.Next(1, 12), random.Next(2, 20),
        random.Next(1, 23), random.Next(0, 60), 0
      ));

      // Add slice data
      var sliceCount = random.Next(100, 200);
      records = GetSliceRecordData(sliceCount, def, startPeriod);

      // Encode
      var recordConverter = new SliceRecordConverter(def);
      var headerBytes = new SliceHeaderConverter().Encode(def);
      var recordBytes = records.Select((record) => recordConverter.Encode(record)).ToArray().ConcatTogether();

      var finalBytes = headerBytes.Concat(recordBytes).ToArray();

      var file = Storage.Join(string.Format("mock\\{0}.tally", def.Name));
      Storage.CreateDirectory(Storage.Join("mock\\"));
      File.WriteAllBytes(file, finalBytes);
    }
  }
}
