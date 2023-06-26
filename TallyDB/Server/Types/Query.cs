using Newtonsoft.Json;

namespace TallyDB.Server.Types
{
  /// <summary>
  /// Represents a query object
  /// Combinations of function and object properties determines what action server performs.
  /// </summary>
  public class Query
  {
    [JsonProperty("function")]
    public string? Function { get; set; }

    [JsonProperty("slice")]
    public SliceCreationData? Slice { get; set; }
    [JsonProperty("database")] 
    public DatabaseCreationData? Database { get; set; }
    [JsonProperty("range")]
    public QueryRange? Range { get; set; }
    [JsonProperty("users")]
    public UserCreationData? Users { get; set; }

    public Query(string? function = null)
    {
      Function = function;
    }
  }
}
