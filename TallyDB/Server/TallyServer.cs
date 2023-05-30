using System.Net;
using System.Net.Sockets;
using Newtonsoft.Json;
using System.Text;
using TallyDB.Server.Types;
using TallyDB.Server.QueryProcessor;

namespace TallyDB.Server
{
    public class TallyServer
  {
    int port = 4053;
    IPAddress localAddr = IPAddress.Parse("127.0.0.1");

    public TallyServer(int port)
    {
      this.port = port;
    }

    public TallyServer() { }

    public void StartServer()
    {
      TcpListener listener = new TcpListener(localAddr, port);
      listener.Start();

      while (true)
      {
        Socket client = listener.AcceptSocket();

        var childSocketThread = new Thread(() =>
        {
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
        });

        childSocketThread.Start();
      }
    }
  }
}
