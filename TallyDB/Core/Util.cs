
namespace TallyDB.Core
{
  /// <summary>
  /// Utility methods and extensions
  /// </summary>
  public static class Util
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

    /// <summary>
    /// Appends extension name
    /// </summary>
    /// <param name="filename">Original filename</param>
    /// <returns>Filename with new extension</returns>
    public static string AppendExtension(this string filename)
    {
      return string.Format("{0}.{1}", filename, Constants.TallyExtension);
    }
  }
}
