using System;
using System.Collections;
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

        private List<string> clientIPs = new List<string>();

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
            if (e.message == "connectionrequest")
            {
                uDPNetworking.UDPSend(e.ipAddress, 14999, "connectionaccepted" + "_" + NetworkUtil.GetMachineName());
            }
        }

        public void TCPBroadcastMessage(string messageType, string message)
        {
            //TODO: stop referencing clientviewobjects? maybe move it to a seperate dataclass?
            Console.Out.WriteLine("[Server] broadcasting message of Type: {0}", messageType);
            foreach (ClientViewObject clientViewObject in ClientViewObject.clientViewObjects)
            {
                tcpServer.Send(clientViewObject.fullIP, messageType + "_" + message);
            }
        }

        private void MessageReceived(object sender, MessageReceivedFromClientEventArgs args)
        {
            MessageReceivedFromClientEvent(sender, args);
        }

        void ClientConnected(object sender, ClientConnectedEventArgs args)
        {
            Console.WriteLine("[Server] Client connected: " + args.IpPort);
            clientIPs.Add(args.IpPort);
            ClientConnectedEvent(sender, args);
        }

        void ClientDisconnected(object sender, ClientDisconnectedEventArgs args)
        {
            Console.WriteLine("[Server] Client disconnected: " + args.IpPort + ": " + args.Reason.ToString());
            clientIPs.Remove(args.IpPort);
            ClientDisconnectedEvent(sender, args);
        }
    }
}
