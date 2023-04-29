
namespace TallyDB.Core
{
  /// <summary>
  /// Utility methods and extensions
  /// </summary>
  internal static class Util
  {
    /// <summary>
    /// Sanitize filenames, folder names from invalid characters
    /// </summary>
    /// <param name="name">Original filename string</param>
    /// <returns>Sanitized filename string</returns>
    public static string SanitizeFilename(this string name)
    {
      char[] invalidChars = Path.GetInvalidFileNameChars();
      return string.Join("_", name.Split(invalidChars, StringSplitOptions.RemoveEmptyEntries));
    }
  }
}
