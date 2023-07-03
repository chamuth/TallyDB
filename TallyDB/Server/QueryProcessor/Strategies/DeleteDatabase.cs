using TallyDB.Server.Errors;
using TallyDB.Server.Types;

namespace TallyDB.Server.QueryProcessor.Strategies
{
  public class DeleteDatabase : IProcessingStrategy
  {
    public QueryResponse Process(QueryRequest query)
    {
      var name = query.Query?.Database?.Name;

      if (name == null)
      {
        throw DatabaseErrors.InvalidQueryInputError;
      }

      DatabaseManager.DeleteDatabase(name);
      return new QueryResponse(query.RequestId);
    }
  }
}
