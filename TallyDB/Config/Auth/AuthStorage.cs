namespace TallyDB.Config.Auth
{
  public class AuthStorage : ConfigStorage<User>
  {
    public override string Path { get; set; } = "users";
  }
}
