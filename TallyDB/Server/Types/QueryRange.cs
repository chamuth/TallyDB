using Newtonsoft.Json;

namespace TallyDB.Server.Types
{
  public class QueryRange
  {
    [JsonProperty("from")]
    public DateTime from { get; set; }
    [JsonProperty("to")]
    public DateTime to { get; set; }
    [JsonProperty("resolution")]
    public float Resolution { get; set; }
  }
}
