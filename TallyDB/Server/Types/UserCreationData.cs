using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TallyDB.Server.Types
{
  public class UserCreationData
  {
    [JsonProperty("username")]
    public string Username { get; set; }

    [JsonProperty("username")]
    public string Password { get; set; }

    [JsonProperty("permissions")]
    public string[] Permissions { get; set; }

    public UserCreationData(string username, string password, string[] permissions)
    {
      Username = username;
      Password = password;
      Permissions = permissions;
    }
  }
}
