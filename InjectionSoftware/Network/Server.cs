using SimpleTCP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace InjectionSoftware.Network
{
    public class Server
    {
        SimpleTcpServer server;
        
        public Server()
        {
            server = new SimpleTcpServer().Start(8910);
            if (server.IsStarted)
            {
                Console.Out.WriteLine("[Server] Hosting server success");
            }
            else
            {
                Console.Out.WriteLine("[Server] Fail to host server");
            }
            server.ClientConnected += ClientConnected;

            server.Delimiter = 0x13;
        }

        public void BroadCast(string broadcastString)
        {
            server.BroadcastLine(broadcastString);
        }

        public void ClientConnected(Object e, TcpClient client)
        {
            Console.Out.WriteLine("[Server] A client: " + ((IPEndPoint)client.Client.RemoteEndPoint).Address.ToString() +" has connected");
        }
    }
}
