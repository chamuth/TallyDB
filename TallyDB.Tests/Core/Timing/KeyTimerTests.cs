using TallyDB.Core.Timing;

namespace TallyDB.Tests.Core.Timing
{
  [TestClass]
  public class KeyTimerTests
  {
    [TestMethod("Should work with 24 hours period")]
    public void KeyTimer_24HourCase()
    {
      var kt = new KeyTimer(new SliceDefinition("EXAMPLE", new Axis[] { }, 24));
      var start = kt.GetCurrent();
      var expected = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
      Assert.AreEqual(expected.ToString(), start.ToString());
    }

    [TestMethod("Should work with 30 minutes period")]
    public void KeyTimer_30MinutesCase()
    {
      var kt = new KeyTimer(new SliceDefinition("EXAMPLE", new Axis[] { }, 0.5d));
      var start = kt.GetCurrent();
      var now = DateTime.Now;
      var expected = now.AddMinutes(now.Minute > 30 ? -(now.Minute - 30) : -now.Minute).AddSeconds(-now.Second).ToUniversalTime();
      Assert.AreEqual(expected.ToString(), start.ToString());
    }
  }
}
