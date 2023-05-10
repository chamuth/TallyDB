namespace TallyDB.Core.Exceptions
{
  public class IncompatibleDataTypesInAggregator : Exception
  {
    public IncompatibleDataTypesInAggregator()
    {
    }

    public IncompatibleDataTypesInAggregator(string message)
        : base(message)
    {
    }

    public IncompatibleDataTypesInAggregator(string message, Exception innerException)
        : base(message, innerException)
    {
    }
  }
}
