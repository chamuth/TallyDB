
namespace TallyDB.Core
{
  internal class SliceDefinition
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
  }
}
