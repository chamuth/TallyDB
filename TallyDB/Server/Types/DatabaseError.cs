using Newtonsoft.Json;

namespace TallyDB.Server.Types
{
  public class DatabaseError
  {
    [JsonProperty("errorCode")]
    public string ErrorCode { get; set; }
    [JsonProperty("message")]
    public string Message { get; set; }
    [JsonProperty("description")]
    public string Descriptoin { get; set; }

    public DatabaseError(string errorCode, string message, string descriptoin)
    {
      ErrorCode = errorCode;
      Message = message;
      Descriptoin = descriptoin;
    }
  }
}
