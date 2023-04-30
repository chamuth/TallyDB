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
            TcpListener listener = new TcpListener(localAddr, port);
            listener.Start();

            while (true)
            {
                Socket client = listener.AcceptSocket();

                var childSocketThread = new Thread(() =>
                {
                    byte[] data = new byte[100];
                    int size = client.Receive(data);
                    Console.WriteLine("Recieved data: ");

                    for (int i = 0; i < size; i++)
                    {
                        Console.Write(Convert.ToChar(data[i]));
                    }

                    Console.WriteLine();

                    client.Close();
                });

                childSocketThread.Start();
            }
        }
    }
}
