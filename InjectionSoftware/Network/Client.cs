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

        UDPNetworking uDPNetworking = new UDPNetworking(14999);

        public Client()
        {
            // start listening from server message
            uDPNetworking.UDPStartListening();
            // broadcast to all ip, finding server
            uDPNetworking.UDPBroadCast(15000,"connectionrequest");
            uDPNetworking.MessageRecieved += UDPMessageReceived;
        }

        private void UDPMessageReceived(object sender, UDPNetworking.MessageRecievedEventArgs e)
        {
            if(e.message == "connectionaccepted")
            {
                serverip = e.ipAddress;
                Console.Out.WriteLine("[Client] Setting IPAddress: " + e.ipAddress + " as Server Address");
                ConnectToServer();
            }
        }

        public void ConnectToServer()
        {
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
