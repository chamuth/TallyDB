using System.Drawing;
using TallyDB.Core;
using TallyDB.Server.Errors;
using TallyDB.Server.Types;

namespace TallyDB.Server.QueryProcessor.Strategies
{
  /// <summary>
  /// Strategy to create a new database
  /// </summary>
  public class CreateDatabase : IProcessingStrategy
  {
    public QueryResponse Process(QueryRequest query)
    {
      var name = query.Query?.Database?.Name;
      if (name == null)
      {
        throw DatabaseErrors.InvalidQueryInputError;
      }

      DatabaseManager.CreateDatabase(name);

      return new QueryResponse(query.RequestId);
    }
  }
}