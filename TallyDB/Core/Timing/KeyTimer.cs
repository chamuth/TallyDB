namespace TallyDB.Core.Timing
{
  /// <summary>
  /// Responsible for timing/period management
  /// </summary>
  public class KeyTimer
  {
    private SliceDefinition _definition;

    public KeyTimer(SliceDefinition definition)
    {
      _definition = definition;
    }

    private static KeyPeriod GetCurrentPeriodStartAndEnd(DateTime time, double periodDuration)
    {
      var unixEpoch = DateTime.UnixEpoch;
      TimeSpan timeSinceEpoch = time.ToUniversalTime() - unixEpoch;

      double elapsedPeriods = timeSinceEpoch.TotalHours / periodDuration;
      int currentPeriod = (int)Math.Floor(elapsedPeriods);

      var periodStart = unixEpoch.AddHours(currentPeriod * periodDuration);
      var periodEnd = periodStart.AddHours(periodDuration);

      return new KeyPeriod(periodStart, periodEnd);
    }

    public DateTime GetCurrent()
    {
      return GetCurrentPeriodStartAndEnd(DateTime.Now, _definition.Frequency).Start;
    }

    public DateTime GetPeriodFor(DateTime date)
    {
      return GetCurrentPeriodStartAndEnd(date, _definition.Frequency).Start;
    }

    public int PeriodsBetween(DateTime start, DateTime end)
    {
      var periods = (end - start).TotalHours / _definition.Frequency;
      return (int)MathF.Floor((float)periods);
    }
  }
}
