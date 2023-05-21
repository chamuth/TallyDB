
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

    /// <summary>
    /// Split array equally by a divisible chunk size and return array of chunks
    /// </summary>
    /// <typeparam name="T">Array Type</typeparam>
    /// <param name="sourceArray">Source array</param>
    /// <param name="chunkSize">Size of a chunk</param>
    /// <returns>Array of chunks of given type</returns>
    public static T[][] SplitArray<T>(this T[] sourceArray, int chunkSize)
    {
      if (sourceArray.Length % chunkSize != 0)
      {
        // Source array should be perfectly divisible by chunk size
        throw new Exception("Array size is not a multiple of the provided chunk size");
      }

      int numChunks = (int)Math.Ceiling((double)sourceArray.Length / chunkSize);
      T[][] chunks = new T[numChunks][];

      for (int i = 0; i < numChunks; i++)
      {
        int startIndex = i * chunkSize;
        int remainingLength = Math.Min(chunkSize, sourceArray.Length - startIndex);
        T[] chunk = new T[remainingLength];
        Array.Copy(sourceArray, startIndex, chunk, 0, remainingLength);
        chunks[i] = chunk;
      }

      return chunks;
    }
  }
}
