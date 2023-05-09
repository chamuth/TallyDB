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
      var frequency = _definition.Frequency;
      return GetCurrentPeriodStartAndEnd(DateTime.Now, frequency).Start;
    }
  }
}
