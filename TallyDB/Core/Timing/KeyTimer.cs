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

    /// <summary>
    /// Get start and end time of the period the input time belongs to
    /// </summary>
    /// <param name="time">Input time</param>
    /// <param name="periodDuration">Period duration in hours</param>
    /// <returns>KeyPeriod containing start and end time</returns>
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

    /// <summary>
    /// Get start time of the current period
    /// </summary>
    /// <returns>DateTime start of the current period</returns>
    public DateTime GetCurrent()
    {
      return GetCurrentPeriodStartAndEnd(DateTime.Now, _definition.Frequency).Start;
    }

    /// <summary>
    /// Get the start time of the period for a given time
    /// </summary>
    /// <param name="date">Input time</param>
    /// <returns>DateTime start of the period</returns>
    public DateTime GetPeriodFor(DateTime date)
    {
      return GetCurrentPeriodStartAndEnd(date, _definition.Frequency).Start;
    }

    /// <summary>
    /// No of periods between two input times
    /// </summary>
    /// <param name="start">Start time</param>
    /// <param name="end">End time</param>
    /// <returns>No of periods between the two input times</returns>
    public int PeriodsBetween(DateTime start, DateTime end)
    {
      var periods = (end - start).TotalHours / _definition.Frequency;
      return (int)MathF.Floor((float)periods);
    }
  }
}
