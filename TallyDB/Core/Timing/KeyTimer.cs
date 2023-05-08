namespace TallyDB.Core.Timing
{
  public class KeyTimer
  {
    private SliceDefinition _definition;

    public KeyTimer(SliceDefinition definition)
    {
      _definition = definition;
    }

    public DateTime GetCurrent()
    {
      // TODO: Associate starting timing to calculate current nearest period
      return DateTime.Now;
    }
  }
}
