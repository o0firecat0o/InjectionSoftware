using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;
using WatsonTcp;

namespace InjectionSoftware.Network
{
    public class Client
    {
        WatsonTcpClient tcpClient;

        public string serverip;
        public string servername;

        DispatcherTimer timer;

        UDPNetworking uDPNetworking = new UDPNetworking(14999);

        public event EventHandler<EventArgs> ServerFound;

        public event EventHandler<EventArgs> ServerConnected;

        public event EventHandler<EventArgs> ServerDisconnected;

        public Client()
        {
            // start listening from server message
            uDPNetworking.UDPStartListening();

            uDPNetworking.MessageRecieved += UDPMessageReceived;

            FindServer();
        }

        public void FindServer()
        {
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(5000);
            timer.Start();
            timer.Tick += new EventHandler(delegate (object s, EventArgs a)
            {
                // broadcast to all ip, finding server
                uDPNetworking.UDPBroadCast(15000, "connectionrequest");
            });
        }

        private void UDPMessageReceived(object sender, UDPNetworking.MessageRecievedEventArgs e)
        {
            if (e.message.Contains("connectionaccepted"))
            {
                string[] messages = e.message.Split('_');

                servername = messages[1];
                serverip = e.ipAddress;

                Console.Out.WriteLine("[Client] ServerFound!");
                Console.Out.WriteLine("[Client] Setting IPAddress: " + e.ipAddress + " as Server Address");
                Console.Out.WriteLine("[Client] Name of the server is: " + servername);

                ServerFound(this, new EventArgs());
            }
        }


        public void StopUDP()
        {
            Console.Out.WriteLine("[Client] terminating UDP client");
            timer.Stop();
            uDPNetworking.UDPStopListening();
        }

        public void ConnectToServer()
        {
            tcpClient = new WatsonTcpClient(serverip, 8901);
            tcpClient.MessageReceived += MessageReceived;
            tcpClient.ServerDisconnected += _ServerDisconnected;

            Thread.Sleep(3000);

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
                ServerConnected(this, new EventArgs());
                StopUDP();
            }
            else
            {
                Console.Out.WriteLine("[Client] Connection failed");
            }
        }

        public void TCPSendMessageToServer(string messageType, string message)
        {
            tcpClient.Send(messageType + "_" +message);
        }

        private void _ServerDisconnected(object sender, EventArgs e)
        {
            ServerDisconnected(this, e);
        }

        private void MessageReceived(object sender, MessageReceivedFromServerEventArgs args)
        {

        }
    }
}
