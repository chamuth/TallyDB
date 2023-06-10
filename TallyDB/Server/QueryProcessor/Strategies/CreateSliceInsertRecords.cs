using TallyDB.Server.Errors;
using TallyDB.Server.Types;

namespace TallyDB.Server.QueryProcessor.Strategies
{
  public class CreateSliceInsertRecords : IProcessingStrategy
  {
    public QueryResponse Process(QueryRequest query)
    {
      var slice = query.Query?.Slice;

      // Slice creation without records
      var sliceCreationOnly = slice != null && slice?.Name != null && slice?.Database != null && slice?.Axes != null && slice?.Frequency != null && slice?.Records == null;
      var addSliceRecords = slice != null && slice?.Name != null && slice?.Database != null && slice?.Records != null;

      if (!(sliceCreationOnly || addSliceRecords))
      {
        throw DatabaseErrors.InvalidQueryInputError;
      }

      if (sliceCreationOnly)
      {
        // Create slice
      } 
      else if (addSliceRecords)
      {
        // Add slice records
      }

      // Slice Definition
      throw DatabaseErrors.InvalidQueryInputError;
    }
  }
}
