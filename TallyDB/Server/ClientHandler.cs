using Newtonsoft.Json;
using System.Net.Sockets;
using System.Text;
using TallyDB.Config;
using TallyDB.Config.Auth;
using TallyDB.Server.QueryProcessor;
using TallyDB.Server.Types;

namespace TallyDB.Server
{
  public static class ClientHandler
  {
    /// <summary>
    /// Handle each client being connected to the server
    /// </summary>
    /// <param name="client">client socket connection</param>
    public static void Handle(Socket client)
    {
      var authStorage = new AuthStorage(new FileOperator());
      var requestProcessor = new RequestProcessor();

      while (true)
      {
        byte[] data = new byte[100];
        int size = client.Receive(data);

        string value = "";

        for (int i = 0; i < size; i++)
        {
          value += (Convert.ToChar(data[i]));
        }

        var request = JsonConvert.DeserializeObject<QueryRequest>(value);

        if (request != null)
        {
          Console.WriteLine("Request received: {0}", value);
          var response = requestProcessor.Process(request);
          client.Send(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(response)));
          Console.WriteLine("Sending response: {0}", JsonConvert.SerializeObject(response));
        }
      }
    }
  }
}
