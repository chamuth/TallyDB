using TallyDB.Server.Types;

namespace TallyDB.Server.QueryProcessor.Strategies
{
  public interface IProcessingStrategy
  {
    public QueryResponse Process(QueryRequest query);
  }
}