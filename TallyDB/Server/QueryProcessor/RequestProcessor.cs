using TallyDB.Server.QueryProcessor.Strategies;
using TallyDB.Server.Types;

namespace TallyDB.Server.QueryProcessor
{
  public class RequestProcessor
  {
    public RequestProcessor() { }

    public QueryResponse Process(QueryRequest request)
    {
      if (request.Query == null)
      {
        throw new Exception("Query not provided");
      }

      IProcessingStrategy strategy = new UnknownQueryType();
      var query = request.Query;
      var function = query.Function;

      if (function == QueryFunctionType.Create)
      {
        if (query.Database != null)
        {
          strategy = new CreateDatabase();
        }
        else if (query.Slice != null)
        {
          strategy = new CreateSliceInsertRecords();
        }
        else if (query.Users != null)
        {
          strategy = new CreateUsers();
        }
      }
      else if (function == QueryFunctionType.Query)
      {
        if (query.Slice != null)
        {
          strategy = new QuerySlice();
        }
        else if (query.Database != null)
        {
          strategy = new QueryDatabase();
        }
      }
      else if (function == QueryFunctionType.Delete)
      {
        if (query.Database != null)
        {
          strategy = new DeleteDatabase();
        }
        else if (query.Slice != null)
        {
          strategy = new DeleteSlice();
        }
      }

      try
      {
        return strategy.Process(request);
      }
      catch (DatabaseError error)
      {
        return new QueryResponse(request.RequestId)
        {
          Errors = new DatabaseError[]
          {
            error
          }
        };
      }
    }
  }
}