using TallyDB.Core;
using TallyDB.Mock.Slice;

namespace TallyDB.Mock
{
  internal class Program
  {
    static void Main(string[] args)
    {
      // Prepare the mock files
      var sliceName = "mock1";
      var filename = Storage.Join("mock\\" + sliceName);
      if (File.Exists(filename))
      {
        File.Delete(filename);
      }

      // Create new mocked slice
      var mocker = new MockSliceCreator();
      SliceDefinition def;
      SliceRecord[] records;
      mocker.Create("mock1", out def, out records);

      // Load slice
      var storage = new SliceStorage(filename);
      storage.LoadSliceDefinition();

      //var first = storage.First();

      Console.WriteLine("TEST");
    }
  }
}