using TallyDB.Server.Types;

namespace TallyDB.Server.QueryProcessor
{
  public class RequestProcessor
  {
    public RequestProcessor() { }

    public QueryResponse Process(QueryRequest request)
    {
      return new QueryResponse(request.RequestId, "SLICE2", new Core.SliceRecord[] { });
    }
  }
}
