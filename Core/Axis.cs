
namespace TallyDB.Core
{
  internal class Axis
  {
    public string Name;
    public DataType Type;
    public AggregateFunction Function;

    public Axis(string name, DataType type, AggregateFunction function)
    {
      Name = name;
      Type = type;
      Function = function;
    }
  }
}
