using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace InjectionSoftware.Network
{
    public class UDPNetworking
    {
        readonly int PORT_NUMBER;

        private readonly UdpClient udp;
        IAsyncResult ar_ = null;

        private bool isClosed = false;

        public UDPNetworking(int PORT_NUMBER)
        {
            this.PORT_NUMBER = PORT_NUMBER;
            udp =  new UdpClient(PORT_NUMBER);
            UDPStartListening();
        }

        public void UDPStartListening()
        {
            Console.WriteLine("[UDP] start listening on port:" + PORT_NUMBER);
            isClosed = false;
            ar_ = udp.BeginReceive(UDPReceive, new object());
        }
        public void UDPStopListening()
        {
            try
            {
                Console.Out.WriteLine("[UDP] stop listening on port:" + PORT_NUMBER);
                isClosed = true;
                udp.Close();
            }
            catch
            {

            }
            
        }

        // start listening for server message that will send the server ip back to client
        private void UDPReceive(IAsyncResult ar)
        {
            try
            {
                IPEndPoint ip = new IPEndPoint(IPAddress.Any, PORT_NUMBER);
                byte[] bytes = udp.EndReceive(ar, ref ip);
                string message = Encoding.ASCII.GetString(bytes);
                Console.WriteLine("[UDP] From {0} received: {1} ", ip.Address.ToString(), message);

                MessageRecievedEventArgs args = new MessageRecievedEventArgs();
                args.ipAddress = ip.Address.ToString();
                args.message = message;
                OnMessageRecieved(args);

                if (!isClosed)
                {
                    UDPStartListening();
                }
            }
            catch(ObjectDisposedException e)
            {
                Console.Out.WriteLine(e);
            }
            
        }

        public void UDPBroadCast(int targetPort, string message)
        {
            UdpClient client = new UdpClient();
            IPEndPoint ip = new IPEndPoint(IPAddress.Parse("255.255.255.255"), targetPort);
            byte[] bytes = Encoding.ASCII.GetBytes(message);
            client.Send(bytes, bytes.Length, ip);
            client.Close();
            Console.WriteLine("[UDP] Broadcast: {0} ", message);
        }

        public void UDPSend(string IPAddress, int targetPort, string message)
        {
            UdpClient client = new UdpClient();
            IPEndPoint ip = new IPEndPoint(System.Net.IPAddress.Parse(IPAddress), targetPort);
            byte[] bytes = Encoding.ASCII.GetBytes(message);
            client.Send(bytes, bytes.Length, ip);
            client.Close();
            Console.WriteLine("[UDP] Sent: {0} ", message);
        }

        protected virtual void OnMessageRecieved(MessageRecievedEventArgs e)
        {
            EventHandler<MessageRecievedEventArgs> handler = MessageRecieved;
            if (handler!= null)
            {
                handler(this, e);
            }
        }

        public event EventHandler<MessageRecievedEventArgs> MessageRecieved;

        public class MessageRecievedEventArgs : EventArgs
        {
            public string ipAddress;
            public string message;
        }
    }
}
