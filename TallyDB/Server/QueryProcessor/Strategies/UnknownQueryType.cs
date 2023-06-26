using TallyDB.Server.Types;

namespace TallyDB.Server.QueryProcessor.Strategies
{
    public class UnknownQueryType : IProcessingStrategy
  {
    public QueryResponse Process(QueryRequest request)
    {
      return new QueryResponse(request.RequestId)
      {
        Errors = new DatabaseError[]
        {
          new DatabaseError("1", "", "")
        }
      };
    }
  }
}
