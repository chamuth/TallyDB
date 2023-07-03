using Newtonsoft.Json;

namespace TallyDB.Server.Types
{
  public class Credentials
  {
    [JsonProperty("username")]
    public string Username { get; set; }
    [JsonProperty("password")]
    public string Password { get; set; }
  }
}
