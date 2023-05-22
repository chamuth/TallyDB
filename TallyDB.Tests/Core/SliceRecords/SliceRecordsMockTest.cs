
using TallyDB.Mock.Slice;

namespace TallyDB.Tests.Core.SliceRecords
{
  [TestClass]
  public class SliceRecordsMockTest
  {
    string sliceName = "mock1";
    string filename = Storage.Join("mock\\mock1");

    [TestInitialize]
    public void Prepare()
    {
      // Prepare the mock files
      if (File.Exists(filename))
      {
        File.Delete(filename);
      }
    }

    [TestMethod("Should create slice")]
    public void ShouldCreateSlice()
    {
      // Create new mocked slice
      var mocker = new MockSliceCreator();
      SliceDefinition def;
      SliceRecord[] records;
      mocker.Create(sliceName, out def, out records);

      // Load slice
      var storage = new SliceStorage(filename);
      var outputDef = storage.LoadSliceDefinition();

      // Assert equality
      Assert.AreEqual(def, outputDef);

      Assert.AreEqual(storage.First(), records.First());
      Assert.AreEqual(storage.Last(), records.Last());
    }
  }
}
