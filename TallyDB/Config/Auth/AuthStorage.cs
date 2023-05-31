namespace TallyDB.Config.Auth
{
  public class AuthStorage : ConfigStorage<User>
  {
    public AuthStorage(IFileOperable file) : base(file) { }

    public override string Path { get; set; } = "users.tallyc";

    /// <summary>
    /// Authenticate using username password and return autheticated user. Returns null if invalid credentials
    /// </summary>
    /// <param name="username">Username</param>
    /// <param name="password">Password</param>
    /// <returns>User if authenticated</returns>
    public User? Authenticate(string username, string password)
    {
      // Check if user exists
      var users = GetAll();

      // if no users available create default user on server
      if (users.Length == 0)
      {
        CreateDefaultUser();
        users = GetAll();
      }

      var user = users.FirstOrDefault(user => user.Username == username);
      if (user == null)
      {
        return null;
      }

      // Verify input password to password
      var verify = PasswordCrypto.VerifyPassword(user.Password, password, user.Salt);

      if (verify)
      {
        // Update user's last logged in time
        user.LastLoggedIn = DateTime.Now;
        Save(user, (u) => u.Username == user.Username);

        return user;
      }

      return null;
    }

    /// <summary>
    /// Create the default user first time
    /// </summary>
    private void CreateDefaultUser()
    {
      CreateUser("username", "password", new string[] { });
    }

    /// <summary>
    /// Create new user and save to disk
    /// </summary>
    /// <param name="username">Username</param>
    /// <param name="password">Password</param>
    /// <param name="permissions">Permissions assigned to the new user</param>
    /// <returns>Saved user entity</returns>
    public User? CreateUser(string username, string password, string[] permissions)
    {
      // Check if username already exists
      var users = GetAll();
      var user = users.FirstOrDefault(user => user.Username == username);

      if (user != null)
      {
        return null;
      }

      // Encrypt user credentials
      var newUser = PasswordCrypto.EncryptCredentials(username, password);
      newUser.Permissions = permissions;

      // Store user info on disk
      Insert(newUser);

      // Return newly created user 
      return newUser;
    }
  }
}
