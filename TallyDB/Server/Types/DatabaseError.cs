using Newtonsoft.Json;

namespace TallyDB.Server.Types
{
    public class DatabaseError: Exception
    {
        [JsonProperty("errorCode")]
        public string ErrorCode { get; set; }
        [JsonProperty("message")]
        public override string Message { get; }
        [JsonProperty("description")]
        public string? Description { get; set; }

        public DatabaseError(string errorCode, string message, string? description = null)
        {
            ErrorCode = errorCode;
            Message = message;
            Description = description;
        }
    }
}
