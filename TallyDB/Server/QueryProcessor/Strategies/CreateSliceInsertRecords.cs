using TallyDB.Core;
using TallyDB.Server.Errors;
using TallyDB.Server.Types;

namespace TallyDB.Server.QueryProcessor.Strategies
{
  public class CreateSliceInsertRecords : IProcessingStrategy
  {
    public QueryResponse Process(QueryRequest query)
    {
      var slice = query.Query?.Slice;
      var database = query.Query?.Database;

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
        var sliceCreator = new SliceCreator(DatabaseManager.GetDatabase(slice?.Database ?? ""));
        var createdSlice = sliceCreator.Create(new SliceDefinition(slice?.Name ?? "", slice?.Axes ?? new Axis[] { }, slice?.Frequency ?? 0));

        return new QueryResponse(query.RequestId)
        {
          SliceName = createdSlice.Name
        };
      } 
      else if (addSliceRecords)
      {
        // Add slice records
        var _slice = DatabaseManager.GetDatabase(database?.Name ?? "").GetSlice(slice?.Name ?? "");
        _slice.Insert(slice?.Records ?? new SliceRecordData[] { });
        return new QueryResponse(query.RequestId);
      }

      // Slice Definition
      throw DatabaseErrors.InvalidQueryInputError;
    }
  }
}
