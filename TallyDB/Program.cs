using System.CommandLine;
using TallyDB.Server;

namespace TallyDB
{
    internal class Program
  {
    static async Task<int> Main(string[] args)
    {
      Console.WriteLine(Path.GetFileName("C:\\Options\\Textr.tst"));

      var portOptions = new Option<int>(
          name: "--port",
          description: "Port to start TallyDB");

      var rootCommand = new RootCommand("TallyDB");
      rootCommand.AddOption(portOptions);

      rootCommand.SetHandler((port) =>
      {
        TallyServer tally = new TallyServer();
        tally.StartServer();
      }, portOptions);

      return await rootCommand.InvokeAsync(args);
    }
  }
}