using Newtonsoft.Json;
using TallyDB.Core;

namespace TallyDB.Server.Types
{
  public class SliceCreationData
  {
    [JsonProperty("name")]
    public string? Name { get; set; }
    [JsonProperty("database")]
    public string? Database { get; set; }
    [JsonProperty("axes")]
    public Axis[]? Axes { get; set; }
    [JsonProperty("frequency")]
    public double? Frequency { get; set; }
    [JsonProperty("records")]
    public SliceRecordData[]? Records { get; set; }
  }
}
