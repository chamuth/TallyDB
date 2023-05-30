using Newtonsoft.Json;
using TallyDB.Core;

namespace TallyDB.Server.Types
{
  public class Query
  {
    [JsonProperty("function")]
    public string Function { get; set; }

    [JsonProperty("records")]
    public SliceRecord[]? SliceRecord { get; set; }
    [JsonProperty("definition")]
    public SliceDefinition? Definition { get; set; }

    public Query(string function, SliceRecord[]? sliceRecord, SliceDefinition? definition)
    {
      Function = function;
      SliceRecord = sliceRecord;
      Definition = definition;
    }
  }
}
