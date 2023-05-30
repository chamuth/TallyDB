using Newtonsoft.Json;

namespace TallyDB.Server.Types
{
  public class QueryRequest
  {
    [JsonProperty("requestId")]
    public string RequestId { get; set; }
    [JsonProperty("query")]
    public Query Query { get; set; }

    public QueryRequest(string requestId, Query query)
    {
      RequestId = requestId;
      Query = query;
    }
  }
}
