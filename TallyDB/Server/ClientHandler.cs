using Newtonsoft.Json;
using System.Net.Sockets;
using System.Text;
using TallyDB.Config;
using TallyDB.Config.Auth;
using TallyDB.Server.QueryProcessor;
using TallyDB.Server.Types;

namespace TallyDB.Server
{
  public class ClientHandler
  {
    /// <summary>
    /// Handle each client being connected to the server
    /// </summary>
    /// <param name="client">client socket connection</param>
    public void Handle(Socket client)
    {
      var errors = new List<DatabaseError>();
      User? authUser = null;
      var authStorage = new AuthStorage(new FileOperator());
      var requestProcessor = new RequestProcessor();

      while (true)
      {
        byte[] data = new byte[2048];
        int size = client.Receive(data);

        string value = "";

        for (int i = 0; i < size; i++)
        {
          value += (Convert.ToChar(data[i]));
        }

        var request = JsonConvert.DeserializeObject<QueryRequest>(value);

        if (request != null)
        {
          if (request.Credentials != null)
          {
            // Handle authentication
            var creds = request.Credentials;
            authUser = authStorage.Authenticate(creds.Username, creds.Password);

            if (authUser == null)
            {
              errors.Add(
                new DatabaseError("2", "Invalid credentials", "Please use correct credentials to authenticate user")
              );
            }
          }

          QueryResponse? response = new QueryResponse(request.RequestId);

          if (authUser == null)
          {
            errors.Add(
              new DatabaseError("1", "User not authenticated", "Authenticate first before making any request")
            );
          }
          else
          {
            response = requestProcessor.Process(request);
          }

          Console.WriteLine("Request received: {0}", value);
          response.Errors = errors.ToArray();
          client.Send(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(response)));
          errors.Clear();
          Console.WriteLine("Sending response: {0}", JsonConvert.SerializeObject(response));
        }
      }
    }
  }
}
