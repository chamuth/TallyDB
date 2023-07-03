
using Newtonsoft.Json;

namespace TallyDB.Core
{
  public class Axis
  {
    [JsonProperty("name")]
    public string Name;
    [JsonProperty("type")]
    public DataType Type;
    [JsonProperty("function")]
    public AggregateFunction Function;

    public Axis(string name, DataType type, AggregateFunction function)
    {
      Name = name;
      Type = type;
      Function = function;
    }
  }
}
