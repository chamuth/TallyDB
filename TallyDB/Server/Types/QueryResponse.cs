using Newtonsoft.Json;
using TallyDB.Core;

namespace TallyDB.Server.Types
{
  public class QueryResponse
  {
    [JsonProperty("requestId")]
    public string RequestId { get; set; }
    [JsonProperty("sliceName")]
    public string SliceName { get; set; }
    [JsonProperty("records")]
    public SliceRecord[] Records { get; set; }

    public QueryResponse(string requestId, string sliceName, SliceRecord[] records)
    {
      RequestId = requestId;
      SliceName = sliceName;
      Records = records;
    }
  }
}
