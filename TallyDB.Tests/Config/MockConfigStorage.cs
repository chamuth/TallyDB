using Newtonsoft.Json;
using TallyDB.Config;

namespace TallyDB.Tests.Config
{
  public class MockConfigType
  {
    [JsonProperty("name")]
    public string Name;

    public MockConfigType(string name)
    {
      Name = name;
    }
  }

  public class MockConfigStorage : ConfigStorage<MockConfigType>
  {
    public override string Path { get; set; } = "Path";
    public MockConfigStorage(IFileOperable file) : base(file) { }
  }
}
