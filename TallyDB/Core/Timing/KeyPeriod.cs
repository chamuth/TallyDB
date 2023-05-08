namespace TallyDB.Core.Timing
{
  public class KeyPeriod
  {
    public DateTime Start { get; set; }
    public DateTime End { get; set; }

    public KeyPeriod(DateTime start, DateTime end)
    {
      Start = start;
      End = end;
    }
  }
}
