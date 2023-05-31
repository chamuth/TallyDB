using Newtonsoft.Json;

namespace TallyDB.Config.Auth
{
  public class User
  {
    [JsonProperty("username")]
    public string Username { get; set; }
    [JsonProperty("password")]
    public string Password { get; set; }
    [JsonProperty("salt")]
    public string Salt { get; set; }
    [JsonProperty("permissions")]
    public string[]? Permissions { get; set; }
    [JsonProperty("lastLoggedIn")]
    public DateTime? LastLoggedIn { get; set; }

    public User(string username, string password, string salt)
    {
      Username = username;
      Password = password;
      Salt = salt;
    }
  }
}
