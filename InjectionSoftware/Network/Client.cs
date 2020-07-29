using SimpleTCP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace InjectionSoftware.Network
{
    public class Client
    {
        SimpleTcpClient client = new SimpleTcpClient();
        string serverIP;

        public Client(string serverIP)
        {
            this.serverIP = serverIP;
            Connect();
            client.Delimiter = 0x13;
            client.DelimiterDataReceived += DataReceived;
        }

        public async void Connect()
        {
            try
            {

                client.Connect(serverIP, 8910);
                Console.Out.WriteLine("[Client] Trying to connect");                

                if (client.TcpClient.Connected)
                {
                    Console.Out.WriteLine("[Client] Client is initialized, connection successful");
                }
                else
                {
                    Console.Out.WriteLine("[Client] Client initialization failed, connection unsuccessful");
                    await Task.Delay(2000);
                }
            }
            catch (System.Exception)
            {
                Console.Out.WriteLine("[Client] Client initialization failed");
            }
        }

        public void DataReceived(object e, Message message)
        {
            Console.Out.WriteLine("[Client] the following message is recived: ");
            Console.Out.WriteLine(message.MessageString);
        }
    }
}
