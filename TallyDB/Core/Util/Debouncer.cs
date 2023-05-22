public static class Debouncer
{
  private static Timer? _timer;

  public static void Debounce(Action action, int delayMilliseconds = 300)
  {
    if (_timer != null)
    {
      _timer.Dispose();
    }

    _timer = new Timer(_ =>
    {
      action.Invoke();
      _timer?.Dispose();
      _timer = null;
    }, null, delayMilliseconds, Timeout.Infinite);
  }
}