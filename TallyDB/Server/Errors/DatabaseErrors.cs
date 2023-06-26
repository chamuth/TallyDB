using TallyDB.Server.Types;

namespace TallyDB.Server.Errors
{
  public static class DatabaseErrors
  {
    public static DatabaseError DatabaseNotFoundError = new DatabaseError("DB001", "Database not found");
    public static DatabaseError DatabaseCreationFailedError = new DatabaseError("DB002", "Database creation failed");

    public static DatabaseError InvalidQueryInputError = new DatabaseError("IN001", "Invalid query input");

    public static DatabaseError SliceNotFoundError = new DatabaseError("SL001", "Slice not found");
  }
}
