using TallyDB.Server.Types;

namespace TallyDB.Server.QueryProcessor.Strategies
{
  public class QuerySlice : IProcessingStrategy
  {
    public QueryResponse Process(QueryRequest query)
    {
      var slice = query.Query?.Slice;
      var range = query.Query?.Range;

      var from = range?.from ?? DateTime.Now;
      var to = range?.to ?? DateTime.Now;

      // Find slice and read data
      var records = DatabaseManager.GetDatabase(slice?.Database ?? "").GetSlice(slice?.Name ?? "")
        .Query(from, to, 1);

      return new QueryResponse(query.RequestId)
      {
        Records = records
      };
    }
  }
}
