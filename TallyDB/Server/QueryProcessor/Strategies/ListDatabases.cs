using TallyDB.Server.Errors;
using TallyDB.Server.Types;

namespace TallyDB.Server.QueryProcessor.Strategies
{
  public class ListDatabases : IProcessingStrategy
  {
    public QueryResponse Process(QueryRequest query)
    {
      return new QueryResponse(query.RequestId)
      {
        Databases = DatabaseManager.GetDatabases().Select(x => x.Name).ToArray()
      };
    }
  }
}
