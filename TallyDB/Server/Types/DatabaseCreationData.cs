using Newtonsoft.Json;

namespace TallyDB.Server.Types
{
  public class DatabaseCreationData
  {
    [JsonProperty("name")]
    public string Name { get; set; }

    public DatabaseCreationData(string name)
    {
      Name = name;
    }
  }
}
  