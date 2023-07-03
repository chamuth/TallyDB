using static System.Environment;

namespace TallyDB.Core
{
  public static class Constants
  {
    public const string TallyExtension = "tally";
    public const int DefaultCacheExtensionCount = 100;
    public const string DefaultUsername = "username";
    public const string DefaultPassword = "password";

#if ENV_DOCKER
    public static string TallyRoot = Path.Join(GetFolderPath(SpecialFolder.CommonApplicationData), "TallyDB/");
#else
    public static string TallyRoot = Path.Join(GetFolderPath(SpecialFolder.ApplicationData), "TallyDB/");
#endif
  }
}
