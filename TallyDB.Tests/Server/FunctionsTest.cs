using Newtonsoft.Json;
using TallyDB.Server;
using TallyDB.Server.QueryProcessor;
using TallyDB.Server.Types;

namespace TallyDB.Tests.Server
{
  [TestClass]
  public class FunctionsTest
  {
    QueryRequest queryRequest;
    RequestProcessor requestProcessor;
    string testDatabaseName = "TEST";

    public FunctionsTest()
    {
      // Load databases first
      DatabaseManager.LoadDatabases();

      var requestId = Guid.NewGuid().ToString();
      queryRequest = new QueryRequest(requestId, new Query())
      {
        RequestId = requestId,
      };

      requestProcessor = new RequestProcessor();
    }

    [TestInitialize]
    public void Initialize()
    {
      var requestId = Guid.NewGuid().ToString();
      queryRequest = new QueryRequest(requestId, new Query())
      {
        RequestId = requestId,
      };
    }

    [TestMethod]
    public void CreateDatabaseListDatabaseDeleteDatabase()
    {
      // Create database
      queryRequest.Query = new Query(QueryFunctionType.Create);
      queryRequest.Query.Database = new DatabaseCreationData(testDatabaseName);
      var createResponse = requestProcessor.Process(queryRequest);

      // List databases before delete
      Initialize();
      queryRequest.Query = new Query(QueryFunctionType.List);
      var beforeDelete = requestProcessor.Process(queryRequest);

      // Delete database
      Initialize();
      queryRequest.Query = new Query(QueryFunctionType.Delete);
      queryRequest.Query.Database = new DatabaseCreationData(testDatabaseName);
      requestProcessor.Process(queryRequest);

      // List databases after delete
      Initialize();
      queryRequest.Query = new Query(QueryFunctionType.List);
      var afterDelete = requestProcessor.Process(queryRequest);

      if (beforeDelete.Databases?.Count(x => x == testDatabaseName) != 1 || afterDelete.Databases?.Count(x => x == testDatabaseName) != 0)
      {
        Assert.Fail();
      }
    }
  }
}
