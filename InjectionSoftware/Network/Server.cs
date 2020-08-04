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

        UDPNetworking uDPNetworking = new UDPNetworking(15000);

        public event EventHandler<MessageReceivedFromClientEventArgs> MessageReceivedFromClientEvent;

        public event EventHandler<ClientConnectedEventArgs> ClientConnectedEvent;

        public event EventHandler<ClientDisconnectedEventArgs> ClientDisconnectedEvent;

        public Server()
        {
            tcpServer = new WatsonTcpServer(NetworkUtil.GetLocalIPAddress(), 8901);

            tcpServer.MessageReceived += MessageReceived;
            tcpServer.ClientConnected += ClientConnected;
            tcpServer.ClientDisconnected += ClientDisconnected;            

            tcpServer.Start();

            uDPNetworking.UDPStartListening();
            uDPNetworking.MessageRecieved += UDPMessageReceived;
        }

        private void UDPMessageReceived(object sender, UDPNetworking.MessageRecievedEventArgs e)
        {
            if(e.message == "connectionrequest")
            {
                uDPNetworking.UDPSend(e.ipAddress, 14999, "connectionaccepted"+"_"+ NetworkUtil.GetMachineName());
            }
        }

        private void MessageReceived(object sender, MessageReceivedFromClientEventArgs args)
        {
            MessageReceivedFromClientEvent(sender, args);
        }

        void ClientConnected(object sender, ClientConnectedEventArgs args)
        {
            Console.WriteLine("[Server] Client connected: " + args.IpPort);
            ClientConnectedEvent(sender, args);
        }

        void ClientDisconnected(object sender, ClientDisconnectedEventArgs args)
        {
            Console.WriteLine("[Server] Client disconnected: " + args.IpPort + ": " + args.Reason.ToString());
            ClientDisconnectedEvent(sender, args);
        }
    }
}
