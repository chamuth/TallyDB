using TallyDB.Core;
using TallyDB.Server;

namespace TallyDB
{
  public class Program
  {
    static void Main(string[] args)
    {
      new TallyServer().StartServer();
    }
  }
}