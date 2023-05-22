namespace TallyDB.Mock
{
  public abstract class MockCreator
  {
    internal Random random;

    public MockCreator(int? seed)
    {
      if (seed != null)
      {
        random = new Random((int)seed);
      }
      else
      {
        random = new Random();
      }
    }
  }
}
