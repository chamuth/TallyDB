using System.Net;
using System.Net.Sockets;

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
      DatabaseManager.LoadDatabases();

      IPEndPoint ep = new IPEndPoint(IPAddress.Any, port);
      TcpListener listener = new TcpListener(ep);
      listener.Start();

      Console.WriteLine("Started TallyDB server at {0} 🚀", port);

      while (true)
      {
        Console.WriteLine("Waiting for socket connection");
        Socket client = listener.AcceptSocket();
        Console.WriteLine("ACCEPTED SOCKET CONNECTION");
        Task.Run(() => new ClientHandler().Handle(client));
      }
    }
  }
}