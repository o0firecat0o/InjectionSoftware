using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WatsonTcp;

namespace InjectionSoftware.Network
{
    public class Server
    {
        WatsonTcpServer tcpServer;


        private readonly UdpClient udp = new UdpClient(15000);
        IAsyncResult ar_ = null;

        /// <summary>
        /// start listening for any client broadcast
        /// </summary>
        private void UDPStartListening()
        {
            ar_ = udp.BeginReceive(UDPReceive, new object());
        }

        /// <summary>
        /// if recieved client broadcast, re-send the selfip to the client
        /// </summary>
        /// <param name="ar"></param>
        private void UDPReceive(IAsyncResult ar)
        {
            IPEndPoint ip = new IPEndPoint(IPAddress.Any, 15000);
            byte[] bytes = udp.EndReceive(ar, ref ip);
            string message = Encoding.ASCII.GetString(bytes);
            Console.WriteLine("[UDP] From {0} received: {1} ", ip.Address.ToString(), message);
            if (message == "connectionrequest")
            {

                UDPSend(ip.Address, "connectionaccepted");
            }
            UDPStartListening();
        }

        public void UDPSend(IPAddress address, string message)
        {
            UdpClient client = new UdpClient();
            IPEndPoint ip = new IPEndPoint(IPAddress.Parse("255.255.255.255"), 14999);
            byte[] bytes = Encoding.ASCII.GetBytes(message);
            client.Send(bytes, bytes.Length, ip);
            client.Close();
            Console.WriteLine("[UDP] Sent: {0} ", message);
        }

        public static string GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            throw new Exception("No network adapters with an IPv4 address in the system!");
        }

        public Server()
        {
            tcpServer = new WatsonTcpServer(GetLocalIPAddress(), 8901);

            tcpServer.MessageReceived += MessageReceived;
            tcpServer.ClientConnected += ClientConnected;
            tcpServer.ClientDisconnected += ClientDisconnected;            

            tcpServer.Start();

            UDPStartListening();
        }

        private void MessageReceived(object sender, MessageReceivedFromClientEventArgs args)
        {

        }

        void ClientConnected(object sender, ClientConnectedEventArgs args)
        {
            Console.WriteLine("[Server] Client connected: " + args.IpPort);
        }

        void ClientDisconnected(object sender, ClientDisconnectedEventArgs args)
        {
            Console.WriteLine("[Server] Client disconnected: " + args.IpPort + ": " + args.Reason.ToString());
        }
    }
}
