using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using WatsonTcp;

namespace InjectionSoftware.Network
{
    public class Client
    {
        WatsonTcpClient tcpClient;

        string serverip;

        private readonly UdpClient udp = new UdpClient(14999);
        IAsyncResult ar_ = null;


        private void UDPStartListening()
        {
            ar_ = udp.BeginReceive(UDPReceive, new object());
        }
        private void UDPStopListening()
        {
            udp.Close();
        }

        // start listening for server message that will send the server ip back to client
        private void UDPReceive(IAsyncResult ar)
        {
            IPEndPoint ip = new IPEndPoint(IPAddress.Any, 14999);
            byte[] bytes = udp.EndReceive(ar, ref ip);
            string message = Encoding.ASCII.GetString(bytes);
            Console.WriteLine("[UDP] From {0} received: {1} ", ip.Address.ToString(), message);

            Console.WriteLine("[Client] Now adding ip address: " + ip.Address.ToString() + " as server.");
            serverip = ip.Address.ToString();
            ConnectToServer();

            UDPStartListening();
        }

        public void UDPSend(string message)
        {
            UdpClient client = new UdpClient();
            IPEndPoint ip = new IPEndPoint(IPAddress.Parse("255.255.255.255"), 15000);
            byte[] bytes = Encoding.ASCII.GetBytes(message);
            client.Send(bytes, bytes.Length, ip);
            client.Close();
            Console.WriteLine("[UDP] Sent: {0} ", message);
        }


        public Client()
        {
            // start listening from server message
            UDPStartListening();
            // broadcast to all ip, finding server
            UDPSend("connectionrequest");
        }

        public void ConnectToServer()
        {
            Console.Out.WriteLine(serverip);
            tcpClient = new WatsonTcpClient(serverip, 8901);
            tcpClient.MessageReceived += MessageReceived;


            try
            {
                Console.Out.WriteLine("[Client] Trying to connect to server with ip: " +serverip);
                tcpClient.Start();
            }
            catch (SocketException e)
            {
                Console.Out.WriteLine(e);
            }

            if (tcpClient.Connected)
            {
                Console.Out.WriteLine("[Client] Connection sucessful");
            }
            else
            {
                Console.Out.WriteLine("[Client] Connection failed");
            }
        }

        private void MessageReceived(object sender, MessageReceivedFromServerEventArgs args)
        {

        }
    }
}
