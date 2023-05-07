
namespace TallyDB.Core
{
  public class SliceDefinition
  {
    public string Name { get; set; }
    public Axis[] Axes { get; set; }
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
  }
}
