using System.Security.Cryptography;
using System.Text;

namespace TallyDB.Config.Auth
{
  public static class PasswordCrypto
  {
    private const int SaltLength = 256;

    /// <summary>
    /// Generates a new random salt string
    /// </summary>
    /// <returns>Salt string</returns>
    private static string GenerateSalt()
    {
      var rng = new Random();
      byte[] salt = new byte[SaltLength];
      rng.NextBytes(salt);
      return Convert.ToBase64String(salt);
    }

    /// <summary>
    /// Get hashed password for password string
    /// </summary>
    /// <param name="password">Password</param>
    /// <returns>hashed string</returns>
    private static string HashPassword(string password)
    {
      using (SHA256 sha256 = SHA256.Create())
      {
        byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
        byte[] hashBytes = sha256.ComputeHash(passwordBytes);
        string hashedPassword = Convert.ToBase64String(hashBytes);
        return hashedPassword;
      }
    }

    /// <summary>
    /// Verifies the passwords to saved password
    /// </summary>
    /// <param name="hash">Stored hashed password</param>
    /// <param name="password">Input password</param>
    /// <param name="salt">Salt</param>
    /// <returns>True if valid</returns>
    public static bool VerifyPassword(string hash, string password, string salt)
    {
      return string.Equals(HashPassword(password + salt), hash);
    }

    /// <summary>
    /// Get encrypted username password for user
    /// </summary>
    /// <param name="username">Username</param>
    /// <param name="password">Input password</param>
    /// <returns>User entity with password and salt populated</returns>
    public static User EncryptCredentials(string username, string password)
    {
      var salt = GenerateSalt();
      var hash = HashPassword(password + salt);
      return new User(username, hash, salt);
    }
  }
}
