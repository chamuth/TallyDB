
using Newtonsoft.Json;

namespace TallyDB.Core
{
  public class SliceDefinition
  {
    [JsonProperty("name")]
    public string Name { get; set; }
    [JsonProperty("axes")]
    public Axis[] Axes { get; set; }
    [JsonProperty("frequency")]
    public double Frequency { get; set; }

    public SliceDefinition(string name, Axis[] axes, double frequency)
    {
      Name = name;
      Axes = axes;
      Frequency = frequency;
    }

    public override bool Equals(object? obj)
    {
      if (obj == null || GetType() != obj.GetType())
      {
        return false;
      }

      var other = (SliceDefinition)obj;

      if (other.Name != this.Name || other.Frequency != this.Frequency)
      {
        return false;
      }

      for(var i= 0; i < other.Axes.Length;i ++)
      {
        if (!(other.Axes[i].Name == Axes[i].Name && other.Axes[i].Type == Axes[i].Type && other.Axes[i].Function == Axes[i].Function))
        { 
          return false;
        }
      }

      return true;
    }

    public override int GetHashCode()
    {
      int hash = 17;
      hash = hash * 23 + Name.GetHashCode();
      hash = hash * 23 + Axes.Length;
      hash = hash * 23 + Frequency.GetHashCode();
      return hash;
    }
  }
}
