using TallyDB.Server.Errors;
using TallyDB.Server.Types;

namespace TallyDB.Server.QueryProcessor.Strategies
{
  public class ListSlices : IProcessingStrategy
  {
    public QueryResponse Process(QueryRequest query)
    {
      var database = query?.Query?.Database?.Name;

      if (query == null || database == null)
      {
        throw DatabaseErrors.InvalidQueryInputError;
      }

      return new QueryResponse(query.RequestId)
      {
        Slices = DatabaseManager.GetDatabase(database).GetAllSlices().Select(x => x.Name).ToArray()
      };
    }
  }
}
